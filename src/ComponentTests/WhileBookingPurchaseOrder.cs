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

            var purchaseOrder = new PurchaseOrderBuilder()
                                .WellknownOrder("1aa6ab11-a111-4687-a6e0-cbcf403bc6a8")                
                                .Build();

            //todo: builder
            var command = new BookPurchaseOrder()
            {
                BookingId = bookingId,
                BookingReference = "0e397128-7544-4269-8eed-eaea1dc523b5",
                PurchaseOrder = purchaseOrder,
            };

            var controller = new CommandController(mock.Object);

            var actionResult = controller.Book(command.BookingId, command);

            // then
            mock.Verify(repository => repository.Flush(), Times.Once() );
        }
    }
}