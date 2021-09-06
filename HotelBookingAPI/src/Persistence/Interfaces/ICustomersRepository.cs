using Domain.Models;
using Extensions.Paging;
using System;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface ICustomersRepository
    {
        public Task SaveCustomer(Customer customer);
        public Task<PageModel<Customer>> ListCustomers(PagingRequest pagingRequest, bool? active = null);
        public Task<Customer> GetCustomer(Guid customerId);
    }
}
