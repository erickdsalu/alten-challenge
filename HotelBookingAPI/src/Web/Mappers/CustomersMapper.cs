using Application.Models;
using Extensions.Paging;
using System.Linq;
using Web.ApiModels.Responses.Customers;

namespace Web.Mappers
{
    public static class CustomersMapper
    {
        public static PageModel<GetCustomerResponse> AsResponseModel(this PageModel<CustomerModel> customer)
        {
            return new PageModel<GetCustomerResponse>
            {
                LastIndex = customer.LastIndex,
                Items = customer.Items.Select(AsResponseModel)
            };
        }

        public static GetCustomerResponse AsResponseModel(this CustomerModel customerModel)
        {
            return new GetCustomerResponse
            {
                Id = customerModel.Id,
                FistName = customerModel.FistName,
                LastName = customerModel.LastName,
                PhoneNumber = new PhoneNumberResponse
                {
                    DDI = customerModel.PhoneNumber.DDI,
                    DDD = customerModel.PhoneNumber.DDD,
                    Number = customerModel.PhoneNumber.Number
                },
                Email = customerModel.Email,
                CreatedAt = customerModel.CreatedAt,
                Active = customerModel.Active
            };
        }
    }
}
