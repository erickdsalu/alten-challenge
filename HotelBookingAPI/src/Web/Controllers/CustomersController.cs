using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Queries.Customers;
using Extensions.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get(PagingRequest pagingRequest, [FromQuery] bool onlyActiveCustomers)
        {
            var query = new ListCustomersQuery
            {
                LastIndex = pagingRequest.LastIndex,
                Limit = pagingRequest.Limit,
                Active = onlyActiveCustomers
            };

            var result = await _mediator.Send(query);

            if (!result.Items.Any())
                return NotFound();

            return Ok(result.AsResponseModel());
        }
    }
}
