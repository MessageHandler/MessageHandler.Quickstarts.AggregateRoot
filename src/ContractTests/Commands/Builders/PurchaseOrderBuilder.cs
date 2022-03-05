using System;
using System.Collections.Generic;

namespace Contract
{
    public class PurchaseOrderBuilder
    {
        private PurchaseOrder _purchaseOrder;

        public PurchaseOrderBuilder()
        {
            _purchaseOrder = new PurchaseOrder
            {                
                PurchaseOrderId = Guid.NewGuid().ToString(),
                SellerReference = Guid.NewGuid().ToString(),
                BuyerReference = Guid.NewGuid().ToString(),
                OrderLines = new List<OrderLine>()
            };
        }       

        public PurchaseOrderBuilder WithOrderline(string name)
        {
            _purchaseOrder.OrderLines.Add(new OrderLine
            {
                OrderLineId = Guid.NewGuid().ToString(),
                Quantity = 1,
                OrderedItem = new Item
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CatalogId = Guid.NewGuid().ToString(),
                    CollectionId = Guid.NewGuid().ToString(),
                    Name = name,
                    Price = new Price { Currency = "EUR", Value = 1 }
                }
            });

            return this;
        }

        public PurchaseOrderBuilder WellknownOrder(string purchaseOrderId)
        {
            if(_wellknownPurchaseOrders.ContainsKey(purchaseOrderId))
            {
                _purchaseOrder = _wellknownPurchaseOrders[purchaseOrderId]();
            }

            return this;
        }

        public PurchaseOrder Build()
        {
            return _purchaseOrder;
        }



        private Dictionary<string, Func<PurchaseOrder>> _wellknownPurchaseOrders = new()
        {
            {
                "1aa6ab11-a111-4687-a6e0-cbcf403bc6a8",
                () =>
                {
                    return new PurchaseOrder() {
                        PurchaseOrderId = "1aa6ab11-a111-4687-a6e0-cbcf403bc6a8",
                        SellerReference = "cb05ff8a-0ee2-4f59-88d1-b8aa1c1e8025",
                        BuyerReference = "5c9276b1-a9fe-4303-b958-0202aaccd79d",
                        OrderLines = new List<OrderLine>()
                        {
                            new OrderLine
                            {
                                OrderLineId = "7ccb042b-2fec-407d-963a-28dbe40bee6b",
                                Quantity = 2,
                                OrderedItem = new Item
                                {
                                    ItemId = "250f9c8f-081c-4d74-abbf-4721fae038e2",
                                    CatalogId = "161bb828-34dc-42ce-9cec-d5611bbb1a5d",
                                    CollectionId = "c0dddc22-8394-48d3-b8fc-234cfab88280",
                                    Name = "Lasagna Bolognese",
                                    Price = new Price { Currency = "EUR", Value = 12 }
                                }
                            },
                            new OrderLine
                            {
                                OrderLineId = "3da450d0-3215-4248-b07c-a8c22855d337",
                                Quantity = 2,
                                OrderedItem = new Item
                                {
                                    ItemId = "11385982-dc9f-4213-984a-03f48f5d330d",
                                    CatalogId = "161bb828-34dc-42ce-9cec-d5611bbb1a5d",
                                    CollectionId = "c0dddc22-8394-48d3-b8fc-234cfab88280",
                                    Name = "Vol au vent",
                                    Price = new Price { Currency = "EUR", Value = 14 }
                                }
                            }
                        }
                    };
                }
            }
        };
    }
}