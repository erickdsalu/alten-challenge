using Amazon.DynamoDBv2;
using Domain.Models;
using Extensions.Paging;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class RoomsTable : DynamoDbClient<Room>, IRoomsRepository
    {
        public override string HashKey => "Id";

        public override string RangeKey => null;

        public RoomsTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Rooms")
        {
        }

        public async Task SaveRoom(Room room)
        {
            await PutItem(room);
        }

        public async Task<PageModel<Room>> ListRooms(PagingRequest pagingRequest)
        {
            return await Scan(pagingRequest, false);
        }

        public async Task<Room> GetRoom(Guid roomId)
        {
            return await GetItemById(roomId.ToString());
        }
    }
}
