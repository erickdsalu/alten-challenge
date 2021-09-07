using Application.Queries.Rooms;
using Application.QueryHandlers.Rooms;
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

namespace Application.Tests.QueryHandlers.Rooms
{
    public class ListRoomsQueryHandlersTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly ListRoomsQueryHandler _handler;

        public ListRoomsQueryHandlersTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<ListRoomsQueryHandler>();
        }

        [Fact]
        public async Task Handle_GivenNoIds_ShouldCallListRooms()
        {
            //Arrange
            var query = new ListRoomsQuery();

            var roomsRepositoryMock = _autoMocker.GetMock<IRoomsRepository>();
            roomsRepositoryMock.Setup(x => x.ListRooms(It.IsAny<PagingRequest>()))
                .ReturnsAsync(new PageModel<Room>
                {
                    Items = new List<Room> {
                        new Room(Guid.NewGuid(), 3)
                    }
                }); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            roomsRepositoryMock.Verify(x => x.ListRooms(It.IsAny<PagingRequest>()), Times.Once);
        }
    }
}
