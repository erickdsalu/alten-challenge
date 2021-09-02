using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
