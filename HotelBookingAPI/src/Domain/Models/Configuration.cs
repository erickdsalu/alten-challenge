using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Configuration
    {
        public int MaxDaysInAdvance { get; private set; }

        public int MaximumReservationDays { get; private set; }

        public Configuration(int maxDaysInAdvance, int maximumReservationDays)
        {
            MaxDaysInAdvance = maxDaysInAdvance;
            MaximumReservationDays = maximumReservationDays;
        }
    }
}
