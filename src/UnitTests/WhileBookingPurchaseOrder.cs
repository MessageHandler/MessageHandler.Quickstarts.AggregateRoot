using System;
using System.Collections.Generic;
using MessageHandler.EventSourcing.Contracts;
using Contract;
using Xunit;
using FluentAssertions;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.UnitTests
{
    public class WhileBookingPurchaseOrder
    {
        [Fact]
        public void GivenNewBookingProcess_WhenBookingPurchaseOrder_ThenPurchaseOrderBooked()
        {
            var bookingId = Guid.NewGuid().ToString();
            var history = new List<SourcedEvent>();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            var purchaseOrder = new PurchaseOrderBuilder()
                                .WithOrderline()
                                .Build();            

            booking.Book(purchaseOrder.BookingReference,
                         purchaseOrder.PurchaseOrderId,
                         purchaseOrder.SellerReference,
                         purchaseOrder.BuyerReference,
                         purchaseOrder.OrderLines,
                         userId: null);

            var pendingEvents = booking.Commit();

            pendingEvents.Should().NotBeEmpty();
            pendingEvents.Should().AllBeOfType<PurchaseOrderBooked>();
        }

        public class PurchaseOrderBuilder
        {
            private PurchaseOrder _purchaseOrder;

            public PurchaseOrderBuilder()
            {
                _purchaseOrder = new PurchaseOrder
                {
                    BookingReference = Guid.NewGuid().ToString(),
                    PurchaseOrderId = Guid.NewGuid().ToString(),
                    SellerReference = Guid.NewGuid().ToString(),
                    BuyerReference = Guid.NewGuid().ToString(),
                    OrderLines = new List<OrderLine>() 
                };
            }
            
            public PurchaseOrderBuilder WithOrderline()
            {
                _purchaseOrder.OrderLines.Add(new OrderLine
                {
                    OrderLineId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    OrderedItem = new Item
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        CatalogId = Guid.NewGuid().ToString(),
                        CollectionId = Guid.NewGuid().ToString(),
                        Name = "Test Item",
                        Price = new Price { Currency = "EUR", Value = 1 }
                    }
                });

                return this;
            }

            public PurchaseOrder Build() {
                return _purchaseOrder;
            }

            
        }

        public class PurchaseOrder
        {
            public string BookingReference { get; set; }
            public string PurchaseOrderId { get; set; }
            public string SellerReference { get; set; }
            public string BuyerReference { get; set; }
            public List<OrderLine> OrderLines { get; set; }
        }
    }
}