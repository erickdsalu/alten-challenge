using Amazon.DynamoDBv2;
using Domain.Models;
using Extensions.Paging;
using Persistence.DynamoDb.Abstractions;
using Persistence.Interfaces;
using Persistence.Mappers;
using Persistence.Models;
using System;
using System.Threading.Tasks;

namespace Persistence.DynamoDb.Tables
{
    public class CustomersTable : DynamoDbClient<CustomerPersistence>, ICustomersRepository
    {
        public override string HashKey => "Id";

        public override string RangeKey => null;

        public CustomersTable(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB, "Customers")
        {
        }

        public async Task SaveCustomer(Customer customer)
        {
            await PutItem(customer.AsPersistence());
        }

        public async Task<PageModel<Customer>> ListCustomers(PagingRequest pagingRequest, bool? active = null)
        {
            return (await Scan(pagingRequest, false, active)).AsDomainModel();
        }

        public async Task<Customer> GetCustomer(Guid customerId)
        {
            return (await GetItemById(customerId.ToString())).AsDomainModel();
        }
    }
}
