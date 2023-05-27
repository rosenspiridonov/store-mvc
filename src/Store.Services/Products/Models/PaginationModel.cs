using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public List<int> PageSizes { get; set; } = new List<int> { 12, 24, 36, 48, 60 };
    }
}
