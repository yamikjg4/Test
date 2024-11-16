using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interface.ProductRepo
{
   public interface IProductRepo
    {
        Task<(IEnumerable<ListDto>, int cnt)> GetList(int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<CategoryDTO>> GetAllCategory();
        Task<int> InsertProductAsync(ProductModel product);
        Task<int> UpdateProductAsync(ProductModel product);
        Task<int> DeleteProductAsync(long productId);
        long GetCategoryNameById(string categoryId);
        Task<ProductModel> GetProductById(long Id);
    }
}
