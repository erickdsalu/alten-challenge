using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Queries.Rooms;
using Extensions.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : BaseController
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get(PagingRequest pagingRequest, [FromQuery] bool onlyAvailableRooms)
        {
            var query = new ListRoomsQuery
            {
                StartDate = pagingRequest.StartDate?.Date,
                EndDate = pagingRequest.EndDate?.Date,
                LastIndex = pagingRequest.LastIndex,
                Limit = pagingRequest.Limit,
                OnlyAvailableRooms = onlyAvailableRooms
            };

            var result = await _mediator.Send(query);

            if (!result.Items.Any())
                return NotFound();

            return Ok(result.AsResponseModel());
        }
    }
}
