const EventType = {
  Viewed: 1,
  Clicked: 2,
};

class BannerTracker {
  constructor() {
    const baseUrl = window.location.protocol + "//" + window.location.host;
    this._apiUrl = `${baseUrl}/api/tracking/event`;
    this._sessionId = this._getOrCreateSessionId();
    this._retryDelay = 1000;
    this._maxRetries = 3;
    this._pendingEvents = new Map();
    this._failedEvents = [];
    this._networkStatus = { isOnline: true, lastCheck: Date.now() };
    this._offlineStorageKey = "noru_offline_events";
    this._validBannerIds = new Set();

    this._setupNetworkMonitoring();
    this._recoverOfflineEvents();
    this._initTracking();
  }

  _getOrCreateSessionId() {
    let sessionId = localStorage.getItem("noru_session_id");
    if (!sessionId) {
      sessionId = "session-" + crypto.randomUUID();
      localStorage.setItem("noru_session_id", sessionId);
    }
    return sessionId;
  }

  _setupNetworkMonitoring() {
    window.addEventListener("online", () => {
      this._networkStatus.isOnline = true;
      this._networkStatus.lastCheck = Date.now();
      this._processPendingEvents();
    });

    window.addEventListener("offline", () => {
      this._networkStatus.isOnline = false;
      this._networkStatus.lastCheck = Date.now();
    });
  }

  _saveOfflineEvent(event) {
    const offlineEvents = JSON.parse(
      localStorage.getItem(this._offlineStorageKey) || "[]"
    );
    offlineEvents.push({
      ...event,
      timestamp: Date.now(),
    });
    localStorage.setItem(
      this._offlineStorageKey,
      JSON.stringify(offlineEvents)
    );
  }

  async _recoverOfflineEvents() {
    const offlineEvents = JSON.parse(
      localStorage.getItem(this._offlineStorageKey) || "[]"
    );
    if (!offlineEvents && offlineEvents.length > 0) {
      localStorage.removeItem(this._offlineStorageKey);

      for (const event of offlineEvents) {
        await this._trackEvent(event.bannerId, event.eventType);
      }
    }
  }

  _validateBannerId(bannerId) {
    if (typeof bannerId !== "string") {
      console.error("Banner ID must be a string, received:", typeof bannerId);
      return false;
    }

    if (this._validBannerIds.has(bannerId)) {
      return true;
    }

    const uuidRegex =
      /^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    const isValid = uuidRegex.test(bannerId);

    if (!isValid) {
      console.error(
        `Invalid banner ID format: ${bannerId}. Expected a valid UUID v4 (e.g., 123e4567-e89b-4456-8af1-123e4567abcd)`
      );
    }

    return isValid;
  }

  _initTracking() {
    const banners = document.querySelectorAll(".noru-banner");
    if (banners.length === 0) {
      console.warn("No banner elements found with class 'noru-banner'");
      return;
    }

    banners.forEach((banner) => {
      const bannerId = banner.dataset.bannerId;
      if (!bannerId) {
        console.error(
          "Banner element missing data-banner-id attribute:",
          banner
        );
        return;
      }

      if (this._validateBannerId(bannerId)) {
        this._validBannerIds.add(bannerId);
      } else {
        banner.style.border = "2px solid red";
        console.error("Invalid banner ID format. Banner element:", banner);
      }
    });

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting && entry.intersectionRatio >= 0.5) {
            const bannerId = entry.target.dataset.bannerId;
            if (bannerId && this._validateBannerId(bannerId)) {
              this._trackEvent(bannerId, EventType.Viewed);
            } else {
              console.error("Invalid or missing banner-id attribute");
            }
          }
        });
      },
      { threshold: [0.5] }
    );

    banners.forEach((banner) => {
      observer.observe(banner);
      banner.addEventListener("click", () => {
        const bannerId = banner.dataset.bannerId;
        if (bannerId && this._validateBannerId(bannerId)) {
          this._trackEvent(bannerId, EventType.Clicked);
        } else {
          console.error("Invalid or missing banner-id attribute");
        }
      });
    });
  }

  async _processPendingEvents() {
    if (!this._networkStatus.isOnline) {
      return;
    }

    const currentFailedEvents = [...this._failedEvents];
    this._failedEvents = [];

    for (const event of currentFailedEvents) {
      await this._trackEvent(event.bannerId, event.eventType, event.retryCount);
    }
  }

  async _trackEvent(bannerId, eventType, retryCount = 0) {
    if (!this._validateBannerId(bannerId)) {
      console.error("Invalid bannerId format:", bannerId);
      return;
    }

    if (![EventType.Viewed, EventType.Clicked].includes(eventType)) {
      console.error("Invalid event type:", eventType);
      return;
    }

    const eventId = `${bannerId}-${eventType}-${Date.now()}`;

    if (!this._networkStatus.isOnline) {
      this._saveOfflineEvent({ bannerId, eventType });
      return;
    }

    if (this._pendingEvents.has(eventId)) {
      return;
    }

    this._pendingEvents.set(eventId, {
      bannerId,
      eventType,
      timestamp: Date.now(),
    });

    try {
      const response = await fetch(this._apiUrl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          bannerId: bannerId,
          userSessionId: this._sessionId,
          eventType: eventType,
        }),
        signal: AbortSignal.timeout(5000),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(
          `HTTP error! status: ${response.status}, message: ${errorText}`
        );
      }

      this._pendingEvents.delete(eventId);
    } catch (error) {
      console.error("Error tracking banner event:", error);

      this._pendingEvents.delete(eventId);

      if (error instanceof TypeError && !this._networkStatus.isOnline) {
        this._saveOfflineEvent({ bannerId, eventType });
        return;
      }

      if (
        retryCount < this._maxRetries &&
        (error instanceof TypeError || error.name === "AbortError")
      ) {
        const delay = this._retryDelay * Math.pow(2, retryCount);
        console.log(
          `Retrying in ${delay}ms... (attempt ${retryCount + 1}/${
            this._maxRetries
          })`
        );

        this._failedEvents.push({
          bannerId,
          eventType,
          retryCount: retryCount + 1,
          nextRetry: Date.now() + delay,
        });

        await new Promise((resolve) => setTimeout(resolve, delay));
        await this._trackEvent(bannerId, eventType, retryCount + 1);
      }
    }
  }
}

document.addEventListener("DOMContentLoaded", () => {
  new BannerTracker();
});
