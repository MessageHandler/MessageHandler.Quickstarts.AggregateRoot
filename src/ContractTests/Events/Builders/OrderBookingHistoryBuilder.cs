using MessageHandler.EventSourcing.Contracts;
using System;
using System.Collections.Generic;

namespace Contract
{
    public class OrderBookingHistoryBuilder
    {
        public IEnumerable<SourcedEvent> _events = new List<SourcedEvent>();

        public IEnumerable<SourcedEvent> Build()
        {
            return _events;
        }

        public OrderBookingHistoryBuilder WellknownBooking(string v)
        {
            //"91d6950e-2ddf-4e98-a97c-fe5f434c13f0"

            return this;
        }
    }
}