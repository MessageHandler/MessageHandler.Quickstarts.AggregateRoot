using MessageHandler.Quickstart.Contract;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MessageHandler.Quickstart.AggregateRoot.ContractTests
{
    public class WhenCreatingPurchaseOrdersUsingBuilder
    {
        [Fact]
        public async Task ShouldAdhereToContract()
        {
            var purchaseOrder = new PurchaseOrderBuilder()
                                        .WellknownOrder("1aa6ab11-a111-4687-a6e0-cbcf403bc6a8")
                                        .Build();

            string csOutput = JsonSerializer.Serialize(purchaseOrder);

            await File.WriteAllTextAsync(@"./.verification/1aa6ab11-a111-4687-a6e0-cbcf403bc6a8/actual.state.cs.json", csOutput);

            // output provided by similar tests on the client side, using javascript
            var jsOutput = await File.ReadAllTextAsync(@"./.verification/1aa6ab11-a111-4687-a6e0-cbcf403bc6a8/verified.state.js.json");

            Assert.Equal(jsOutput, csOutput);
        }
    }
}