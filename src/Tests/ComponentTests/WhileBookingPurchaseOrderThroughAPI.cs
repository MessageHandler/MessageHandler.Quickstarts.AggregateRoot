using Moq;
using Xunit;
using MessageHandler.EventSourcing.DomainModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using MessageHandler.Quickstart.Contract;
using MessageHandler.Quickstart.AggregateRoot.API;

namespace MessageHandler.Quickstart.AggregateRoot.ComponentTests
{
    public class WhileBookingPurchaseOrderThroughAPI
    {
        [Fact]
        public async Task GivenNewBookingProcess_WhenBookingPurchaseOrder_ThenPurchaseOrderBookedIsFlushed()
        {
            // given
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                                .Build();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            var mock = new Mock<IEventSourcedRepository<OrderBooking>>();
            mock.Setup(repository => repository.Get(bookingId))
                .ReturnsAsync(booking);

            // when
            var command = new BookPurchaseOrderCommandBuilder()
                          .WellknownBooking(bookingId)
                          .Build();
           
            var controller = new CommandController(mock.Object);

            var actionResult = await controller.Book(command.BookingId, command);

            // then
            mock.Verify(repository => repository.Flush(), Times.Once() );

            actionResult.Should().BeOfType<OkResult>();
        }
    }
}