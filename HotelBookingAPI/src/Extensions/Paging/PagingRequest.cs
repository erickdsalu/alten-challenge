using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application")]
[assembly: InternalsVisibleTo("Persistence.DynamoDb")]

namespace Extensions.Paging
{
    public class PagingRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Limit { get; set; } = 100;
        public string LastIndex { get; set; }
        internal string[] ExcludentIds { get; set; }
    }
}
