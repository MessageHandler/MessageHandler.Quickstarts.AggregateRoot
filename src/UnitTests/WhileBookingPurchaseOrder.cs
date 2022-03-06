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
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0"; 
            var history = new List<SourcedEvent>();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            var purchaseOrder = new PurchaseOrderBuilder()
                                .WellknownOrder("1aa6ab11-a111-4687-a6e0-cbcf403bc6a8")
                                .Build();

            booking.Book("0e397128-7544-4269-8eed-eaea1dc523b5", purchaseOrder);

            var pendingEvents = booking.Commit();

            pendingEvents.Should().NotBeEmpty();
            pendingEvents.Should().AllBeOfType<PurchaseOrderBooked>();
        }

             
    }
}