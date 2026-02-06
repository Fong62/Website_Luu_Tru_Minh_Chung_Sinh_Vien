using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HoatDongSinhVien.Pages.Student
{
    public class XemHoatDongModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public XemHoatDongModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string IDHoatDong { get; set; }

        public HoatDong HoatDong { get; set; }
        public string QRCodeDangKyUrl { get; set; }
        public string QRCodeDiemDanhUrl { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(IDHoatDong))
            {
                return NotFound();
            }

            // Tìm hoạt động dựa trên ID
            HoatDong = _context.HoatDongs
                .Include(h => h.LinhVuc)  // Include LinhVuc nếu cần hiển thị thông tin lĩnh vực
                .Where(h => h.IDHoatDong == IDHoatDong)
                .FirstOrDefault();

            if (HoatDong == null)
            {
                return NotFound();
            }

            // Lấy mã QR đăng ký và điểm danh
            QRCodeDangKyUrl = HoatDong.QRCodeDangKy;
            QRCodeDiemDanhUrl = HoatDong.QRCodeDiemDanh;

            return Page();
        }
    }
}
