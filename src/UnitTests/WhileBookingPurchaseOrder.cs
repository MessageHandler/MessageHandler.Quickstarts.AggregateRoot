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
                                .WithOrderline("Test Item")
                                .Build();

             booking.Book(Guid.NewGuid().ToString(), purchaseOrder);

            var pendingEvents = booking.Commit();

            pendingEvents.Should().NotBeEmpty();
            pendingEvents.Should().AllBeOfType<PurchaseOrderBooked>();
        }

             
    }
}