using Application.CommandHandlers.Reservations;
using Application.Commands.Reservations;
using Domain.Enums;
using Domain.Models;
using Extensions.Exceptions;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.CommandHandlers.Reservations
{
    public class CancelReservationCommandHandlerTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly CancelReservationCommandHandler _handler;

        public CancelReservationCommandHandlerTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<CancelReservationCommandHandler>();
        }

        [Fact]
        public async Task Handle_GivenValidReservation_ShouldSaveWithIsActiveFalse()
        {
            //Arrange
            var command = new CancelReservationCommand
            {
                ReservationId = Guid.NewGuid(),
                RoomId = Guid.NewGuid()
            };

            var reservationRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationRepositoryMock.Setup(x => x.GetReservation(command.RoomId, command.ReservationId))
                .ReturnsAsync(new Domain.Models.Reservation(command.ReservationId, Guid.NewGuid(), command.RoomId,
                ReservationStatus.Scheduled, DateTime.UtcNow.Date.AddDays(1), DateTime.UtcNow.Date.AddDays(2), true));

            //Act
            await _handler.Handle(command, CancellationToken.None);

            //Assert
            reservationRepositoryMock.Verify(x => x.SaveReservation(It.Is<Reservation>(y => y.IsActive == false)), Times.Once);
        }

        [Fact]
        public void Handle_GivenNewRoomReservationUpdate_ShouldNotSave()
        {
            //Arrange
            var command = new CancelReservationCommand
            {
                ReservationId = Guid.NewGuid(),
                RoomId = Guid.NewGuid()
            };

            var reservationRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();
            reservationRepositoryMock.Setup(x => x.GetReservation(command.RoomId, command.ReservationId))
                .ReturnsAsync(new Reservation(command.ReservationId, Guid.NewGuid(), Guid.NewGuid(),
                ReservationStatus.Scheduled, DateTime.UtcNow.Date.AddDays(1), DateTime.UtcNow.Date.AddDays(2), true));

            //Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().Throw<CustomNotificationException>().WithMessage("Can't change rooms for a reservation, please cancel this reservation and create another one");

            //Assert
            reservationRepositoryMock.Verify(x => x.SaveReservation(It.Is<Reservation>(y => y.IsActive == false)), Times.Never);
        }

        [Fact]
        public void Handle_GivenNotFoundReservation_ShouldNotSave()
        {
            //Arrange
            var command = new CancelReservationCommand
            {
                ReservationId = Guid.NewGuid(),
                RoomId = Guid.NewGuid()
            };

            var reservationRepositoryMock = _autoMocker.GetMock<IReservationsRepository>();

            //Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().Throw<CustomNotificationException>().WithMessage($"Reservation {command.ReservationId} wasn't found");

            //Assert
            reservationRepositoryMock.Verify(x => x.SaveReservation(It.Is<Reservation>(y => y.IsActive == false)), Times.Never);
        }
    }
}
