using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Quickstart.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MessageHandler.Quickstart.AggregateRoot.API
{
    [ApiController]
    [Route("api")]
    public class CommandController : ControllerBase
    {
        private readonly IEventSourcedRepository<OrderBooking> _repository;

        public CommandController(IEventSourcedRepository<OrderBooking> repository)
        {
            _repository = repository;
        }

        [HttpPost()]
        [Route("{id}/book")]
        //[Authorize]
        public async Task<IActionResult> Book([FromRoute] string id, [FromBody] BookPurchaseOrder cmd)
        {
            var aggregate = await _repository.Get(id);

            var result = aggregate.Book(cmd.BookingReference, cmd.PurchaseOrder);

            if(result.Success)
            {
                await _repository.Flush();                
            }

            return Ok();

        }

        [HttpPost()]
        [Route("{id}/confirm")]
        //[Authorize]
        public async Task<IActionResult> Confirm([FromRoute] string id, [FromBody] ConfirmBooking cmd)
        {
            var aggregate = await _repository.Get(id);

            var result = aggregate.Confirm();

            if (result.Success)
            {
                await _repository.Flush();
            }

            return Ok();

        }
    }
}