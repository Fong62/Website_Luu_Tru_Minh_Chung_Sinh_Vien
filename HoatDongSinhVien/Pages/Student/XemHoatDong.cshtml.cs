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

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(IDHoatDong))
            {
                return NotFound();
            }

            // Tìm hoạt động dựa trên ID
            HoatDong = _context.HoatDongs
                .Include(h => h.LinhVuc)
                .Where(h => h.IDHoatDong == IDHoatDong)
                .FirstOrDefault();

            if (HoatDong == null)
            {
                return NotFound();
            }

            if (HoatDong.ChoPhepDiemDanh && HoatDong.ThoiGianDongDiemDanh.HasValue)
            {
                if (DateTime.Now > HoatDong.ThoiGianDongDiemDanh.Value)
                {
                    // Đã quá hạn! Tự động khóa lại
                    HoatDong.ChoPhepDiemDanh = false;
                    await _context.SaveChangesAsync(); // Cập nhật ngay xuống CSDL
                }
            }

            // Lấy mã QR đăng ký và điểm danh
            QRCodeDangKyUrl = HoatDong.QRCodeDangKy;
            QRCodeDiemDanhUrl = HoatDong.QRCodeDiemDanh;

            return Page();
        }
        public async Task<IActionResult> OnPostToggleDiemDanhAsync(string id)
        {
            var hoatDong = await _context.HoatDongs.FindAsync(id);
            if (hoatDong != null)
            {
                hoatDong.ChoPhepDiemDanh = !hoatDong.ChoPhepDiemDanh;

                if (hoatDong.ChoPhepDiemDanh)
                {
                    // Nếu vừa mở lên -> Cài đặt mốc đóng cửa là 2 phút sau (tính từ hiện tại)
                    hoatDong.ThoiGianDongDiemDanh = DateTime.Now.AddMinutes(2);
                }
                else
                {
                    hoatDong.ThoiGianDongDiemDanh = null;
                }

                await _context.SaveChangesAsync();
            }

            // Load lại trang hiện tại
            return RedirectToPage(new { IDHoatDong = id });
        }
    }
}
