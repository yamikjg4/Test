using BAL.Interface.ProductRepo;
using Domain.DTO;
using Domain.Enum;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepo _productrepo;
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;
        private readonly string apiurl;

        public ProductController(IProductRepo productrepo, IConfiguration configuration, HttpClient httpClient)
        {
            _productrepo = productrepo;
            this.configuration = configuration;
            _httpClient = httpClient;
            apiurl = configuration.GetValue<string>("ApiUrl:localhostApiUrl");
        }

        public IActionResult Index()
        {
           
            List<ListDto> lst=new List<ListDto>();
           
            return View(lst);
        }
        

        public async Task<IActionResult> Create()
        {
            var data2 = await _httpClient.GetAsync($"{apiurl}api/Category");

            if (data2.IsSuccessStatusCode)
            {

                ViewData["Currencies"] = Enum.GetValues(typeof(CurrencyEnum)).Cast<CurrencyEnum>().ToList();
                var content1 = await data2.Content.ReadAsStringAsync();
                var product1 = JsonConvert.DeserializeObject<ApiResponse>(content1);
                var datalist = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(product1.Result));
                ViewData["CategoryList"] = datalist;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product, int v)
        {
            var data2 = await _httpClient.GetAsync($"{apiurl}api/Category");

            if (data2.IsSuccessStatusCode)
            {

                ViewData["Currencies"] = Enum.GetValues(typeof(CurrencyEnum)).Cast<CurrencyEnum>().ToList();
                var content1 = await data2.Content.ReadAsStringAsync();
                var product1 = JsonConvert.DeserializeObject<ApiResponse>(content1);
                var datalist = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(product1.Result));
                ViewData["CategoryList"] = datalist;
            }
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiurl}api/product", content);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ViewBag["ErrorMessage"] = "Something Wrong";
                }
            }
            return View();
        }
        public async Task<IActionResult> Edit(long id)
        {
            
            var data2 = await _httpClient.GetAsync($"{apiurl}api/Category");
            
            if(  data2.IsSuccessStatusCode)
            {
               
                ViewData["Currencies"] = Enum.GetValues(typeof(CurrencyEnum)).Cast<CurrencyEnum>().ToList();
                var content1 = await data2.Content.ReadAsStringAsync();
                var product1 = JsonConvert.DeserializeObject<ApiResponse>(content1);
                var datalist= JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(product1.Result));
                ViewData["CategoryList"] = datalist;
            }
           
          
            var res = new ProductModel();
            var response = await _httpClient.GetAsync($"{apiurl}api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ApiResponse>(content);
                res = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(product.Result));
                return View(res);
            }

            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            var data2 = await _httpClient.GetAsync($"{apiurl}api/Category");

            if (data2.IsSuccessStatusCode)
            {

                ViewData["Currencies"] = Enum.GetValues(typeof(CurrencyEnum)).Cast<CurrencyEnum>().ToList();
                var content1 = await data2.Content.ReadAsStringAsync();
                var product1 = JsonConvert.DeserializeObject<ApiResponse>(content1);
                var datalist = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(product1.Result));
                ViewData["CategoryList"] = datalist;
            }
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiurl}api/Product", content);
               if(response.StatusCode== HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index","Product");
                }
                else
                {
                    ViewBag["ErrorMessage"] = "Something Wrong";
                }
            }
            return View();
        }
       
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{apiurl}api/Product/{id}");
            if (response.StatusCode==HttpStatusCode.NoContent)
            {
                return RedirectToAction(nameof(Index),"Product");
            }

            return RedirectToAction("Index", "Product");
        }
    }
}
