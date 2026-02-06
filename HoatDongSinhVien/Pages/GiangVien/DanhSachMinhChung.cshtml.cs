using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HoatDongSinhVien.Pages.GiangVien
{
    [Authorize(Roles ="Admin,GiaoVien,BanCanSu")]
    public class DanhSachMinhChungModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public DanhSachMinhChungModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? IDHocKy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? IDHoatDong { get; set; }

        public Paging PagingInfo { get; set; }
        public List<HocKy> HocKys { get; set; } = new();
        public List<HoatDong> HoatDongs { get; set; } = new();
        public List<MinhChung> MinhChungs { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int trang = 1)
        {
            HocKys = await _context.HocKys
                .OrderBy(h => h.NamHoc)
                .ThenBy(h => h.TenHocKy)
                .ToListAsync();

            // Load dropdown hoạt động
            if (!string.IsNullOrWhiteSpace(IDHocKy))
            {
                HoatDongs = await _context.HoatDongs
                    .Where(hd => hd.IDHocKy == IDHocKy)
                    .OrderByDescending(hd => hd.NgayToChuc)
                    .ToListAsync();
            }

            // Chưa chọn hoạt động
            if (string.IsNullOrWhiteSpace(IDHocKy))
            {
                ErrorMessage = "Vui lòng chọn học kỳ để xem minh chứng.";
                return Page();
            }

            // Chưa chọn hoạt động
            if (string.IsNullOrWhiteSpace(IDHoatDong))
            {
                ErrorMessage = "Vui lòng chọn hoạt động để xem minh chứng.";
                return Page();
            }

            var query = _context.MinhChungs
                .Include(x => x.SinhVien)
                .Where(x => x.IDHoatDong == IDHoatDong)
                .AsQueryable();

            // ===== PAGING =====
            int totalItems = await query.CountAsync();

            PagingInfo = new Paging
            {
                TotalItems = totalItems,
                ItemPerPage = 8,
                CurrentPage = trang
            };

            MinhChungs = await query
                .OrderByDescending(x => x.ThoiGianDiemDanh)
                .Skip((trang - 1) * PagingInfo.ItemPerPage)
                .Take(PagingInfo.ItemPerPage)
                .ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostDuyetAsync(string mssv, string idHoatDong)
        {
            // Load dropdown lại để không bị null khi return Page
            HoatDongs = await _context.HoatDongs
                .OrderByDescending(x => x.NgayToChuc)
                .ToListAsync();

            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(idHoatDong))
            {
                ErrorMessage = "Thiếu dữ liệu MSSV hoặc ID hoạt động.";
                return await ReloadAndReturnPage(idHoatDong);
            }

            // Lấy minh chứng
            var minhChung = await _context.MinhChungs
                .FirstOrDefaultAsync(x => x.MSSV == mssv && x.IDHoatDong == idHoatDong);

            if (minhChung == null)
            {
                ErrorMessage = "Không tìm thấy minh chứng.";
                return await ReloadAndReturnPage(idHoatDong);
            }

            if (minhChung.TrangThaiHienThi == "Đã duyệt")
            {
                return RedirectToPage("/GiangVien/DanhSachMinhChung", new { IDHoatDong = idHoatDong });
            }

            // Lấy hoạt động + lĩnh vực để lấy điểm + học kỳ
            var hoatDong = await _context.HoatDongs
                .Include(x => x.LinhVuc)
                .FirstOrDefaultAsync(x => x.IDHoatDong == idHoatDong);

            if (hoatDong == null)
            {
                ErrorMessage = "Không tìm thấy hoạt động.";
                return await ReloadAndReturnPage(idHoatDong);
            }

            if (string.IsNullOrWhiteSpace(hoatDong.IDHocKy))
            {
                ErrorMessage = "Hoạt động chưa có học kỳ (IDHocKy).";
                return await ReloadAndReturnPage(idHoatDong);
            }

            var thangDiem = hoatDong.LinhVuc?.ThangDiem ?? 0;
            var diemCong = Convert.ToDecimal(thangDiem); // double -> decimal

            // Join vào KQRL theo (MSSV, IDHocKy)
            var kq = await _context.KetQuaRenLuyens
                .FirstOrDefaultAsync(x => x.MSSV == mssv && x.IDHocKy == hoatDong.IDHocKy);

            if (kq == null)
            {
                kq = new KetQuaRenLuyen
                {
                    MSSV = mssv,
                    IDHocKy = hoatDong.IDHocKy,
                    DiemRenLuyen = diemCong,
                    ThanhTich = "" // bạn có thể gán null nếu cho phép
                };
                _context.KetQuaRenLuyens.Add(kq);
            }
            else
            {
                kq.DiemRenLuyen += diemCong;
            }

            // Cập nhật minh chứng đã duyệt
            minhChung.TrangThaiHienThi = "Đã duyệt";

            await _context.SaveChangesAsync();

            SuccessMessage = "Duyệt minh chứng thành công";

            // quay lại trang với filter đang chọn
            return RedirectToPage("/GiangVien/DanhSachMinhChung", new { IDHoatDong = idHoatDong });
        }

        // Helper reload list minh chứng để hiển thị lại Page nếu có lỗi
        private async Task<IActionResult> ReloadAndReturnPage(string idHoatDong)
        {
            IDHoatDong = idHoatDong;
            HocKys = await _context.HocKys
                .OrderBy(h => h.NamHoc).ThenBy(h => h.TenHocKy)
                .ToListAsync();

            var currentHoatDong = await _context.HoatDongs.FindAsync(idHoatDong);
            if (currentHoatDong != null)
            {
                IDHocKy = currentHoatDong.IDHocKy; // Gán lại IDHocKy để View hiển thị đúng
                HoatDongs = await _context.HoatDongs
                    .Where(hd => hd.IDHocKy == IDHocKy)
                    .OrderByDescending(hd => hd.NgayToChuc)
                    .ToListAsync();
            }

            MinhChungs = await _context.MinhChungs
                .Include(x => x.SinhVien)
                .Where(x => x.IDHoatDong == idHoatDong)
                .OrderByDescending(x => x.ThoiGianDiemDanh)
                .ToListAsync();

            return Page();
        }
    }
}
