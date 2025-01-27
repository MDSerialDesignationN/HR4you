using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR4You.Model.Base
{
    public class TableColumn<T>
    {
        public string? Header { get; set; }
        public Func<T, object>? CellTemplate { get; set; }

    }
}
