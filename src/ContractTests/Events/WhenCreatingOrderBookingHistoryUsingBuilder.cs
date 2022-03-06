using MessageHandler.Samples.EventSourcing.AggregateRoot.Contract;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.ContractTests
{
    public class WhenCreatingOrderBookingHistoryUsingBuilder
    {
        [Fact]
        public async Task ShouldAdhereToContract()
        {
            var history = new OrderBookingHistoryBuilder()
                               .WellknownBooking("91d6950e-2ddf-4e98-a97c-fe5f434c13f0")
                               .Build();

            string csOutput = JsonSerializer.Serialize(history.Cast<object>().ToArray());

            await File.WriteAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/actual.history.cs.json", csOutput);

            // output provided by similar tests on the client side, using javascript
            var jsOutput = await File.ReadAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/verified.history.js.json");

            Assert.Equal(jsOutput, csOutput);
        }
    }
}