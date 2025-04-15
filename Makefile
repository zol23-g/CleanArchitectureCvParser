# Constants
PROJECT_NAME=CVParser
STARTUP_PROJECT=Presentation
INFRA_PROJECT=Infrastructure
DB_NAME=CVParserDb
MIGRATION_NAME?=AutoMigration
CONNECTION="Host=localhost;Port=5432;Database=$(DB_NAME);Username=postgres;Password=password"

# .PHONY prevents these from clashing with files
.PHONY: run build migrate update remove clean test swagger

## 🏃 Run the application
run:
	dotnet run --project $(STARTUP_PROJECT)

## 🛠️ Build all projects
build:
	dotnet build

## 📦 Add a new migration
migrate:
	dotnet ef migrations add $(MIGRATION_NAME) --project $(INFRA_PROJECT) --startup-project $(STARTUP_PROJECT)

## 🗃️ Apply the latest migration to the database
update:
	dotnet ef database update --project $(INFRA_PROJECT) --startup-project $(STARTUP_PROJECT)

## 🔙 Remove the last migration (only if not applied)
remove:
	dotnet ef migrations remove --project $(INFRA_PROJECT) --startup-project $(STARTUP_PROJECT)

## 🧪 Run tests (if you have a test project)
test:
	dotnet test

## 🧹 Clean solution
clean:
	dotnet clean

## 🌐 Open Swagger UI (assumes port 5186)

swagger:
	explorer.exe http://localhost:5186/swagger

