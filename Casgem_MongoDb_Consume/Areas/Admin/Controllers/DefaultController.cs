using Casgem_MongoDb_Consume.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Casgem_MongoDb_Consume.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DefaultController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        
    }
}
