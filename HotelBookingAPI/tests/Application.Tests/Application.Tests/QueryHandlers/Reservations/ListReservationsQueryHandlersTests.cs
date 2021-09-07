using Application.Queries.Reservations;
using Application.QueryHandlers.Reservations;
using Domain.Models;
using Extensions.Paging;
using Moq;
using Moq.AutoMock;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.QueryHandlers.Reservations
{
    public class ListReservationsQueryHandlersTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly ListReservationsQueryHandler _handler;

        public ListReservationsQueryHandlersTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<ListReservationsQueryHandler>();
        }

        [Fact]
        public async Task Handle_GivenRoomId_ShouldCallListReservationsByRoom()
        {
            //Arrange
            var query = new ListReservationsQuery
            {
                RoomId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid()
            };

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(x => x.ListReservationsByRoom(It.IsAny<PagingRequest>(), query.RoomId.Value))
                .ReturnsAsync(new PageModel<Reservation>
                {
                    Items = new List<Reservation> {
                        new Reservation(Guid.NewGuid(), query.CustomerId.Value, query.RoomId.Value,
                        Domain.Enums.ReservationStatus.Scheduled, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), true)
                    }
                }); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            reservationsRepositoryMock.Verify(x => x.ListReservationsByRoom(It.IsAny<PagingRequest>(), query.RoomId.Value), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenValidCustomerId_ShouldCallListReservationsByCustomer()
        {
            //Arrange
            var query = new ListReservationsQuery
            {
                CustomerId = Guid.NewGuid()
            };

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(x => x.ListReservationsByCustomer(It.IsAny<PagingRequest>(), query.CustomerId.Value))
                .ReturnsAsync(new PageModel<Reservation>
                {
                    Items = new List<Reservation> {
                        new Reservation(Guid.NewGuid(), query.CustomerId.Value, Guid.NewGuid(),
                        Domain.Enums.ReservationStatus.Scheduled, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), true)
                    }
                }); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            reservationsRepositoryMock.Verify(x => x.ListReservationsByCustomer(It.IsAny<PagingRequest>(), query.CustomerId.Value), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenNoIds_ShouldCallListReservations()
        {
            //Arrange
            var query = new ListReservationsQuery();

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(x => x.ListReservations(It.IsAny<PagingRequest>()))
                .ReturnsAsync(new PageModel<Reservation>
                {
                    Items = new List<Reservation> {
                        new Reservation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                        Domain.Enums.ReservationStatus.Scheduled, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), true)
                    }
                }); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            reservationsRepositoryMock.Verify(x => x.ListReservations(It.IsAny<PagingRequest>()), Times.Once);
        }
    }
}
