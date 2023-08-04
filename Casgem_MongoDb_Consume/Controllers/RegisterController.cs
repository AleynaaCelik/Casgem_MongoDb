using Casgem_MongoDb_Consume.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Casgem_MongoDb_Consume.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User userCreate)
        {
            userCreate.Id = Guid.NewGuid().ToString();
            if (userCreate.UserName != null && userCreate.Password != null)
            {
                var client = _httpClientFactory.CreateClient();
                var JsonData = JsonConvert.SerializeObject(userCreate);
                StringContent content = new StringContent(JsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7207/api/User/add", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Login");
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}
