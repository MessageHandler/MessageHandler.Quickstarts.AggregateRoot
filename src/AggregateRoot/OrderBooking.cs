using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Samples.EventSourcing.AggregateRoot.Contract;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot
{
    public class OrderBooking : EventSourced,
        IApply<PurchaseOrderBooked>
    {
        public OrderBooking(string id) : base(id)
        {
        }

        public BookingValidationResult Book(string bookingReference, PurchaseOrder purchaseOrder, string userId = null){

            // maintain integrity
            if (this._bookingReference != null) {
                return new BookingValidationResult() { Success = false };
            }

            // record decision
            Emit(new PurchaseOrderBooked()
            {
                TenantId = purchaseOrder.SellerReference,
                Context = new Context
                {
                    Id = Id,
                    What = nameof(PurchaseOrderBooked),
                    When = DateTime.UtcNow,
                    Who = userId
                },
                BookingId = Id,
                BookingReference = bookingReference,
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                SellerReference = purchaseOrder.SellerReference,
                BuyerReference = purchaseOrder.BuyerReference,
                OrderLines = purchaseOrder.OrderLines

            });

            return new BookingValidationResult() { Success = true };
        }

        public void Apply(PurchaseOrderBooked msg)
        {
            // only store state actually needed for maintaining integrity
            this._bookingReference = msg.BookingReference;
        }
             
        private string _bookingReference { get; set; }
        
        public class BookingValidationResult
        {
            public bool Success { get; set; }
        }
    }
}