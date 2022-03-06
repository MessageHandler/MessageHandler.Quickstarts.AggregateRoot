namespace MessageHandler.Samples.EventSourcing.AggregateRoot.Contract
{
    public class BookPurchaseOrder
    {
        public string BookingReference { get; set; }

        public string BookingId { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}