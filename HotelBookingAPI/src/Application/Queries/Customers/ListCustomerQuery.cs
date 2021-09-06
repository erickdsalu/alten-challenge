using Application.Models;
using Extensions.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Customers
{
    public class ListCustomersQuery : PagingRequest, IRequest<PageModel<CustomerModel>>
    {
        public bool? Active { get; set; }
    }
}
