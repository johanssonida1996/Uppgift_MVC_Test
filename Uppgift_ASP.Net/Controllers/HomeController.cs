using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Uppgift_ASP.Net.Data;
using Uppgift_ASP.Net.Models;
using Uppgift_ASP.Net.Services.Identity;

namespace Uppgift_ASP.Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;      
        private readonly IIdentityService _identityService;

        public HomeController(ILogger<HomeController> logger, IIdentityService identityService)
        {

            _logger = logger;
            _identityService = identityService;
        }
         

        public async Task<IActionResult> Index()
        {

                await _identityService.CreateRootAccountAsync(); 

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
