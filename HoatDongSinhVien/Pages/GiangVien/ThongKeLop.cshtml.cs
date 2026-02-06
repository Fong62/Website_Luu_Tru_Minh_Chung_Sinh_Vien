using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoatDongSinhVien.Pages.GiangVien
{
    [Authorize(Roles = "GiaoVien")]
    public class ThongKeLopModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public ThongKeLopModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        // --- BỘ LỌC ---
        [BindProperty(SupportsGet = true)]
        public string IDLop { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IDHocKy { get; set; }

        public List<SelectListItem> LopHocs { get; set; }
        public List<SelectListItem> HocKys { get; set; }

        // --- SỐ LIỆU THỐNG KÊ ---
        public int TongSinhVien { get; set; }
        public int SinhVienThamGia { get; set; } // Số SV có ít nhất 1 hoạt động
        public int TongMinhChungChoDuyet { get; set; }
        public int TongLuotDangKy { get; set; }

        // --- BIỂU ĐỒ & TOP SV ---
        public List<string> ChartLabels { get; set; }
        public List<int> ChartValues { get; set; }
        public List<SinhVienScoreViewModel> TopSinhVien { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Lấy ID Giảng viên (Giả sử Username là Mã GV)
            var idGiangVien = User.Identity.Name;

            // 2. Load Dropdown Học Kỳ
            HocKys = await _context.HocKys
                .OrderBy(h => h.IDHocKy)
                .Select(h => new SelectListItem { Value = h.IDHocKy, Text = h.TenHocKy })
                .ToListAsync();

            // 3. Load Dropdown Lớp (CHỈ LẤY LỚP CỦA GV NÀY)
            var queryLop = _context.Lops.Where(l => l.IDGiangVien == idGiangVien);

            LopHocs = await queryLop
                .Select(l => new SelectListItem { Value = l.IDLop, Text = l.IDLop + "-" + l.TenLop })
                .ToListAsync();

            LopHocs.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả lớp quản lý --" });

            // 4. BẮT ĐẦU LỌC DỮ LIỆU

            // a. Lọc Sinh viên thuộc phạm vi quản lý
            var querySV = _context.SinhViens.AsQueryable();

            if (!string.IsNullOrEmpty(IDLop))
            {
                querySV = querySV.Where(sv => sv.IDLop == IDLop);
            }
            else
            {
                // Nếu chọn "Tất cả", lấy tất cả SV thuộc các lớp do GV này quản lý
                var danhSachLopCuaGV = await queryLop.Select(l => l.IDLop).ToListAsync();
                querySV = querySV.Where(sv => danhSachLopCuaGV.Contains(sv.IDLop));
            }

            // Lấy danh sách MSSV để query bảng liên quan
            var listMSSV = await querySV.Select(s => s.MSSV).ToListAsync();
            TongSinhVien = listMSSV.Count;

            // b. Truy vấn Hoạt động & Minh chứng theo Học kỳ
            var queryDangKy = _context.DangKyHoatDongs
                .Include(dk => dk.HoatDong)
                .Where(dk => listMSSV.Contains(dk.MSSV));

            var queryMinhChung = _context.MinhChungs
                .Include(mc => mc.HoatDong)
                .Where(mc => listMSSV.Contains(mc.MSSV));

            // Áp dụng bộ lọc Học kỳ
            if (!string.IsNullOrEmpty(IDHocKy))
            {
                queryDangKy = queryDangKy.Where(dk => dk.HoatDong.IDHocKy == IDHocKy);
                queryMinhChung = queryMinhChung.Where(mc => mc.HoatDong.IDHocKy == IDHocKy);
            }

            // --- TÍNH TOÁN CON SỐ ---
            TongLuotDangKy = await queryDangKy.CountAsync();

            TongMinhChungChoDuyet = await queryMinhChung
                .Where(mc => mc.TrangThaiHienThi == "Chờ duyệt")
                .CountAsync();

            SinhVienThamGia = await queryDangKy
                .Select(dk => dk.MSSV)
                .Distinct()
                .CountAsync();

            // --- DỮ LIỆU BIỂU ĐỒ ---
            var statsMC = await queryMinhChung
                .GroupBy(mc => mc.TrangThaiHienThi)
                .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
                .ToListAsync();

            ChartLabels = statsMC.Select(x => x.TrangThai ?? "Chưa xử lý").ToList();
            ChartValues = statsMC.Select(x => x.SoLuong).ToList();

            // --- TOP 5 SINH VIÊN TÍCH CỰC ---
            TopSinhVien = await queryDangKy
                .GroupBy(dk => dk.MSSV)
                .Select(g => new SinhVienScoreViewModel
                {
                    MSSV = g.Key,
                    SoLuotThamGia = g.Count()
                })
                .OrderByDescending(x => x.SoLuotThamGia)
                .Take(5)
                .ToListAsync();

            var topMSSVs = TopSinhVien.Select(x => x.MSSV).ToList();
            var infoSVs = await _context.SinhViens
                .Where(s => topMSSVs.Contains(s.MSSV))
                .ToDictionaryAsync(s => s.MSSV, s => s.HoTen);

            foreach (var item in TopSinhVien)
            {
                if (infoSVs.ContainsKey(item.MSSV)) item.HoTen = infoSVs[item.MSSV];
            }
        }

        public class SinhVienScoreViewModel
        {
            public string MSSV { get; set; }
            public string HoTen { get; set; }
            public int SoLuotThamGia { get; set; }
        }
    }
}