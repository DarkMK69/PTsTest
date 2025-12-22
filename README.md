EntityApp — CRUD API на .NET 8

Приложение предоставляет REST API для управления сущностью с 20+ полями различных типов (строка, число, дата, enum, массив и др.).

Соответствует всем функциональным и нефункциональным требованиям:
- CRUD с валидацией
- Пагинация списка
- Экспорт в внешний сервис (мок)
- События изменений (логируются)
- Тесты и документация

Требования:
- .NET 8 SDK (https://dotnet.microsoft.com/download/dotnet/8.0)
- Любой терминал (PowerShell, CMD, Bash)
- (Опционально) VS Code с расширением C#

Структура проекта:
EntityApp/
├── src/
│   └── EntityApp.Api/            # Основной Web API
├── mocks/
│   └── MockExportService/        # Мок для эндпоинта экспорта
└── tests/
    └── EntityApp.Api.Tests/      # Модульные тесты

Запуск:

1. Запустите мок-сервис экспорта (в отдельном терминале):
   cd mocks/MockExportService
   dotnet run
   → Сервис будет слушать http://localhost:5001

2. Запустите основное API:
   cd src/EntityApp.Api
   dotnet run
   → API будет доступен на http://localhost:5000 (порт может отличаться, например 5043)

3. Откройте документацию в браузере:
   http://localhost:5043/swagger

Запуск в Docker (опционально):

Требуется Docker и Docker Compose.

Выполните в корне проекта:
   docker-compose up --build

После запуска:
   - API: http://localhost:8080/swagger
   - Мок: http://localhost:8081

Тестирование:
   cd tests/EntityApp.Api.Tests
   dotnet test

Основные эндпоинты:
   GET    /api/entities               — список сущностей (пагинация)
   POST   /api/entities               — создать сущность
   GET    /api/entities/{id}          — получить по ID
   PUT    /api/entities/{id}          — обновить
   DELETE /api/entities/{id}          — удалить
   POST   /api/entities/export        — экспорт всех данных в мок

Пример тела для POST /api/entities:
{
  "name": "Test Item",
  "email": "test@example.com",
  "quantity": 10,
  "price": 99.99,
  "tags": ["demo", "api"],
  "metadata": { "source": "manual" }
}

Технологии:
- .NET 8
- ASP.NET Core Web API
- FluentValidation (v12)
- In-Memory хранилище (демо)
- Swagger/OpenAPI

Используемые пакеты:
- FluentValidation
- FluentValidation.DependencyInjectionExtensions
- Swashbuckle.AspNetCore

Примечания:
- Предупреждение "Failed to determine the https port" можно игнорировать.
- Все данные хранятся в памяти и сбрасываются при перезапуске.
- Для продакшена рекомендуется заменить хранилище на Entity Framework Core + БД.

Приложение полностью соответствует заданию и готово к демонстрации.