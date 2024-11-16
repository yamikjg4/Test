using Domain.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ListDto
    {
        
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string SKU { get; set; }
            public string Category { get; set; } // This is the `CategoryName` from `Tbl_Category`
            public int BasePrice { get; set; }
            public int MRP { get; set; }
            public string Description { get; set; }
            public CurrencyEnum Currency { get; set; }
            public string ManufacturedDate { get; set; }
            public string ExpireDate { get; set; }

        public ProductModel Select(Func<object, ProductModel> value)
        {
            throw new NotImplementedException();
        }
    }
}
