using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PhoneNumber
    {
        public string DDI { get; private set; }
        public string DDD { get; private set; }
        public string Number { get; private set; }

        public PhoneNumber(string dDI, string dDD, string number)
        {
            DDI = dDI;
            DDD = dDD;
            Number = number;
        }
    }
}
