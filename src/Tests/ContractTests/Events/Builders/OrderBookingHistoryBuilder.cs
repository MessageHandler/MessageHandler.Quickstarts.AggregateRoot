using MessageHandler.EventSourcing.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MessageHandler.Quickstart.Contract
{
    public class OrderBookingHistoryBuilder
    {
        public IEnumerable<SourcedEvent> _events = new List<SourcedEvent>();

        public IEnumerable<SourcedEvent> Build()
        {
            return _events;
        }

        public OrderBookingHistoryBuilder WellknownBooking(string bookingId)
        {
            if (_wellknownBookings.ContainsKey(bookingId))
            {
                _events = _wellknownBookings[bookingId]();
            }
            return this;
        }

        private readonly Dictionary<string, Func<IEnumerable<SourcedEvent>>> _wellknownBookings = new()
        {
            {
                "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                () => new List<SourcedEvent>()
                    {
                        new PurchaseOrderBooked()
                        {
                            TenantId = "cb05ff8a-0ee2-4f59-88d1-b8aa1c1e8025",
                            Context = new Context
                            {
                                Id = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                                What = nameof(PurchaseOrderBooked),
                                When = DateTime.Parse("2022-03-06T15:07:53.3598499Z", null, DateTimeStyles.AdjustToUniversal),
                                Who = null,
                                Why = null,
                            },
                            BookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                            BookingReference = "0e397128-7544-4269-8eed-eaea1dc523b5",
                            PurchaseOrderId = "1aa6ab11-a111-4687-a6e0-cbcf403bc6a8",
                            SellerReference = "cb05ff8a-0ee2-4f59-88d1-b8aa1c1e8025",
                            BuyerReference = "5c9276b1-a9fe-4303-b958-0202aaccd79d",
                            OrderLines = new List<OrderLine>()
                            {
                                new OrderLine()
                                {
                                    OrderLineId = "7ccb042b-2fec-407d-963a-28dbe40bee6b",
                                    OrderedItem = new Item {
                                      ItemId = "250f9c8f-081c-4d74-abbf-4721fae038e2",
                                      CatalogId = "161bb828-34dc-42ce-9cec-d5611bbb1a5d",
                                      CollectionId = "c0dddc22-8394-48d3-b8fc-234cfab88280",
                                      Name = "Lasagna Bolognese",
                                      Price = new Price {
                                        Value = 12,
                                        Currency = "EUR"
                                      }
                                    },
                                    Quantity = 2
                                },
                                new OrderLine()
                                {
                                    OrderLineId = "3da450d0-3215-4248-b07c-a8c22855d337",
                                    OrderedItem = new Item {
                                      ItemId = "11385982-dc9f-4213-984a-03f48f5d330d",
                                      CatalogId = "161bb828-34dc-42ce-9cec-d5611bbb1a5d",
                                      CollectionId = "c0dddc22-8394-48d3-b8fc-234cfab88280",
                                      Name = "Vol au vent",
                                      Price = new Price {
                                        Value = 14,
                                        Currency = "EUR"
                                      }
                                    },
                                    Quantity = 2
                                }
                            },
                            EventId = "a1f75f3c-e2aa-4978-9273-a4c3e624d68c",
                            SourceId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                            Version = 1
                        }
                    }
            }
        };
    }
}