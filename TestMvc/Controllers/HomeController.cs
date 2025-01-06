using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using TestMvc.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;
        private readonly string apiurl;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            this.configuration = configuration;
            _httpClient = httpClient;
            apiurl = configuration.GetValue<string>("ApiUrl:localhostApiUrl");
        }
        public async Task<IActionResult> Index()
        {
            string Username = ""; 
            var response = await _httpClient.GetAsync(apiurl + "api/User");
            if (response.IsSuccessStatusCode)
            {
                var content1 = await response.Content.ReadAsStringAsync();
                Username = content1;
            }
            Console.WriteLine(Username);
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
