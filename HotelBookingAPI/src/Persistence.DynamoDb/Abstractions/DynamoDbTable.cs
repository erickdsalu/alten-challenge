using Amazon.DynamoDBv2;

namespace Persistence.DynamoDb.Abstractions
{
    public abstract class DynamoDbTable
    {
        public IAmazonDynamoDB Database { get; }
        public string TableName { get; }

        protected DynamoDbTable(IAmazonDynamoDB amazonDynamoDB, string tableName)
        {
            Database = amazonDynamoDB;
            TableName = tableName;
        }

    }
}
