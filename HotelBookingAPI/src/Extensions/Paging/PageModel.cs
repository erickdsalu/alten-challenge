using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.Paging
{
    public class PageModel<T>
    {
        public int Count { get { return Items?.Count() ?? 0; } }
        public string LastIndex { get; set; }
        public IEnumerable<T> Items { get; set; }
        public bool HasNextPage { get { return !string.IsNullOrEmpty(LastIndex); } }
    }
}
