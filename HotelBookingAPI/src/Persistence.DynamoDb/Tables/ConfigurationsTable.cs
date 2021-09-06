using Amazon.DynamoDBv2;
using Domain.Models;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using Persistence.Mappers;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class ConfigurationsTable : DynamoDbClient<ConfigurationPersistence>, IConfigurationsRepository
    {
        public ConfigurationsTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Configurations")
        {
        }

        public override string HashKey => "Id";
        public override string RangeKey => "";

        public async Task<Configuration> GetHotelConfiguration()
        {
            return (await GetItemById("HotelConfiguration")).AsDomainModel();
        }
    }
}
