using Application.Queries.Customers;
using Application.QueryHandlers.Customers;
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

namespace Application.Tests.QueryHandlers.Customers
{
    public class ListCustomersQueryHandlersTests
    {

        private readonly AutoMocker _autoMocker;
        private readonly ListCustomersQueryHandler _handler;

        public ListCustomersQueryHandlersTests()
        {
            _autoMocker = new AutoMocker();
            _handler = _autoMocker.CreateInstance<ListCustomersQueryHandler>();
        }

        [Fact]
        public async Task Handle_GivenValidCustomerId_ShouldCallListCustomers()
        {
            //Arrange
            var query = new ListCustomersQuery();

            var customersRepositoryMock = _autoMocker.GetMock<ICustomersRepository>();
            customersRepositoryMock.Setup(x => x.ListCustomers(It.IsAny<PagingRequest>(), null))
                .ReturnsAsync(new PageModel<Customer>
                {
                    Items = new List<Customer> {
                        new Customer(Guid.NewGuid(), "Name", "Surname", new PhoneNumber("55","41","99999999"), "email@test.com",DateTime.UtcNow, true)
                    }
                }); ;

            //Act
            await _handler.Handle(query, CancellationToken.None);

            //Assert            
            customersRepositoryMock.Verify(x => x.ListCustomers(It.IsAny<PagingRequest>(), null), Times.Once);
        }
    }
}
