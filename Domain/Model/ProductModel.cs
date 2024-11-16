using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Model
{
    public class ProductModel
    {
        [Key]
        public long? ProductId { get; set; }
        [Required(ErrorMessage ="Name Is Required")]
        [MaxLength(255,ErrorMessage ="Max Length 256")]
        public string Name { get; set; }
        [Required(ErrorMessage = "SKU Is Required")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Category Is Required")]
        public long Category { get; set; } // This is the `CategoryName` from `Tbl_Category`
        [Required(ErrorMessage = "BasePrice Is Required")]
        public int BasePrice { get; set; }
        [Required(ErrorMessage = "MRP Is Required")]
        public int MRP { get; set; }
        
        public string? Description { get; set; }
        [Required(ErrorMessage = "Currency Is Required")]
        public int Currency { get; set; }
        [JsonIgnore]
        public DateTime? ManufacturedDate { get; set; }
        [Required(ErrorMessage = "ExpireDate Is Required")]
        public DateTime ExpireDate { get; set; }
        
    }
}
