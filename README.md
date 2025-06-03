# NoruBanner

## Описание
NoruBanner - это система отслеживания взаимодействия пользователей с баннерами на веб-страницах. Система автоматически отслеживает просмотры (когда баннер виден минимум на 50%) и клики по баннерам, сохраняет статистику и предоставляет API для её получения.

## Технологии

### Бэкенд
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- MediatR (CQRS pattern)
- Swagger/OpenAPI

### Фронтенд
- JavaScript (Vanilla)
- Intersection Observer API
- Local Storage для оффлайн функциональности
- Nginx

## API Endpoints

### Tracking API
```http
POST /api/v1/tracking/event
```
Записывает событие просмотра или клика по баннеру.

**Тело запроса:**
```json
{
  "bannerId": "123e4567-e89b-4456-8af1-123e4567abcd",
  "userSessionId": "session-98765432-1234-5678-9012-345678901234",
  "eventType": "Viewed" // или "Clicked"
}
```

### Statistics API
```http
GET /api/v1/statistics/banner/{bannerId}
```
Возвращает статистику по конкретному баннеру.

**Ответ:**
```json
{
  "bannerId": "123e4567-e89b-4456-8af1-123e4567abcd",
  "totalViews": 100,
  "totalClicks": 15,
  "uniqueUsers": 80,
  "clickThroughRate": 0.15
}
```

## Запуск проекта

### Предварительные требования
- Docker Desktop
- Docker Compose
- PowerShell

### Шаги по запуску

1. Клонируйте репозиторий:
```powershell
git clone <repository-url>
cd NoruBanner
```

2. Запустите проект через Docker Compose:
```powershell
cd Server/NoruBanner
docker compose up --build -d
```

3. Проверьте работу приложения:
- Frontend: http://localhost:80
- API: [http://localhost:8080]
- Swagger UI: [http://localhost:8080/swagger/index.html]

### Интеграция на страницу

1. Добавьте баннер на страницу:
```html
<div class="noru-banner" data-banner-id="ваш-guid-баннера">
    <!-- содержимое баннера -->
</div>
```

2. Подключите скрипт отслеживания:
```html
<script src="/js/bannerTracker.js"></script>
```

Трекер автоматически начнет отслеживать просмотры и клики по баннерам.
