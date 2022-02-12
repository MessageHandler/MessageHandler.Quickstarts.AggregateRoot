using MessageHandler.EventSourcing.Contracts;

namespace Contract
{
    public class BookingCancelled : SourcedEvent
    {
        public Context Context { get; set; }

        public string BookingId { get; set; }
    }
}