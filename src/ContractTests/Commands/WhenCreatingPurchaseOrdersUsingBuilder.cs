using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Contract
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

            await File.WriteAllTextAsync(@"./.verification/1aa6ab11-a111-4687-a6e0-cbcf403bc6a8/actual.cs.json", csOutput);

            // output provided by similar tests on the client side, using javascript
            var jsOutput = await File.ReadAllTextAsync(@"./.verification/1aa6ab11-a111-4687-a6e0-cbcf403bc6a8/verified.js.json");

            Assert.Equal(jsOutput, csOutput);
        }
    }
}