using System;

namespace Persistence.Models
{
    public class CustomerPersistence
    {
        public Guid Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public PhoneNumberPersistence PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
