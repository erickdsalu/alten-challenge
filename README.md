## Pre-Requisites

Docker Desktop - https://www.docker.com/products/docker-desktop <br>
AWS CLI - https://aws.amazon.com/pt/cli/ <br>
.NET Core SDK 3.1 - https://dotnet.microsoft.com/download/dotnet/3.1

## Running Local DynamoDb

At folder "infra" will be the .yml file to run on docker-compose

```sh
cd infra
docker-compose up -d (version 3.3)
docker compose up -d (version 3.8+)
```
## Migrate Database

Execute migrate-tables.ps1 (PowerShell) or migrate-tables.sh (Shell) script for creating tables and initial data migration

```sh
.\migrate-tables.ps1
```

## Running Application

At the solution folder we can run the Web API project

```
dotnet run --project src/Webdotnet run --project src/Web
```
## Discovering API

After running the application, all endpoint documentation will be accessible under swagger
```
https://localhost:5001/swagger
```
