using MessageHandler.Quickstart.AggregateRoot.Contract;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MessageHandler.Quickstart.AggregateRoot.ContractTests
{
    public class WhenCreatingConfirmBookingCommandsUsingBuilder
    {
        [Fact]
        public async Task ShouldAdhereToContract()
        {
            var command = new ConfirmBookingCommandBuilder()
                                        .WellknownBooking("91d6950e-2ddf-4e98-a97c-fe5f434c13f0")
                                        .Build();

            string csOutput = JsonSerializer.Serialize(command);

            await File.WriteAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/actual.confirmbooking.command.cs.json", csOutput);

            // output provided by similar tests on the client side, using javascript
            var jsOutput = await File.ReadAllTextAsync(@"./.verification/91d6950e-2ddf-4e98-a97c-fe5f434c13f0/verified.confirmbooking.command.js.json");

            Assert.Equal(jsOutput, csOutput);
        }
    }
}