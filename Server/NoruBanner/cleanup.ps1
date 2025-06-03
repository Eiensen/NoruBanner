# Остановка и удаление контейнеров проекта
Write-Host "Stopping and removing containers..." -ForegroundColor Yellow
docker compose down

# Удаление неиспользуемых образов
Write-Host "Removing unused images..." -ForegroundColor Yellow
docker image prune -f --filter "label=com.docker.compose.project=norubanner"

# Удаление неиспользуемых томов
Write-Host "Removing unused volumes..." -ForegroundColor Yellow
docker volume prune -f --filter "label=com.docker.compose.project=norubanner"

# Удаление неиспользуемых сетей
Write-Host "Removing unused networks..." -ForegroundColor Yellow
docker network prune -f --filter "label=com.docker.compose.project=norubanner"

Write-Host "Cleanup completed!" -ForegroundColor Green
