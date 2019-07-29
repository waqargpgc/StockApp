using System.Collections.Generic;

namespace StockApp.Models
{
    public class ResourceEndpoint
    {
        public string ResouceName { get; set; } // account/products    
        public string Uri { get; set; } // account/products    
        public IEnumerable<object> Params { get; set; } // name, type, optional,    
        public IEnumerable<string> SampleUri { get; set; } // account/products?pname=soaps&pagesize=20&searchQuery=cheap  
        public IEnumerable<object> SampleOutput { get; set; }     
        public string Description { get; set; }     
    }
}