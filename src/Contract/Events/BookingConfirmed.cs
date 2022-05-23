using MessageHandler.EventSourcing.Contracts;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.Contract
{
    public class BookingConfirmed : SourcedEvent
    {
        public Context Context { get; set; }

        public string BookingId { get; set; }

        public string SalesOrderReference { get; set; }
    }
}