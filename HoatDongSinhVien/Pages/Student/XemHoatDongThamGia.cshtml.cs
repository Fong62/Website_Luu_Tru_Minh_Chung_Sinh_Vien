using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoatDongSinhVien.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoatDongSinhVien.Models.Data;
using Microsoft.AspNetCore.Authorization;

namespace HoatDongSinhVien.Pages.Student
{
    [Authorize(Roles ="HocSinh")]
    public class XemHoatDongThamGiaModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public XemHoatDongThamGiaModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedHocKy { get; set; }

        public List<SelectListItem> HocKyList { get; set; }
        public List<MinhChung> MinhChungList { get; set; }
        public KetQuaRenLuyen KetQuaTongKet { get; set; }
        public int TongDiem { get; set; } = 0;

        public async Task OnGetAsync()
        {
            // Load danh sách học kỳ
            HocKyList = await _context.HocKys
                .Select(h => new SelectListItem { Value = h.IDHocKy, Text = h.TenHocKy })
                .ToListAsync();

            if (!string.IsNullOrEmpty(SelectedHocKy))
            {
                // Lấy tất cả hoạt động sinh viên đã đăng ký và đang hiển thị
                string mssv = User.Identity.Name; // TODO: lấy từ session/Identity
                MinhChungList = await _context.MinhChungs
                    .Include(mc => mc.HoatDong)
                        .ThenInclude(h => h.LinhVuc)
                    .Where(mc => mc.MSSV == mssv
                              && mc.HoatDong.IDHocKy == SelectedHocKy
                              && mc.TrangThaiHienThi == "Đã duyệt")
                    .ToListAsync();

                // --- PHẦN 2: LẤY TỔNG ĐIỂM RÈN LUYỆN ---
                // Lấy trực tiếp từ bảng KetQuaRenLuyen theo MSSV và Học Kỳ
                KetQuaTongKet = await _context.KetQuaRenLuyens
                    .FirstOrDefaultAsync(kq => kq.MSSV == mssv && kq.IDHocKy == SelectedHocKy);
            }
            else
            {
                MinhChungList = new List<MinhChung>();
            }
        }
    }
}
