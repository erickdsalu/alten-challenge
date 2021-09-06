using System;

namespace Web.ApiModels.Requests.Customers
{
    public class CreateCustomerRequest
    {
        public Guid Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public PhoneNumberRequest PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
