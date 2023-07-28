using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignelR_Practice.Models;
using SignelR_Practice.Services.Interface;
using System.Diagnostics;

namespace SignelR_Practice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IChatServices _chatServices;
        
        public HomeController(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }
        public async Task<IActionResult> Index(string Id)
        {
            return View(await _chatServices.GetChatHistory(Id));
        }

        public async Task<IActionResult> Privacy()
        {
            return View(await _chatServices.AllUsers());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}