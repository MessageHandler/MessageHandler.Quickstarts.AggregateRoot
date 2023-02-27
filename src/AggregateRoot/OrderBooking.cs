using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Quickstart.AggregateRoot.Contract;

namespace MessageHandler.Quickstart.AggregateRoot
{
    public class OrderBooking : EventSourced,
        IApply<PurchaseOrderBooked>,
        IApply<BookingConfirmed>
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

        public BookingConfirmationResult Confirm(string userId = null)
        {
            if (this._salesOrderReference != null)
            {
                return new BookingConfirmationResult() { Success = false };
            }

            Emit(new BookingConfirmed()
            {
                TenantId = this._tenantId,
                Context = new Context
                {
                    Id = Id,
                    What = nameof(PurchaseOrderBooked),
                    When = DateTime.UtcNow,
                    Who = userId
                },
                BookingId = Id,
                SalesOrderReference = Guid.NewGuid().ToString()

            });

            return new BookingConfirmationResult() { Success = true };
        }

        public void Apply(PurchaseOrderBooked msg)
        {
            // only store state actually needed for maintaining integrity
            this._tenantId = msg.TenantId;
            this._bookingReference = msg.BookingReference;
        }

        public void Apply(BookingConfirmed msg)
        {
            // only store state actually needed for maintaining integrity
            this._salesOrderReference = msg.SalesOrderReference;
        }

        private string _tenantId { get; set; }
        private string _bookingReference { get; set; }
        private string _salesOrderReference { get; set; }

        public class BookingValidationResult
        {
            public bool Success { get; set; }
        }

        public class BookingConfirmationResult
        {
            public bool Success { get; set; }
        }
    }
}