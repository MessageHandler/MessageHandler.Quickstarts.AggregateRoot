namespace MessageHandler.Quickstart.AggregateRoot.Contract
{
    public class PurchaseOrder
    {        
        public string PurchaseOrderId { get; set; }
        public string SellerReference { get; set; }
        public string BuyerReference { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}