using System.Data;
using System.Data.Common;
using System.Web.Mvc;
using BAL.Interface.ProductRepo;
using Dapper;
using Domain.DTO;
using Domain.Enum;
using Domain.Model;
namespace DAL.Repositry
{
    public class ProductRepo: IProductRepo
    {
       private readonly DapperContext.DapperContext _context;
        public ProductRepo(DapperContext.DapperContext context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<ListDto>,int cnt)> GetList(int pageNumber = 1, int pageSize = 10)
        {
            List<ListDto> lst = new List<ListDto>();
            int cntdata = 0;
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                using (var lsd = await connection.QueryMultipleAsync("sp_getdata", new { Pageno = pageNumber, PageSize = pageSize}, commandType: CommandType.StoredProcedure))
             {
                    var products = await lsd.ReadAsync<ListDto>();
                    int totalCount = await lsd.ReadSingleAsync<int>();
                    if (products != null && products.Count() > 0)
                    {
                        cntdata = totalCount;
                        lst = products.Select(dto =>
                        {
                            dto.Currency = (CurrencyEnum)(int)dto.Currency; // Cast to enum if needed
                            dto.Description = String.IsNullOrEmpty(dto.Description) ? "" : dto.Description;
                            return dto;
                        }).ToList();
                    }
                }
              
            }
            return (lst, cntdata);
        }
        public async Task<ProductModel> GetProductById(long Id)
        {
            ProductModel model = new ProductModel();
            string query = @"SELECT [ProductId]
      ,[Name]
      ,[SKU]
      ,[Category] As Category
      ,[BasePrice]
      ,[MRP]
      ,[Description]
      ,[Currency],
       [ManufacturedDate] AS ManufacturedDate, 
	   [ExpireDate] AS ExpireDate 
	   FROM [db_test].[dbo].[Tbl_Product] t1
Where [ProductId]=@id";
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                var res = await connection.QueryFirstOrDefaultAsync<ProductModel>(query, new { id = Id });
                model = res??new();          
            }
            return model;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategory()
        {
            List<CategoryDTO> lstdata = new List<CategoryDTO>();
            using (var connection = _context.GetConnection())
            {
                var Query = "SELECT  [CategoryID] ,[CategoryName] FROM [db_test].[dbo].[Tbl_Category] WHERE IsActive=1";
                var res = await connection.QueryAsync<CategoryDTO>(Query);
                if(res!=null && res.Count() > 0)
                {
                    lstdata = res.ToList();
                }
            }
            return lstdata;
        }
        public long GetCategoryNameById(string categoryId)
        {
            using (var connection = _context.GetConnection())
            {
                var query = "SELECT [CategoryID] FROM Tbl_Category WHERE [CategoryName] = @CategoryId";
                var categoryName = connection.QueryFirstOrDefault<long>(query, new { CategoryId = categoryId });
                return categoryName;
            }
            }
        public async Task<int> InsertProductAsync(ProductModel product)
        {
            using (var connection = _context.GetConnection()) // Assuming _context.GetConnection() returns an open SQL connection
            {
                connection.Open();

                // Define the SQL insert query
                var query = @"
            INSERT INTO Tbl_Product
            ([Name], [SKU], [Category], [BasePrice], [MRP], [Description], [Currency], [ManufacturedDate], [ExpireDate])
            VALUES
            (@Name, @SKU, @Category, @BasePrice, @MRP, @Description, @Currency, @ManufacturedDate, @ExpireDate)";

                // Execute the query using Dapper
                var result = await connection.ExecuteAsync(query, new
                {
                    Name = product.Name,
                    SKU = product.SKU,
                    Category = product.Category,
                    BasePrice = product.BasePrice,
                    MRP = product.MRP,
                    Description = product.Description,
                    Currency = product.Currency, // If Currency is stored as int in DB
                    ManufacturedDate = DateTime.Now.Date,
                    ExpireDate = product.ExpireDate.Date
                });

                return result; // Returns the number of affected rows
            }
        }
        public async Task<int> UpdateProductAsync(ProductModel product)
        {
            using (var connection = _context.GetConnection()) // Assuming _context.GetConnection() returns an open SQL connection
            {
                connection.Open();

                // Define the SQL update query
                var query = @"
            UPDATE Tbl_Product
            SET 
                [Name] = @Name,
                [SKU] = @SKU,
                [Category] = @Category,
                [BasePrice] = @BasePrice,
                [MRP] = @MRP,
                [Description] = @Description,
                [Currency] = @Currency,
               
                [ExpireDate] = @ExpireDate
            WHERE [ProductId] = @ProductId";

                // Execute the update query using Dapper
                var result = await connection.ExecuteAsync(query, new
                {
                    ProductId = product.ProductId,  // ProductId is used to identify the record
                    Name = product.Name,
                    SKU = product.SKU,
                    Category = product.Category,
                    BasePrice = product.BasePrice,
                    MRP = product.MRP,
                    Description = product.Description,
                    Currency = product.Currency,  // Assuming Currency is stored as an integer in DB
                    //ManufacturedDate = product.ManufacturedDate.Date,
                    ExpireDate = product.ExpireDate.Date
                });

                return result; // Returns the number of affected rows
            }
        }
        public async Task<int> DeleteProductAsync(long productId)
        {
            using (var connection = _context.GetConnection()) // Assuming _context.GetConnection() returns an open SQL connection
            {
                connection.Open();

                // Define the SQL delete query
                var query = @"
            DELETE FROM Tbl_Product
            WHERE ProductId = @ProductId";

                // Execute the delete query using Dapper
                var result = await connection.ExecuteAsync(query, new { ProductId = productId });

                return result; // Returns the number of affected rows
            }
        }
        


    }
}
