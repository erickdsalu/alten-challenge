using Domain.Models;
using Extensions.Paging;
using System;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IReservationsRepository
    {
        public Task SaveReservation(Reservation reservation);
        public Task<PageModel<Reservation>> ListReservations(PagingRequest pagingRequest);
        public Task<PageModel<Reservation>> ListReservationsByRoom(PagingRequest pagingRequest, Guid roomId);
        public Task<PageModel<Reservation>> ListReservationsByCustomer(PagingRequest pagingRequest, Guid customerId);
        public Task<Reservation> GetReservation(Guid roomId, Guid reservationId);
    }
}
