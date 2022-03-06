using Moq;
using Xunit;
using MessageHandler.EventSourcing.DomainModel;
using MessageHandler.Samples.EventSourcing.AggregateRoot;
using System;
using System.Threading.Tasks;
using Contract;
using API;

namespace ComponentTests
{
    public class WhileBookingPurchaseOrder
    {
        [Fact]
        public void GivenNewBookingProcess_WhenBookingPurchaseOrder_ThenPurchaseOrderBookedIsFlushed()
        {
            // given
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                                .Build();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            var mock = new Mock<IEventSourcedRepository>();
            mock.Setup(repository => repository.Get<OrderBooking>(bookingId))
                .Returns(Task.FromResult(booking));

            // when
            var command = new BookPurchaseOrderCommandBuilder()
                          .WellknownBooking(bookingId)
                          .Build();
           
            var controller = new CommandController(mock.Object);

            var actionResult = controller.Book(command.BookingId, command);

            // then
            mock.Verify(repository => repository.Flush(), Times.Once() );
        }
    }
}