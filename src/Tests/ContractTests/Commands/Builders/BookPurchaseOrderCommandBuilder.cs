using System;
using System.Collections.Generic;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.Contract
{
    public class BookPurchaseOrderCommandBuilder
    {
        private BookPurchaseOrder _command;

        public BookPurchaseOrderCommandBuilder()
        {
            var purchaseOrder = new PurchaseOrderBuilder()                               
                               .Build();

            _command = new BookPurchaseOrder
            {
                BookingId = Guid.NewGuid().ToString(),
                BookingReference = Guid.NewGuid().ToString(),
                PurchaseOrder = purchaseOrder
            };
        }

        public BookPurchaseOrderCommandBuilder WellknownBooking(string bookingId)
        {
            if (_wellknownCommands.ContainsKey(bookingId))
            {
                _command = _wellknownCommands[bookingId]();
            }

            return this;
        }

        public BookPurchaseOrder Build()
        {
            return _command;
        }

        private readonly Dictionary<string, Func<BookPurchaseOrder>> _wellknownCommands = new()
        {
            {
                "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                () =>
                {
                    var purchaseOrder = new PurchaseOrderBuilder()
                               .WellknownOrder("1aa6ab11-a111-4687-a6e0-cbcf403bc6a8")
                               .Build();

                    return new BookPurchaseOrder()
                    {
                        BookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                        BookingReference = "0e397128-7544-4269-8eed-eaea1dc523b5",
                        PurchaseOrder = purchaseOrder,
                    };
                }
            }
        };
    }
}