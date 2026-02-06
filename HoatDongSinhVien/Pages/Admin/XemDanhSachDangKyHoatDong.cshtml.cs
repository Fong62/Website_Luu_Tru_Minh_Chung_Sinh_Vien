using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace HoatDongSinhVien.Pages.Admin
{
    [Authorize(Roles = "Admin,GiaoVien,BanCanSu")]
    public class XemDanhSachDangKyHoatDongModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public XemDanhSachDangKyHoatDongModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string IDHoatDong { get; set; }
        public string TenHoatDong { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? KhoaFilter { get; set; }

        public List<string> Faculties { get; } = new List<string>  
        {
            "Ngữ Văn","Toán - Tin học","Công nghệ thông tin","Vật lý","Hóa học",
            "Sinh học","Lịch sử","Địa lý","Tiếng Anh","Tiếng Pháp",
            "Tiếng Nga","Tiếng Trung","Tiếng Nhật","Tiếng Hàn Quốc",
            "Giáo dục chính trị","Tâm lý học","Khoa học Giáo dục",
            "Giáo dục Tiểu học","Giáo dục Mầm non","Giáo dục Quốc phòng",
            "Giáo dục Đặc biệt","Giáo dục Thể chất","Tổ Nữ công"
        };

        public Paging PagingInfo { get; set; }

        public IList<DangKyHoatDong> DangKyHoatDongs { get; set; }

        public IActionResult OnGet(int trang = 1)
        { 
            if (string.IsNullOrEmpty(IDHoatDong))
            {
                return NotFound();
            }

            var hoatDong = _context.HoatDongs.FirstOrDefault(h => h.IDHoatDong == IDHoatDong);
            TenHoatDong = hoatDong.TenHoatDong;

            // Lấy danh sách đăng ký cho hoạt động với IDHoatDong
            var query = _context.DangKyHoatDongs
                .Include(d => d.SinhVien)  // Include SinhVien nếu bạn muốn hiển thị thông tin sinh viên
                    .ThenInclude(sv => sv.Lop)
                .Include(d => d.HoatDong)  // Include HoatDong nếu bạn muốn hiển thị thông tin hoạt động
                .Where(d => d.IDHoatDong == IDHoatDong);

            if (!string.IsNullOrEmpty(KhoaFilter))
            {
                var khoa = FromSlug(KhoaFilter);
                query = query.Where(d => d.TenKhoa == khoa);
            }

            int totalItems = query.Count();

            PagingInfo = new Paging
            {
                TotalItems = totalItems,
                ItemPerPage = 6,
                CurrentPage = trang
            };

            DangKyHoatDongs = query.Skip((trang - 1) * PagingInfo.ItemPerPage)
                             .Take(PagingInfo.ItemPerPage)
                             .ToList();

            this.KhoaFilter = KhoaFilter;

            return Page();
        }

        public async Task<IActionResult> OnPostImportExcelAsync(IFormFile? fileUpload, string IDHoatDong)
        {
            var hoatDongObj = await _context.HoatDongs.FirstOrDefaultAsync(h => h.IDHoatDong == IDHoatDong);
            if (hoatDongObj != null)
            {
                TenHoatDong = hoatDongObj.TenHoatDong;
            }

            if (fileUpload == null || fileUpload.Length == 0)
            {
                ModelState.AddModelError("fileUpload", "Vui lòng chọn file Excel.");
                return Page();
            }

            if (!fileUpload.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("fileUpload", "Vui lòng chọn file có định dạng .xlsx.");
                return Page();
            }

            // Cấu hình License cho EPPlus
            //ExcelPackage.License.SetNonCommercialPersonal("Student Project");

            try
            {
                using (var stream = new MemoryStream())
                {
                    await fileUpload.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new ExcelPackage(stream))
                    {
                        // Kiểm tra file có ít nhất một worksheet
                        if (package.Workbook.Worksheets.Count == 0)
                        {
                            ModelState.AddModelError("", "File Excel không có Sheet nào.");
                            return Page();
                        }

                        var worksheet = package.Workbook.Worksheets[0];

                        var headerHoTen = worksheet.Cells[1, 2].Text?.Trim().ToLower(); // Đọc ô B1
                        var headerMSSV = worksheet.Cells[1, 3].Text?.Trim().ToLower();  // Đọc ô C1

                        // Kiểm tra xem tiêu đề có chứa từ khóa mong muốn không
                        // Dùng .Contains để linh hoạt (ví dụ người dùng ghi "Họ tên" hay "Họ và tên" đều được)
                        bool isDungMau = true;

                        if (string.IsNullOrEmpty(headerHoTen) || !headerHoTen.Contains("tên")) isDungMau = false;
                        if (string.IsNullOrEmpty(headerMSSV) || !headerMSSV.Contains("mã số sinh viên")) isDungMau = false;

                        if (!isDungMau)
                        {
                            ModelState.AddModelError("", "File tải lên không đúng mẫu quy định! Vui lòng kiểm tra lại file Excel.");
                            return Page();
                        }

                        // Kiểm tra Dimension để đảm bảo có dữ liệu
                        if (worksheet.Dimension == null)
                        {
                            ModelState.AddModelError("", "Sheet đầu tiên không có dữ liệu.");
                            return Page();
                        }

                        var daDangKyMSSVs = await _context.DangKyHoatDongs
                            .Where(d => d.IDHoatDong == IDHoatDong)
                            .Select(d => d.MSSV)
                            .ToListAsync();

                        var processedMSSVs = new HashSet<string>(daDangKyMSSVs);

                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var strThoiGian = worksheet.Cells[row, 1]?.Text;

                            if (string.IsNullOrWhiteSpace(strThoiGian)) continue;

                            // Lấy các giá trị từ các ô và đảm bảo chúng không null
                            var hoTen = worksheet.Cells[row, 2]?.Text;
                            var mssv = worksheet.Cells[row, 3]?.Text;
                            var tenKhoa = worksheet.Cells[row, 4]?.Text;

                            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(tenKhoa))
                            {
                                ModelState.AddModelError("", "Sheet thiếu thông tin đầu vào");
                                return Page();
                            }

                            if (processedMSSVs.Contains(mssv))
                            {
                                continue;
                            }

                            var dangKyHoatDong = new DangKyHoatDong
                            {
                                ThoiGianDangKy = DateTime.TryParse(strThoiGian, out DateTime dt) ? dt : DateTime.Now,
                                HoTen = hoTen,
                                MSSV = mssv,
                                TenKhoa = tenKhoa,
                                IDHoatDong = IDHoatDong
                            };

                            // Kiểm tra _context.DangKyHoatDongs trước khi thêm vào
                            if (_context.DangKyHoatDongs == null)
                            {
                                throw new InvalidOperationException("DangKyHoatDongs không được khởi tạo.");
                            }

                            _context.DangKyHoatDongs.Add(dangKyHoatDong);

                            processedMSSVs.Add(mssv);
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                TempData["Message"] = "Dữ liệu đã được import thành công!";
                return RedirectToPage(new { IDHoatDong = IDHoatDong });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi hệ thống: {ex.Message}");
                return Page();
            }
        }
        private string FromSlug(string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : value.Replace("-", " ");
        }
    }
}
