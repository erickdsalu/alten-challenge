using Amazon.DynamoDBv2;
using Application.Queries.Reservations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DynamoDb.Tables;
using Persistence.Interfaces;
using System.Reflection;

namespace DependencyInjection
{
    public static class Bootstraper
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(GetReservationQuery).GetTypeInfo().Assembly);

            var dynamoDbConfig = configuration.GetSection("DynamoDb");
            var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }


            services.AddScoped<IReservationsRepository, ReservationsTable>();
            services.AddScoped<IConfigurationsRepository, ConfigurationsTable>();
            services.AddScoped<IRoomsRepository, RoomsTable>();
            services.AddScoped<ICustomersRepository, CustomersTable>();
        }
    }
}
