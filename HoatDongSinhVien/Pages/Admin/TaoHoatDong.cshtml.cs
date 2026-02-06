using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.Services_Interfaces;
using HoatDongSinhVien.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.Collections.Generic;
using System.Linq;

namespace HoatDongSinhVien.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class TaoHoatDongModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;
        private readonly ITaoUpdateGGForm _taoUpdateGGForm;
        public TaoHoatDongModel(HoatDongSinhVienDbcontext context, ITaoUpdateGGForm taoUpdateGGForm)
        {
            _context = context;
            _taoUpdateGGForm = taoUpdateGGForm;
        }

        [BindProperty]
        public HoatDong HoatDong { get; set; }
        public string QRCodeDangKyUrl { get; set; }
        public string QRCodeDiemDanhUrl { get; set; }
        public List<LinhVuc> LinhVucs { get; set; }
        public List<HocKy> HocKys { get; set; }

        public void OnGet()
        {
            LinhVucs = _context.LinhVucs.ToList();
            HocKys = _context.HocKys.ToList();

            if (LinhVucs == null || !LinhVucs.Any())
            {
                ModelState.AddModelError("", "Chưa có lĩnh vực nào trong hệ thống.");
            }

            if (HocKys == null || !HocKys.Any())
            {
                ModelState.AddModelError("", "Chưa có học kỳ nào trong hệ thống.");
            }

        }

        public async Task<IActionResult> OnPost()
        {
            LinhVucs = _context.LinhVucs.ToList();
            HocKys = _context.HocKys.ToList();

            if (string.IsNullOrEmpty(HoatDong.IDLinhVuc))
            {
                ModelState.AddModelError("", "Lĩnh vực không thể để trống.");
                return Page();
            }

            if (string.IsNullOrEmpty(HoatDong.IDHocKy))
            {
                ModelState.AddModelError("", "Học kỳ không thể để trống.");
                return Page();
            }

            //HoatDong.IDHoatDong = GenerateCode.TaoMaMoi(_context, _context.HoatDongs, "HD", "IDHoatDong");
            HoatDong.IDHoatDong = GenerateCode.TaoMaMoi2("HD");

            string googleFormUrl = await _taoUpdateGGForm.CreateGoogleFormAsync(HoatDong.TenHoatDong, HoatDong.MoTa);

            if (googleFormUrl.Contains("Có lỗi xảy ra")) // Kiểm tra xem có phải lỗi không
            {
                ModelState.AddModelError("", googleFormUrl);  // Thông báo lỗi nếu có
                return Page();
            }

            // Tạo mã QR cho đăng ký và điểm danh
            var qrDangKyUrl = GenerateQRCodeDangKy(googleFormUrl);
            var qrDiemDanhUrl = GenerateQRCodeDiemDanh($"https://attendance.com/{HoatDong.IDHoatDong}");

            HoatDong.QRCodeDangKy = qrDangKyUrl;
            HoatDong.QRCodeDiemDanh = qrDiemDanhUrl;
            //HoatDong.NgayToChuc = DateTime.Now; 
            HoatDong.NguoiTao = "Admin"; 
            HoatDong.TrangThaiDuyet = "Đã duyệt"; 
            HoatDong.TrangThaiHienThi = "Hiển thị"; 
            HoatDong.NguoiDuyet = "HCMUE";

            _context.HoatDongs.Add(HoatDong);
            _context.SaveChanges();

            return RedirectToPage( "/Student/XemHoatDong", new { IDHoatDong = HoatDong.IDHoatDong });
        }

        private string GenerateQRCodeDangKy(string url)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);

                // Lưu QR code vào thư mục wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes", $"{HoatDong.IDHoatDong}DangKy.png");
                qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                return $"/qrcodes/{HoatDong.IDHoatDong}DangKy.png"; 
            }
        }

        private string GenerateQRCodeDiemDanh(string url)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);

                // Lưu QR code vào thư mục wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes", $"{HoatDong.IDHoatDong}DiemDanh.png");
                qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                return $"/qrcodes/{HoatDong.IDHoatDong}DiemDanh.png"; 
            }
        }
    }
}
