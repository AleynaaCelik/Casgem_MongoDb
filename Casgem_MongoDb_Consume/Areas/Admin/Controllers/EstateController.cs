using Casgem_MongoDb_Consume.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Casgem_MongoDb_Consume.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EstateController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EstateController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("UserName");
            ViewBag.userName = username;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7207/api/Estate/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Estate>>(jsonData);
                var userEstates = values.Where(e => e.UserName == username).ToList();
                return View(userEstates);
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddEstate()
        {
            var username = HttpContext.Session.GetString("UserName");
            ViewBag.userName = username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEstate(Estate estate)
        {
            var username = HttpContext.Session.GetString("UserName");
                
            estate.UserName = username;
            estate.Id = Guid.NewGuid().ToString();

            var jsonData = JsonConvert.SerializeObject(estate);
            var jsonDocument = JObject.Parse(jsonData);

            var httpContent = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonDocument.ToString()));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var responseMessage = await client.PostAsync("https://localhost:7207/api/Estate/add", httpContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Estate", "Admin");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEstate(string id)
        {

            
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7207/api/Estate/get?id={id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Estate>(jsonData);
                return View(value);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEstate(Estate estate)
        {
            var username = HttpContext.Session.GetString("UserName");

            estate.UserName = username;

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(estate);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"https://localhost:7207/api/Estate/update?id={estate.Id}", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Estate", "Admin");
            }
            return View();
        }


        public async Task<IActionResult> DeleteEstate(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7207/api/Estate/delete?id={id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Estate", "Admin");
            }
            return View();

        }



    }
}
