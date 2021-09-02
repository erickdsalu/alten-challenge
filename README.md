## Pre-Requisites

Docker Desktop - https://www.docker.com/products/docker-desktop
AWS CLI - https://aws.amazon.com/pt/cli/
.NET Core SDK 3.1 - https://dotnet.microsoft.com/download/dotnet/3.1

## Running Local DynamoDb

At folder "infra" will be the .yml file to run on docker-compose

```sh
cd infra
docker compose up -d
```
## Migrate Database

Execute migrate-tables.ps1 (PowerShell) or migrate-tables.sh (Unix Shell) script for creating tables

```sh
.\migrate-tables.ps1
```

## Migrate Data

Run the aws cli command below to add configuration itens at Configuration table

```sh
aws dynamodb put-item --table-name Configurations --item file://tables/Configuration.json --endpoint-url http://localhost:8000
```

## Running Application
```
dotnet run --project src/Webdotnet run --project src/Web
```
## Discovering API
After running the application, all endpoint documentation will be accessible under swagger
```
https://localhost:5001/swagger
```