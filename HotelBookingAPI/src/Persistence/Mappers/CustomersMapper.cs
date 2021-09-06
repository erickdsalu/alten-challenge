using Domain.Models;
using Extensions.Paging;
using Persistence.Models;
using System.Linq;

namespace Persistence.Mappers
{
    public static class CustomersMapper
    {
        public static CustomerPersistence AsPersistence(this Customer customer)
        {
            if (customer is null)
                return null;
            return new CustomerPersistence
            {
                Id = customer.Id,
                FistName = customer.FistName,
                LastName = customer.LastName,
                PhoneNumber = new PhoneNumberPersistence
                {
                    DDI = customer.PhoneNumber.DDI,
                    DDD = customer.PhoneNumber.DDD,
                    Number = customer.PhoneNumber.Number
                },
                Email = customer.Email,
                CreatedAt = customer.CreatedAt,
                Active = customer.Active
            };
        }

        public static PageModel<Customer> AsDomainModel(this PageModel<CustomerPersistence> customer)
        {
            return new PageModel<Customer>
            {
                LastIndex = customer.LastIndex,
                Items = customer.Items.Select(AsDomainModel)
            };
        }

        public static Customer AsDomainModel(this CustomerPersistence customer)
        {
            if (customer is null)
                return null;
            return new Customer(id: customer.Id, fistName: customer.FistName, lastName: customer.LastName, phoneNumber: new PhoneNumber(dDI: customer.PhoneNumber.DDI, dDD: customer.PhoneNumber.DDD, number: customer.PhoneNumber.Number), email: customer.Email, createdAt: customer.CreatedAt, active: customer.Active);
        }
    }
}
