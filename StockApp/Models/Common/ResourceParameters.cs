using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models.Common
{
    public abstract class ResourceParameters
    {
        const int maxPageSize = 100;

        private int _pageSize = 10;
        public int PageNo { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string SearchQuery { get; set; }
        public string Key { get; set; }
        public string OrderBy { get; set; }
    }
}

