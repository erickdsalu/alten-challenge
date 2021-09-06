using Application.Models;
using Domain.Models;
using Extensions.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Mappers
{
    public static class CustomerMapper
    {

        public static PageModel<CustomerModel> AsApplicationModel(this PageModel<Customer> customer)
        {
            return new PageModel<CustomerModel>
            {
                LastIndex = customer.LastIndex,
                Items = customer.Items.Select(AsApplicationModel)
            };
        }

        public static CustomerModel AsApplicationModel(this Customer customer)
        {
            if (customer is null)
                return null;
            return new CustomerModel
            {
                Id = customer.Id,
                FistName = customer.FistName,
                LastName = customer.LastName,
                PhoneNumber = new PhoneNumberModel
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
    }
}
