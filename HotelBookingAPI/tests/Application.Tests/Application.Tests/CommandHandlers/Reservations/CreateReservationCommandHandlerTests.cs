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
    public class CreateReservationCommandHandlerTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly CreateReservationCommandHandler _handler;

        public CreateReservationCommandHandlerTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<CreateReservationCommandHandler>();
            SetupHotelConfiguration();
        }

        [Fact]
        public async Task Handle_GivenValidReservation_ShouldSaveWithIsActiveFalse()
        {
            //Arrange
            var command = new CreateReservationCommand
            {
                CustomerId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2)
            };

            var roomsRepositoryMock = _autoMocker.GetMock<IRoomsRepository>();
            roomsRepositoryMock.Setup(x => x.GetRoom(command.RoomId))
                .ReturnsAsync(new Room(Guid.NewGuid(), 3));

            var reservationsRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();

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
