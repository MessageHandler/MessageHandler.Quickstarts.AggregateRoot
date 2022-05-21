using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Samples.EventSourcing.AggregateRoot.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.API
{
    [ApiController]
    [Route("api")]
    public class CommandController : ControllerBase
    {
        private readonly IEventSourcedRepository _repository;

        public CommandController(IEventSourcedRepository repository)
        {
            _repository = repository;
        }

        [HttpPost()]
        [Route("{id}/book")]
        //[Authorize]
        public async Task<IActionResult> Book([FromRoute] string id, [FromBody] BookPurchaseOrder cmd)
        {
            var aggregate = await _repository.Get<OrderBooking>(id);

            var result = aggregate.Book(cmd.BookingReference, cmd.PurchaseOrder);

            if(result.Success)
            {
                await _repository.Flush();                
            }

            return Ok();

        }
    }
}