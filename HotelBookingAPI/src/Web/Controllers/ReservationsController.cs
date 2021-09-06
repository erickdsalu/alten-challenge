using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Reservations;
using Application.Queries.Reservations;
using Extensions.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.ApiModels.Requests.Reservations;
using Web.Mappers;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController : BaseController
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get(PagingRequest pagingRequest, [FromQuery] Guid? roomId, [FromQuery] Guid? customerId)
        {
            var query = new ListReservationsQuery
            {
                StartDate = pagingRequest.StartDate?.Date,
                EndDate = pagingRequest.EndDate?.Date,
                LastIndex = pagingRequest.LastIndex,
                Limit = pagingRequest.Limit,
                CustomerId = customerId
            };

            query.RoomId = roomId;

            var result = await _mediator.Send(query);

            if (!result.Items.Any())
                return NotFound();

            return Ok(result.AsResponseModel());
        }

        // GET api/values/5
        [HttpGet("{reservationId}")]
        public async Task<ActionResult> Get(Guid reservationId, [FromQuery] Guid roomId)
        {
            var query = new GetReservationQuery
            {
                ReservationId = reservationId,
                RoomId = roomId
            };

            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result.AsResponseModel());
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateReservationRequest request)
        {
            var command = new CreateReservationCommand
            {
                CustomerId = request.CustomerId,
                RoomId = request.RoomId,
                StartDate = request.StartDate.Date,
                EndDate = request.EndDate.Date
            };

            await _mediator.Send(command);

            return NoContent();
        }

        // POST api/values
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateReservationRequest request)
        {
            var command = new UpdateReservationCommand
            {
                ReservationId = request.ReservationId,
                CustomerId = request.CustomerId,
                RoomId = request.RoomId,
                StartDate = request.StartDate.Date,
                EndDate = request.EndDate.Date,
            };

            await _mediator.Send(command);

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{reservationId}")]
        public async Task<ActionResult> Delete(Guid reservationId, [FromQuery] Guid roomId)
        {
            var command = new CancelReservationCommand
            {
                ReservationId = reservationId,
                RoomId = roomId
            };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
