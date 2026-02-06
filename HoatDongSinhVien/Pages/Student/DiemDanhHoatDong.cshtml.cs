using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.Services_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HoatDongSinhVien.Pages.Student
{
    [Authorize(Roles ="HocSinh")]
    public class DiemDanhHoatDongModel : PageModel
    {
        private readonly InterfaceOCR _ocr;
        private readonly HoatDongSinhVienDbcontext _context;
        private readonly IWebHostEnvironment _env;

        public DiemDanhHoatDongModel(InterfaceOCR ocr, HoatDongSinhVienDbcontext context, IWebHostEnvironment env)
        {
            _ocr = ocr;
            _context = context;
            _env = env;
        }

        [BindProperty]
        public IFormFile AnhThe { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IDHoatDong { get; set; }

        public MinhChung KetQua { get; set; }

        public IActionResult OnGet() 
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AnhThe == null || AnhThe.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ảnh thẻ sinh viên";
                return Page();
            }

            var webRootPath = _env.WebRootPath;
            var folder = Path.Combine(webRootPath, "imagediemdanh");

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AnhThe.FileName)}";
            var filePath = Path.Combine(folder, fileName);
            // -------------------------

            using (var stream = new FileStream(filePath, FileMode.Create))
                await AnhThe.CopyToAsync(stream);

            KetQua = await _ocr.ExtractMinhChungFromFlask(filePath, IDHoatDong);
            Console.WriteLine("Mssv da ocr: KetQua.MSSV");
            var mssv = KetQua.MSSV?.Trim();

            bool sinhVienTonTai = await _context.SinhViens
                .AnyAsync(sv => sv.MSSV == mssv);

            if (!sinhVienTonTai)
            {
                TempData["ErrorDiemDanh"] =
                    $"MSSV {mssv} không tồn tại trong hệ thống. Vui lòng liên hệ quản trị viên.";
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                return Page();
            }

            if (KetQua.MSSV == "Chưa xác định")
            {
                TempData["ErrorDiemDanh"] = "Không thể xác định MSSV từ ảnh. Vui lòng chụp lại rõ hơn.";
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

                return Page();
            }

            bool coDangKy = await _context.DangKyHoatDongs.AnyAsync(x => x.MSSV.Trim() == KetQua.MSSV
                   && x.IDHoatDong == IDHoatDong);

            if (!coDangKy) 
            {
                TempData["ErrorDiemDanh"] = "Bạn chưa có đăng ký hoạt động này, vui lòng thử lại sau!";
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

                return Page();
            }

            bool daDiemDanh = await _context.MinhChungs
                .AnyAsync(x => x.MSSV.Trim() == KetQua.MSSV && x.IDHoatDong == IDHoatDong);

            if (daDiemDanh)
            {
                TempData["ErrorDiemDanh"] = "Bạn đã điểm danh hoạt động này rồi.";
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                return Page();
            } 

            KetQua.AnhTheSV = "/imagediemdanh/" + fileName;
            _context.MinhChungs.Add(KetQua);
            await _context.SaveChangesAsync();

            TempData["SuccessDiemDanh"] = "Điểm danh thành công";
            return Page();
        }

    }
}
