class BannerTracker {
  constructor() {
    this.apiUrl = "http://norubanner.webapi/api/tracking/event";
    this.sessionId = this.getOrCreateSessionId();
    this.initTracking();
  }

  getOrCreateSessionId() {
    let sessionId = localStorage.getItem("noru_session_id");
    if (!sessionId) {
      sessionId = "session-" + Math.random().toString(36).substring(2, 15);
      localStorage.setItem("noru_session_id", sessionId);
    }
    return sessionId;
  }

  initTracking() {
    const banners = document.querySelectorAll(".noru-banner");

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            this.trackEvent(entry.target.dataset.bannerId, "Viewed");
          }
        });
      },
      { threshold: 0.5 }
    );

    banners.forEach((banner) => {
      observer.observe(banner);
      banner.addEventListener("click", () => {
        this.trackEvent(banner.dataset.bannerId, "Clicked");
      });
    });
  }

  async trackEvent(bannerId, eventType) {
    try {
      const response = await fetch(this.apiUrl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          bannerId: bannerId,
          userSessionId: this.sessionId,
          eventType: eventType,
        }),
      });

      if (!response.ok) {
        console.error("Tracking error:", response.status);
      }
    } catch (error) {
      console.error("Tracking failed:", error);
    }
  }
}

document.addEventListener("DOMContentLoaded", () => {
  new BannerTracker();
});
