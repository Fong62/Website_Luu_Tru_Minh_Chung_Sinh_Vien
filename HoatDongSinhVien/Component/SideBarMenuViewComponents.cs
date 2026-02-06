using Microsoft.AspNetCore.Mvc;

namespace HoatDongSinhVien.Component
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SidebarMenu");
        }
    }
}