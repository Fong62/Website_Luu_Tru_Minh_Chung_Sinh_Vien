using System.Diagnostics;
using HoatDongSinhVien.Models;
using Microsoft.AspNetCore.Mvc;

namespace HoatDongSinhVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToPage("/Admin/ThongKeTongQuan");
            }
            if (User.IsInRole("GiaoVien"))
            {
                return RedirectToPage("/GiangVien/ThongKeLop");
            }
            if (User.IsInRole("HocSinh") || User.IsInRole("BanCanSu"))
            {
                return RedirectToPage("/Student/XemHoatDongThamGia");
            }

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
