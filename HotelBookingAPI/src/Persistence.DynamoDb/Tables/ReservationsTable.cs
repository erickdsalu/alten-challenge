using Amazon.DynamoDBv2;
using Domain.Models;
using Extensions.Paging;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using Persistence.Mappers;
using Persistence.Models;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class ReservationsTable : DynamoDbClient<ReservationPersistence>, IReservationsRepository
    {
        public override string HashKey => "RoomId";

        public override string RangeKey => "ReservationId";

        public ReservationsTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Reservations")
        {
        }

        public async Task SaveReservation(Reservation reservation)
        {
            await PutItem(reservation.AsPersistence());
        }

        public async Task<PageModel<Reservation>> ListReservations(PagingRequest pagingRequest)
        {
            return (await Scan(pagingRequest, true, active: true)).AsDomainModel();
        }

        public async Task<PageModel<Reservation>> ListReservationsByRoom(PagingRequest pagingRequest, Guid roomId)
        {
            return (await Query(pagingRequest, true, roomId.ToString(), active: true)).AsDomainModel();
        }

        public async Task<Reservation> GetReservation(Guid roomId, Guid reservationId)
        {
            return (await GetItemById(roomId.ToString(), reservationId.ToString())).AsDomainModel();
        }

        public async Task<PageModel<Reservation>> ListReservationsByCustomer(PagingRequest pagingRequest, Guid customerId)
        {
            return (await Query(pagingRequest, true, null, "UserReservationIndex", "CustomerId", customerId.ToString(), active: true)).AsDomainModel();
        }
    }
}
