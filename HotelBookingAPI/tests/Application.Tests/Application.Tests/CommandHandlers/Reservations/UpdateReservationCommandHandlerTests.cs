using Application.CommandHandlers.Reservations;
using Application.Commands.Reservations;
using Domain.Enums;
using Domain.Models;
using Extensions.Exceptions;
using Extensions.Paging;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Persistence.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.CommandHandlers.Reservations
{
    public class UpdateReservationCommandHandlerTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly UpdateReservationCommandHandler _handler;

        public UpdateReservationCommandHandlerTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<UpdateReservationCommandHandler>();
            SetupHotelConfiguration();
        }

        [Fact]
        public async Task Handle_GivenValidReservation_ShouldSaveWithIsActiveFalse()
        {
            //Arrange
            var command = new UpdateReservationCommand
            {
                ReservationId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2)
            };

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();

            reservationsRepositoryMock.Setup(x => x.GetReservation(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Reservation(command.ReservationId, command.CustomerId, command.RoomId, ReservationStatus.Scheduled,
                command.StartDate.AddDays(1), command.EndDate.AddDays(5), true));

            reservationsRepositoryMock.Setup(x => x.ListReservationsByRoom(It.IsAny<PagingRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(new PageModel<Reservation>());

            //Act
            await _handler.Handle(command, CancellationToken.None);

            //Assert            
            reservationsRepositoryMock.Verify(x => x.SaveReservation(It.IsAny<Reservation>()), Times.Once);
        }

        private void SetupHotelConfiguration()
        {
            _autoMocker.GetMock<IConfigurationsRepository>().Setup(x => x.GetHotelConfiguration())
                .ReturnsAsync(new Configuration(30, 3));
        }
    }
}
