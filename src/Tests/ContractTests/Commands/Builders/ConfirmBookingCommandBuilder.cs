using System;
using System.Collections.Generic;

namespace MessageHandler.Quickstart.Contract
{
    public class ConfirmBookingCommandBuilder
    {
        private ConfirmBooking _command;

        public ConfirmBookingCommandBuilder()
        {
            _command = new ConfirmBooking
            {
                BookingId = Guid.NewGuid().ToString()
            };
        }

        public ConfirmBookingCommandBuilder WellknownBooking(string bookingId)
        {
            if (_wellknownCommands.ContainsKey(bookingId))
            {
                _command = _wellknownCommands[bookingId]();
            }

            return this;
        }

        public ConfirmBooking Build()
        {
            return _command;
        }

        private readonly Dictionary<string, Func<ConfirmBooking>> _wellknownCommands = new()
        {
            {
                "91d6950e-2ddf-4e98-a97c-fe5f434c13f0",
                () =>
                {
                    return new ConfirmBooking()
                    {
                        BookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0"
                    };
                }
            }
        };
    }
}