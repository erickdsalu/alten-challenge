using Amazon.DynamoDBv2;
using Domain.Models;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class ConfigurationsTable : DynamoDbClient<HotelConfiguration>, IConfigurationsRepository
    {
        public ConfigurationsTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Configurations")
        {
        }

        public override string HashKey => "Id";
        public override string RangeKey => "";

        public async Task<HotelConfiguration> GetHotelConfiguration()
        {
            return await GetItemById("HotelConfiguration");
        }
    }
}
