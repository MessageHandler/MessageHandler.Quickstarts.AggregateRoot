using Xunit;
using FluentAssertions;
using MessageHandler.Quickstart.AggregateRoot.Contract;
using System.Linq;

namespace MessageHandler.Quickstart.AggregateRoot.UnitTests
{
    public class WhileConfirmingBooking
    {
        [Fact]
        public void GivenWellBookingProcess_WhenConfirmingBooking_ThenSalesOrderReferenceCreated()
        {
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                               .WellknownBooking(bookingId)
                               .Build();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            booking.Confirm();

            var pendingEvents = booking.Commit();
            pendingEvents.Should().NotBeEmpty();
            pendingEvents.Should().AllBeOfType<BookingConfirmed>();
            pendingEvents.First().As<BookingConfirmed>().SalesOrderReference.Should().NotBeNull();
        }
    }
}