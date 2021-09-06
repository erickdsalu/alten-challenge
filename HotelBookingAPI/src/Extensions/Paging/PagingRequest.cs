using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Paging
{
    public class PagingRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Limit { get; set; } = 100;
        public string LastIndex { get; set; }
        public string[] ExcludentIds { get; set; }
    }
}
