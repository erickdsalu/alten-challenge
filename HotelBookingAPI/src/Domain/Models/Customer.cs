using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FistName { get; private set; }
        public string LastName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool Active { get; private set; }

        public Customer(Guid id, string fistName, string lastName, PhoneNumber phoneNumber, string email, DateTime createdAt, bool active)
        {
            Id = id;
            FistName = fistName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            CreatedAt = createdAt;
            Active = active;
        }
    }
}
