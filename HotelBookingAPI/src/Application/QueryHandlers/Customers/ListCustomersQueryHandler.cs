using Application.Mappers;
using Application.Models;
using Application.Queries.Customers;
using Extensions.Exceptions;
using Extensions.Paging;
using MediatR;
using Persistence.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers.Customers
{
    public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, PageModel<CustomerModel>>
    {
        private readonly ICustomersRepository _customersRepository;

        public ListCustomersQueryHandler(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<PageModel<CustomerModel>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customersRepository.ListCustomers(request, request.Active);

            if (customers.Count == 0)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                        $"There is no customers that match your search criteria");

            return customers.AsApplicationModel();
        }
    }
}
