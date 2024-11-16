using BAL.Interface.ProductRepo;
using DAL.Repositry;
using Domain.DTO;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Linq;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private ApiResponse response;
        private readonly IProductRepo _repo;
        private readonly IConfiguration _config;
        private string cachekey;

        public ProductController(IMemoryCache cache, IProductRepo repo, IConfiguration config)
        {
            _cache = cache;
            response = new ApiResponse();
            _repo = repo;
            _config = config;
            cachekey = _config.GetValue<string>("Caches:ProductCache");
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]

        public async Task<ActionResult<ApiResponse>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            cachekey = cachekey + $"?pageNumber={pageNumber}&pageSize={pageSize}";
            if (!_cache.TryGetValue(cachekey, out response))
            {
                response = new ApiResponse();
                var res = await _repo.GetList(pageNumber, pageSize);
                response.Result = res.Item1;
                response.Count = res.cnt;
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };
                response.StatusCode = HttpStatusCode.OK;
                response.PageNumber = pageNumber;
                // Save data in cache
                _cache.Set(cachekey, response, cacheOptions);
            }
            return Ok(response);
        }
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostData([FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                int id = await _repo.InsertProductAsync(model);
                if (id > 0)
                {
                    _cache.Remove(cachekey + $"?pageNumber=1&pageSize=10");
                    return CreatedAtAction(nameof(GetProducts), new { pageNumber = 1, pageSize = 10 });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
        }
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]

        public async Task<ActionResult<ApiResponse>> GetProductById(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (await _repo.GetProductById(id) == null)
            {
                return NotFound();
            }
            else
            {
                response = new ApiResponse();
                response.Result = await _repo.GetProductById(id);
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateData(ProductModel model)
        {
            if (model.ProductId == 0)
            {
                return StatusCode(500, "Product Id Need For Update");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                int id = await _repo.UpdateProductAsync(model);
                if (id > 0)
                {
                    _cache.Remove(cachekey + $"?pageNumber=1&pageSize=10");
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteProduct(long id)
        {
            int data = await _repo.DeleteProductAsync(id);
            if (data > 0)
            {
                _cache.Remove(cachekey + $"?pageNumber=1&pageSize=10");
                return NoContent();
            }
            else
            {
                return BadRequest();
               
            }
        }
    }
}
