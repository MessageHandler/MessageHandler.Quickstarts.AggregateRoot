using Moq;
using Xunit;
using MessageHandler.EventSourcing.DomainModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using MessageHandler.Samples.EventSourcing.AggregateRoot.Contract;
using MessageHandler.Samples.EventSourcing.AggregateRoot.API;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.ComponentTests
{
    public class WhileConfirmingBookingThroughAPI
    {
        [Fact]
        public async Task GivenWellknownBookingProcess_WhenConfirmingBooking_ThenBookingConfirmedIsFlushed()
        {
            // given
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                               .WellknownBooking(bookingId)
                               .Build();

            var booking = new OrderBooking(bookingId);
            booking.RestoreFrom(history);

            var mock = new Mock<IEventSourcedRepository>();
            mock.Setup(repository => repository.Get<OrderBooking>(bookingId))
                .ReturnsAsync(booking);

            // when
            var command = new ConfirmBookingCommandBuilder()
                          .WellknownBooking(bookingId)
                          .Build();

            var controller = new CommandController(mock.Object);

            var actionResult = await controller.Confirm(command.BookingId, command);

            // then
            mock.Verify(repository => repository.Flush(), Times.Once());

            actionResult.Should().BeOfType<OkResult>();
        }
    }
}