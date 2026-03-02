using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Cần thêm cái này
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoatDongSinhVien.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ThongKeTongQuanModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public ThongKeTongQuanModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        // --- FILTER HỌC KỲ ---
        [BindProperty(SupportsGet = true)]
        public string SelectedHocKy { get; set; }
        public List<SelectListItem> HocKyList { get; set; }

        // --- CÁC BIẾN SỐ LIỆU ---
        public int TongSinhVien { get; set; }
        public int TongHoatDong { get; set; }
        public int MinhChungChoDuyet { get; set; }
        public int LuotDangKy { get; set; }

        // --- Dữ liệu biểu đồ & List ---
        public List<string> ChartLabels { get; set; }
        public List<int> ChartValues { get; set; }
        public List<HoatDong> HoatDongMoi { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Load danh sách học kỳ vào Dropdown
            HocKyList = await _context.HocKys
                .OrderBy(h => h.IDHocKy)
                .Select(h => new SelectListItem
                {
                    Value = h.IDHocKy,
                    Text = h.TenHocKy
                })
                .ToListAsync();

            // Thêm lựa chọn "Tất cả"
            HocKyList.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả các kỳ --" });

            // 2. Khởi tạo Query cơ bản cho Hoạt động
            var queryHoatDong = _context.HoatDongs.AsQueryable();

            if (!string.IsNullOrEmpty(SelectedHocKy))
            {
                queryHoatDong = queryHoatDong.Where(h => h.IDHocKy == SelectedHocKy);
            }

            // --- TÍNH TOÁN SỐ LIỆU ---

            // Tổng sinh viên
            TongSinhVien = await _context.SinhViens.CountAsync();

            // Tổng hoạt động (Theo kỳ đã chọn)
            TongHoatDong = await queryHoatDong.CountAsync();

            // Minh chứng chờ duyệt
            var queryMinhChung = _context.MinhChungs.Include(mc => mc.HoatDong).AsQueryable();
            if (!string.IsNullOrEmpty(SelectedHocKy))
            {
                queryMinhChung = queryMinhChung.Where(mc => mc.HoatDong.IDHocKy == SelectedHocKy);
            }
            MinhChungChoDuyet = await queryMinhChung
                .Where(mc => mc.TrangThaiHienThi == "Chờ duyệt" || mc.TrangThaiHienThi == null)
                .CountAsync();

            // Lượt đăng ký
            var queryDangKy = _context.DangKyHoatDongs.Include(dk => dk.HoatDong).AsQueryable();
            if (!string.IsNullOrEmpty(SelectedHocKy))
            {
                queryDangKy = queryDangKy.Where(dk => dk.HoatDong.IDHocKy == SelectedHocKy);
            }
            LuotDangKy = await queryDangKy.CountAsync();

            // 3. Dữ liệu biểu đồ
            var chartData = await queryHoatDong
                .Include(h => h.LinhVuc)
                .GroupBy(h => h.LinhVuc.TenLinhVuc)
                .Select(g => new
                {
                    LinhVuc = g.Key,
                    SoLuong = g.Count()
                })
                .ToListAsync();

            ChartLabels = chartData.Select(x => x.LinhVuc).ToList();
            ChartValues = chartData.Select(x => x.SoLuong).ToList();

            // 4. Lấy 5 hoạt động mới nhất
            HoatDongMoi = await queryHoatDong
                .Include(h => h.LinhVuc)
                .OrderByDescending(h => h.NgayToChuc)
                .Take(5)
                .ToListAsync();
        }
    }
}