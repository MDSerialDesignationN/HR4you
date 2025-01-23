using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR4You.Model.Base
{
    public class FilterOption
    {
            public string? Icon { get; set; }
            public string? Name { get; set; }
            public string? InputType { get; set; }
            public List<string>? Options { get; set; }  
    }
}
