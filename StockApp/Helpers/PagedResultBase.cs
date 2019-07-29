﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }


    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int FirstRecordOnPage 
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRecordOnPage 
        {
            get { return Math.Min(CurrentPage * PageSize, TotalRecords); }
        }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
         
    }
}
