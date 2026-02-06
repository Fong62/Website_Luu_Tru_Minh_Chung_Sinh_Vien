using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace HoatDongSinhVien.Pages.GiangVien
{
    [Authorize(Roles = "GiaoVien,BanCanSu")]
    public class XemHoatDongLopThamGiaModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public XemHoatDongLopThamGiaModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedHocKy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedLop { get; set; }

        public string? TenLop { get; set; }

        public List<SelectListItem> LopList { get; set; }
        public List<SelectListItem> HocKyList { get; set; }
        public List<SinhVien> SinhViens { get; set; } = new();
        public Paging PagingInfo { get; set; }
        int PageSize = 5;
        public async Task OnGetAsync(int trang = 1)
        {
            // Load danh sách học kỳ
            HocKyList = await _context.HocKys
                .Select(h => new SelectListItem { Value = h.IDHocKy, Text = h.TenHocKy })
                .ToListAsync();

            if (User.IsInRole("GiaoVien"))
            {
                string CurrentGiangVienID = User.Identity.Name;
              
                LopList = await _context.Lops
                    .Where(l => l.IDGiangVien == CurrentGiangVienID)
                    .Select(l => new SelectListItem { Value = l.IDLop, Text = l.IDLop + " - " + l.TenLop })
                    .ToListAsync();

                if (string.IsNullOrEmpty(SelectedHocKy) || string.IsNullOrEmpty(SelectedLop))
                    return;
            }
            if (User.IsInRole("BanCanSu"))
            {
                string mssv = User.Identity.Name;
                SelectedLop = await _context.SinhViens
                     .Where(sv => sv.MSSV == mssv)
                     .Select(sv => sv.IDLop)
                     .FirstOrDefaultAsync();

                TenLop = await _context.Lops.Where(l=>l.IDLop == SelectedLop)
                      .Select(l=>l.TenLop).FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(SelectedLop))
                {
                    return;
                }
            }

            var query = _context.SinhViens
               .Where(sv => sv.IDLop == SelectedLop)
               .Include(sv => sv.Lop)
               .Include(sv => sv.KetQuaRenLuyens
                   .Where(kq => kq.IDHocKy == SelectedHocKy))
               .AsQueryable();
            int totalItems = await query.CountAsync();

            SinhViens = await query
                .OrderBy(sv => sv.MSSV)
                .Skip((trang - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            PagingInfo = new Paging
            {
                CurrentPage = trang,
                ItemPerPage = PageSize,
                TotalItems = totalItems
            };
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {
            string CurrentGiangVienID = User.Identity.Name;

            if (string.IsNullOrEmpty(SelectedHocKy) || string.IsNullOrEmpty(SelectedLop))
            {
                TempData["Error"] = "Vui lòng chọn học kỳ và lớp trước khi export.";
                return RedirectToPage();
            }

            var sinhViens = await _context.SinhViens
                .Where(sv => sv.IDLop == SelectedLop)
                .Include(sv => sv.Lop)
                .Include(sv => sv.KetQuaRenLuyens
                    .Where(kq => kq.IDHocKy == SelectedHocKy))
                .ToListAsync();

            var tenHocKy = await _context.HocKys
                .Where(h => h.IDHocKy == SelectedHocKy)
                .Select(h => h.TenHocKy)
                .FirstOrDefaultAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Danh sách rèn luyện");

            // Header
            ws.Cells[1, 1].Value = "MSSV";
            ws.Cells[1, 2].Value = "Họ tên";
            ws.Cells[1, 3].Value = "Lớp";
            ws.Cells[1, 4].Value = "Học kỳ";
            ws.Cells[1, 5].Value = "Điểm rèn luyện";
            ws.Cells[1, 6].Value = "Thành tích";

            using (var range = ws.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var sv in sinhViens)
            {
                var kq = sv.KetQuaRenLuyens.FirstOrDefault();

                ws.Cells[row, 1].Value = sv.MSSV;
                ws.Cells[row, 2].Value = sv.HoTen;
                ws.Cells[row, 3].Value = sv.Lop?.IDLop + " - " + sv.Lop?.TenLop;
                ws.Cells[row, 4].Value = tenHocKy;
                ws.Cells[row, 5].Value = kq?.DiemRenLuyen ?? 0;
                ws.Cells[row, 6].Value = kq?.ThanhTich ?? "-";
                row++;
            }

            int lastRow = row - 1;
            if (lastRow >= 1)
            {
                var tableRange = ws.Cells[1, 1, lastRow, 6];

                tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                for (int i = 1; i <= lastRow; i++)
                {
                    ws.Row(i).Height = 25;
                    ws.Row(i).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
            }

            ws.Cells.AutoFitColumns();

            Response.Cookies.Append("fileDownloadToken", "true", new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                Secure = false
            });

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"Điểm Rèn Luyện_ Lớp {SelectedLop}_{tenHocKy}.xlsx";

            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
