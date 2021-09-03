using Amazon.DynamoDBv2;
using Domain.Models;
using Extensions.Paging;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class ReservationsTable : DynamoDbClient<Reservation>, IReservationsRepository
    {
        public override string HashKey => "RoomId";

        public override string RangeKey => "ReservationId";

        public ReservationsTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Reservations")
        {
        }

        public async Task SaveReservation(Reservation reservation)
        {
            await PutItem(reservation);
        }

        public async Task<PageModel<Reservation>> ListReservations(PagingRequest pagingRequest)
        {
            return await Scan(pagingRequest);
        }

        public async Task<PageModel<Reservation>> ListReservationsByRoom(PagingRequest pagingRequest, Guid roomId)
        {
            return await Query(pagingRequest, roomId.ToString());
        }

        public async Task<Reservation> GetReservation(Guid roomId, Guid reservationId)
        {
            return await GetItemById(roomId.ToString(), reservationId.ToString());
        }

        public async Task<PageModel<Reservation>> ListReservationsByCustomer(PagingRequest pagingRequest, Guid customerId)
        {
            return await Query(pagingRequest, null, "UserReservationIndex", "CustomerId", customerId.ToString());
        }
    }
}
