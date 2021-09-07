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
    public class GetReservationQueryHandlersTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly GetReservationQueryHandler _handler;

        public GetReservationQueryHandlersTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<GetReservationQueryHandler>();
        }

        [Fact]
        public async Task Handle_GivenRoomId_ShouldCallGetReservation()
        {
            //Arrange
            var query = new GetReservationQuery
            {
                RoomId = Guid.NewGuid(),
                ReservationId = Guid.NewGuid()
            };

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(x => x.GetReservation(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Reservation(query.ReservationId, Guid.NewGuid(), query.RoomId,
                        Domain.Enums.ReservationStatus.Scheduled, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), true)); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            reservationsRepositoryMock.Verify(x => x.GetReservation(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
