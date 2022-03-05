using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Contract
{
    public class WhenCreatingOrderBookingHistoryUsingBuilder
    {
        [Fact]
        public async Task ShouldAdhereToContract()
        {
            var history = new OrderBookingHistoryBuilder()
                               .WellknownBooking("91d6950e-2ddf-4e98-a97c-fe5f434c13f0")
                               .Build();

            string csOutput = JsonSerializer.Serialize(history);

            await File.WriteAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/actual.cs.json", csOutput);

            // output provided by similar tests on the client side, using javascript
            var jsOutput = await File.ReadAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/verified.js.json");

            Assert.Equal(jsOutput, csOutput);
        }
    }
}