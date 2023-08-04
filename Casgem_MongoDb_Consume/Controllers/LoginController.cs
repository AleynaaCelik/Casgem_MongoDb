using Casgem_MongoDb_Consume.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Casgem_MongoDb_Consume.Controllers
{

    public class LoginController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public LoginController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}


		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(User user1)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7207/api/User/getall");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<User>>(jsonData);
				var result = values.FirstOrDefault(x => x.UserName == user1.UserName && x.Password == user1.Password);
				if (result != null)
				{
					var user = result.UserName;
					HttpContext.Session.SetString("UserName", result.UserName);
                    ViewData["UserName"] = result.UserName;

                    return RedirectToAction("Index", "Default");
				}
			}
			return View();
		}
	}
}
