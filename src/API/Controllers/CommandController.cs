using Contract;
using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Samples.EventSourcing.AggregateRoot;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly IEventSourcedRepository _repository;

        public CommandController(IEventSourcedRepository repository)
        {
            _repository = repository;
        }

        [HttpPost()]
        [Route("{id}")]
        //[Authorize]
        public async Task<IActionResult> Book([FromRoute] string id, [FromBody] BookPurchaseOrder cmd)
        {
            var aggregate = await _repository.Get<OrderBooking>(cmd.BookingId);

            var result = aggregate.Book(cmd.BookingReference, cmd.PurchaseOrder);

            if(result.Success)
            {
                await _repository.Flush();                
            }

            return Ok();

        }
    }
}