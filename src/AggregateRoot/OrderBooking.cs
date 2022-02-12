using Contract;
using MessageHandler.EventSourcing.DomainModel;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot
{
    public class OrderBooking : EventSourced,
        IApply<PurchaseOrderBooked>
    {
        public OrderBooking(string id) : base(id)
        {
        }

        public BookingValidationResult Book(string bookingReference, 
                                            string purchaseOrderId, 
                                            string sellerReference, 
                                            string buyerReference, 
                                            IList<OrderLine> orderLines, 
                                            string userId){

            if (this._bookingReference != null) {
                return new BookingValidationResult() { Success = false };
            }

            Emit(new PurchaseOrderBooked()
            {
                TenantId = sellerReference,
                Context = new Context
                {
                    Id = Id,
                    What = nameof(PurchaseOrderBooked),
                    When = DateTime.UtcNow,
                    Who = userId
                },
                BookingId = Id,
                BookingReference = bookingReference,
                PurchaseOrderId = purchaseOrderId,
                SellerReference = sellerReference,
                BuyerReference = buyerReference,
                OrderLines = orderLines

            });

            return new BookingValidationResult() { Success = true };
        }

        public void Apply(PurchaseOrderBooked msg)
        {
            this._bookingReference = msg.BookingReference;
        }
             
        private string _bookingReference { get; set; }
        
        public class BookingValidationResult
        {
            public bool Success { get; set; }
        }
    }
}