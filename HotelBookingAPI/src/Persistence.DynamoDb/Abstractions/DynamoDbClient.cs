using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Extensions.Paging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Abstractions
{
    public abstract class DynamoDbClient<T> : DynamoDbTable
    {
        public abstract string HashKey { get; }
        public abstract string RangeKey { get; }

        protected DynamoDbClient(IAmazonDynamoDB amazonDynamoDB, string tableName) : base(amazonDynamoDB, tableName)
        {
        }

        public async Task<T> GetItemById(string hashKeyValue, string rangeKeyValue = "")
        {
            var getItemInput = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue> {
                    { HashKey, new AttributeValue { S = hashKeyValue } }
                }
            };

            if (!string.IsNullOrEmpty(RangeKey))
                getItemInput.Key.Add(RangeKey, new AttributeValue { S = rangeKeyValue });

            var response = await Database.GetItemAsync(getItemInput);


            return response.Item.Any<KeyValuePair<string, AttributeValue>>() ?
                JsonConvert.DeserializeObject<T>(Document.FromAttributeMap(response.Item).ToJson()) : (T)default;
        }

        public async Task PutItem(T item)
        {
            var document = Document.FromJson(JsonConvert.SerializeObject(item));

            await Database.PutItemAsync(new PutItemRequest
            {
                TableName = TableName,
                Item = document.ToAttributeMap()
            });
        }

        public async Task<PageModel<T>> Scan(PagingRequest pagingRequest, bool? active = null)
        {
            bool shouldKeepLooking = true;
            PageModel<T> pageResult = new PageModel<T>
            {
                LastIndex = pagingRequest.LastIndex,
                Items = new List<T>()
            };

            do
            {
                var scanRequest = new ScanRequest
                {
                    TableName = TableName,
                    Limit = pagingRequest.Limit - pageResult.Items.Count(),
                    ExclusiveStartKey = string.IsNullOrEmpty(pagingRequest.LastIndex) ? null : new Dictionary<string, AttributeValue>
                    {
                        {
                            HashKey, new AttributeValue
                            {
                                S = pagingRequest.LastIndex
                            }
                        }
                    }
                };

                if (pagingRequest.StartDate.HasValue && pagingRequest.EndDate.HasValue)
                {
                    scanRequest.FilterExpression = "StartDate BETWEEN :startDate AND :endDate Or EndDate BETWEEN :startDate AND :endDate";
                    scanRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":startDate", new AttributeValue{ S = pagingRequest.StartDate.Value.ToString("yyyy-MM-dd") } },
                        { ":endDate", new AttributeValue{ S = pagingRequest.EndDate.Value.ToString("yyyy-MM-dd") } }
                    };
                }
                else if (pagingRequest.StartDate.HasValue)
                {
                    scanRequest.FilterExpression = "StartDate > :startDate Or EndDate > :startDate";
                    scanRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":startDate", new AttributeValue{ S = pagingRequest.StartDate.Value.ToString("yyyy-MM-dd") } }
                    };
                }
                else if (pagingRequest.StartDate.HasValue)
                {
                    scanRequest.FilterExpression = "StartDate < :endDate";
                    scanRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":endDate", new AttributeValue{ S = pagingRequest.EndDate.Value.ToString("yyyy-MM-dd") } }
                    };
                }

                if (pagingRequest.ExcludentIds.Any())
                {
                    scanRequest.FilterExpression += string.IsNullOrEmpty(scanRequest.FilterExpression) ? "" : " And";
                    scanRequest.FilterExpression += $"NOT ({HashKey} in ({string.Join("", pagingRequest.ExcludentIds)}))";
                }

                if (active.HasValue)
                {
                    scanRequest.FilterExpression += string.IsNullOrEmpty(scanRequest.FilterExpression) ? "" : " And";
                    scanRequest.FilterExpression += $"IsActive = :isActive";
                    scanRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":isActive", new AttributeValue{ BOOL = active.Value } }
                    };
                }

                var result = await Database.ScanAsync(scanRequest);


                var itemList = result.Items.Select(item => item.Any<KeyValuePair<string, AttributeValue>>() ?
                JsonConvert.DeserializeObject<T>(Document.FromAttributeMap(item).ToJson()) : (T)default);

                ((List<T>)pageResult.Items).AddRange(itemList);

                bool GotAllNeededItems()
                {
                    return result.Count == pagingRequest.Limit;
                }

                bool HasMoreItensInDatabase()
                {
                    return result.LastEvaluatedKey.Any();
                }

                if (!HasMoreItensInDatabase())
                {
                    pageResult.LastIndex = null;
                    shouldKeepLooking = false;
                }
                else if (GotAllNeededItems())
                {
                    pageResult.LastIndex = result.LastEvaluatedKey.First().Value.S;
                    shouldKeepLooking = false;
                }
                else
                {
                    pageResult.LastIndex = result.LastEvaluatedKey.First().Value.S;
                }

            } while (shouldKeepLooking);

            return pageResult;
        }

        public async Task<PageModel<T>> Query(PagingRequest pagingRequest, string hashKeyValue,
            string globalSecondaryIndexName = "", string globalSecondaryIndexKey = "", string globalSecondaryIndexValue = "")
        {
            bool shouldKeepLooking = true;
            PageModel<T> pageResult = new PageModel<T>
            {
                LastIndex = pagingRequest.LastIndex,
                Items = new List<T>()
            };

            do
            {
                var queryRequest = new QueryRequest
                {
                    TableName = TableName,
                    Limit = pagingRequest.Limit - pageResult.Items.Count(),
                    ExclusiveStartKey = string.IsNullOrEmpty(pagingRequest.LastIndex) ? null : new Dictionary<string, AttributeValue>
                    {
                        {
                            HashKey, new AttributeValue
                            {
                                S = pagingRequest.LastIndex
                            }
                        }
                    },
                    KeyConditionExpression = $"#HashKey = :hashKeyValue",
                    ExpressionAttributeNames = new Dictionary<string, string> {
                        {"#HashKey", HashKey}
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        { ":hashKeyValue", new AttributeValue{ S = hashKeyValue } }
                    },
                };

                if (!string.IsNullOrEmpty(globalSecondaryIndexName))
                {
                    queryRequest.IndexName = globalSecondaryIndexName;
                    queryRequest.KeyConditionExpression = $"#HashKey = :hashKeyValue";
                    queryRequest.ExpressionAttributeNames = new Dictionary<string, string> {
                        {"#HashKey", globalSecondaryIndexKey}
                    };
                    queryRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        { ":hashKeyValue", new AttributeValue{ S = globalSecondaryIndexValue } }
                    };
                }

                if (pagingRequest.StartDate.HasValue && pagingRequest.EndDate.HasValue)
                {
                    queryRequest.FilterExpression = "StartDate BETWEEN :startDate AND :endDate Or EndDate BETWEEN :startDate AND :endDate";
                    queryRequest.ExpressionAttributeValues.Add(":startDate", new AttributeValue { S = pagingRequest.StartDate.Value.ToString("yyyy-MM-dd") });
                    queryRequest.ExpressionAttributeValues.Add(":endDate", new AttributeValue { S = pagingRequest.EndDate.Value.ToString("yyyy-MM-dd") });
                }
                else if (pagingRequest.StartDate.HasValue)
                {
                    queryRequest.FilterExpression = "StartDate > :startDate Or EndDate > :startDate";
                    queryRequest.ExpressionAttributeValues.Add(":startDate", new AttributeValue { S = pagingRequest.StartDate.Value.ToString("yyyy-MM-dd") });
                }
                else if (pagingRequest.StartDate.HasValue)
                {
                    queryRequest.FilterExpression = "StartDate < :endDate";
                    queryRequest.ExpressionAttributeValues.Add(":endDate", new AttributeValue { S = pagingRequest.EndDate.Value.ToString("yyyy-MM-dd") });
                }

                var result = await Database.QueryAsync(queryRequest);


                var itemList = result.Items.Select(item => item.Any<KeyValuePair<string, AttributeValue>>() ?
                JsonConvert.DeserializeObject<T>(Document.FromAttributeMap(item).ToJson()) : (T)default);

                ((List<T>)pageResult.Items).AddRange(itemList);

                bool GotAllNeededItems()
                {
                    return result.Count == pagingRequest.Limit;
                }

                bool HasMoreItensInDatabase()
                {
                    return result.LastEvaluatedKey.Any();
                }

                if (!HasMoreItensInDatabase())
                {
                    pageResult.LastIndex = null;
                    shouldKeepLooking = false;
                }
                else if (GotAllNeededItems())
                {
                    pageResult.LastIndex = result.LastEvaluatedKey.First().Value.S;
                    shouldKeepLooking = false;
                }
                else
                {
                    pageResult.LastIndex = result.LastEvaluatedKey.First().Value.S;
                }

            } while (shouldKeepLooking);

            return pageResult;
        }

    }
}
