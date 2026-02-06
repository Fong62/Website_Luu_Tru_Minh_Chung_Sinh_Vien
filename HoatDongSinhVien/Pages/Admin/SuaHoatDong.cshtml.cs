using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.Services_Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HoatDongSinhVien.Pages.Admin
{
    public class SuaHoatDongModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;
        private readonly ITaoUpdateGGForm _taoUpdateGGForm;

        public SuaHoatDongModel(HoatDongSinhVienDbcontext context, ITaoUpdateGGForm taoUpdateGGForm)
        {
            _context = context;
            _taoUpdateGGForm = taoUpdateGGForm;
        }

        [BindProperty(SupportsGet = true)]
        public string IDHoatDong { get; set; }

        [BindProperty]
        public string OldName { get; set; }

        [BindProperty]
        public string NewName { get; set; }

        [BindProperty]
        public string NewMoTa { get; set; }

        [BindProperty]
        public DateTime NgayToChuc { get; set; }

        [BindProperty]
        public string SelectedIDLinhVuc { get; set; }
        public List<SelectListItem> LinhVucList { get; set; }

        [BindProperty]
        public string SelectedIDHocKy { get; set; }
        public List<SelectListItem> HocKyList { get; set; }

        [BindProperty]
        public string SelectedTrangThaiHienThi { get; set; }
        public List<SelectListItem> TrangThaiHienThiList { get; set; }

        public string ResultMessage { get; set; }
        public bool IsSuccess { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy thông tin hoạt động từ DB theo IDHoatDong
            var hoatDong = await _context.HoatDongs
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.IDHoatDong == IDHoatDong);

            if (hoatDong == null)
                return NotFound("Không tìm thấy hoạt động này.");

            OldName = hoatDong.TenHoatDong;
            NewName = hoatDong.TenHoatDong;
            NewMoTa = hoatDong.MoTa;
            NgayToChuc = hoatDong.NgayToChuc;
            SelectedIDLinhVuc = hoatDong.IDLinhVuc;
            SelectedIDHocKy = hoatDong.IDHocKy;
            SelectedTrangThaiHienThi = hoatDong.TrangThaiHienThi;

            // Load danh sách dropdown
            LinhVucList = await _context.LinhVucs
                .Select(lv => new SelectListItem { Value = lv.IDLinhVuc, Text = lv.TenLinhVuc })
                .ToListAsync();

            HocKyList = await _context.HocKys
                .Select(hk => new SelectListItem { Value = hk.IDHocKy, Text = hk.TenHocKy })
                .ToListAsync();

            TrangThaiHienThiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Hiển thị", Text = "Hiển thị" },
                new SelectListItem { Value = "Ẩn", Text = "Ẩn" }
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var response = await _taoUpdateGGForm.UpdateGoogleFormAsync(OldName, NewName, NewMoTa);
                ResultMessage = response;
                IsSuccess = response.StartsWith("Success");

                if (IsSuccess)
                {
                    // Cập nhật luôn trong database
                    var hoatDong = await _context.HoatDongs.FirstOrDefaultAsync(h => h.IDHoatDong == IDHoatDong);
                    if (hoatDong != null)
                    {
                        hoatDong.TenHoatDong = NewName;
                        hoatDong.MoTa = NewMoTa;
                        hoatDong.NgayToChuc = NgayToChuc;
                        hoatDong.IDLinhVuc = SelectedIDLinhVuc;
                        hoatDong.IDHocKy = SelectedIDHocKy;
                        hoatDong.TrangThaiHienThi = SelectedTrangThaiHienThi;

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                ResultMessage = "Có lỗi xảy ra khi gọi Google Apps Script.";
                IsSuccess = false;
            }

            return Page();
        }
    }
}
