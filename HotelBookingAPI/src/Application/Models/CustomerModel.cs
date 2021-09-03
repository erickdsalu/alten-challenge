using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public PhoneNumberModel PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
