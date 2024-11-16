using BAL.Interface.ProductRepo;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private ApiResponse response;
        private readonly IProductRepo _repo;
        private readonly IConfiguration _config;

        public CategoryController(IMemoryCache cache,IProductRepo repo,IConfiguration config)
        {
            _cache = cache;
            response = new ApiResponse();
            _repo = repo;
            _config = config;   
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> Index()
        {
            string cacheKey = _config.GetValue<string>("Caches:CategoryCache");
            if (!_cache.TryGetValue(cacheKey, out response))
            {
                response = new ApiResponse();
                var AllCategory = await _repo.GetAllCategory();
                response.Result = AllCategory.ToList();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                };
                response.StatusCode = HttpStatusCode.OK;
                // Save data in cache
                _cache.Set(cacheKey, response, cacheOptions);
            }
            return Ok(response);

        }
    }
}
