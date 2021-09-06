using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class HotelConfiguration
    {
        public int MaxDaysInAdvance { get; private set; }

        public int MaximumReservationDays { get; private set; }

        public HotelConfiguration(int maxDaysInAdvance, int maximumReservationDays)
        {
            MaxDaysInAdvance = maxDaysInAdvance;
            MaximumReservationDays = maximumReservationDays;
        }
    }
}
