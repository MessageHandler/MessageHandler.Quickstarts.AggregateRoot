using MessageHandler.EventSourcing.Contracts;

namespace MessageHandler.Quickstart.Contract
{
    public class PurchaseOrderBooked : SourcedEvent
    {
        public Context Context { get; set; }

        public string BookingId { get; set; }

        public string BookingReference { get; set; }

        public string PurchaseOrderId { get; set; }

        public string SellerReference { get; set; }

        public string BuyerReference { get; set; }

        public IList<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}