using HoatDongSinhVien.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoatDongSinhVien.Models.Data;

namespace HoatDongSinhVien.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new HoatDongSinhVienDbcontext(
                serviceProvider.GetRequiredService<DbContextOptions<HoatDongSinhVienDbcontext>>());

            await SeedKhoa(context);
            await SeedGiangVien(context);
            await SeedLop(context);
            await SeedSinhVien(context);
            await SeedLinhVuc(context);
            await SeedHocKy(context);
        }

        // ================= KHOA =================
        private static async Task SeedKhoa(HoatDongSinhVienDbcontext context)
        {
            var faculties = new List<Khoa>
            {
                new Khoa { IDKhoa = "CNTT", TenKhoa = "Công nghệ thông tin" },
                new Khoa { IDKhoa = "VLY", TenKhoa = "Vật lý" },
                new Khoa { IDKhoa = "HOC", TenKhoa = "Hóa học" },
                new Khoa { IDKhoa = "SNH", TenKhoa = "Sinh học" },
                new Khoa { IDKhoa = "NNV", TenKhoa = "Ngữ văn" },
                new Khoa { IDKhoa = "TTT", TenKhoa = "Toán - Tin học" },
                new Khoa { IDKhoa = "LDH", TenKhoa = "Lịch sử" },
                new Khoa { IDKhoa = "DLY", TenKhoa = "Địa lý" },
                new Khoa { IDKhoa = "TA", TenKhoa = "Tiếng Anh" },
                new Khoa { IDKhoa = "TF", TenKhoa = "Tiếng Pháp" },
                new Khoa { IDKhoa = "TN", TenKhoa = "Tiếng Nga" },
                new Khoa { IDKhoa = "TCN", TenKhoa = "Tiếng Trung" },
                new Khoa { IDKhoa = "TNH", TenKhoa = "Tiếng Nhật" },
                new Khoa { IDKhoa = "TKQ", TenKhoa = "Tiếng Hàn Quốc" },
                new Khoa { IDKhoa = "GDCT", TenKhoa = "Giáo dục chính trị" },
                new Khoa { IDKhoa = "TTH", TenKhoa = "Tâm lý học" },
                new Khoa { IDKhoa = "KHGD", TenKhoa = "Khoa học Giáo dục" },
                new Khoa { IDKhoa = "GDTX", TenKhoa = "Giáo dục Tiểu học" },
                new Khoa { IDKhoa = "GDMN", TenKhoa = "Giáo dục Mầm non" },
                new Khoa { IDKhoa = "GDQH", TenKhoa = "Giáo dục Quốc phòng" },
                new Khoa { IDKhoa = "GDDB", TenKhoa = "Giáo dục Đặc biệt" },
                new Khoa { IDKhoa = "GDTC", TenKhoa = "Giáo dục Thể chất" },
                new Khoa { IDKhoa = "TNC", TenKhoa = "Tổ Nữ công" }
            };

            foreach (var khoa in faculties)
            {
                if (!await context.Khoas.AnyAsync(k => k.IDKhoa == khoa.IDKhoa))
                {
                    context.Khoas.Add(khoa);
                }
            }

            await context.SaveChangesAsync();
        }


        // ================= GIẢNG VIÊN =================
        private static async Task SeedGiangVien(HoatDongSinhVienDbcontext context)
        {
            var giangViens = new List<GiangVien>
            {
                new GiangVien { IDGiangVien = "GV_A", HoTen = "Giảng viên A", Email = "gva@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "GV_B", HoTen = "Giảng viên B", Email = "gvb@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "GV_C", HoTen = "Giảng viên C", Email = "gvc@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "GV_D", HoTen = "Giảng viên D", Email = "gvd@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "GV_E", HoTen = "Giảng viên E", Email = "gve@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "GV_F", HoTen = "Giảng viên F", Email = "gvf@hcmue.edu.vn" },
                new GiangVien { IDGiangVien = "LMT", HoTen = "Lê Minh Triết", Email= "lmt@hcmue.edu.vn"}
            };

            foreach (var gv in giangViens)
            {
                if (!await context.GiangViens.AnyAsync(x => x.IDGiangVien == gv.IDGiangVien))
                {
                    context.GiangViens.Add(gv);
                }
            }

            await context.SaveChangesAsync();
        }

        // ================= LỚP =================
        private static async Task SeedLop(HoatDongSinhVienDbcontext context)
        {
            if (!await context.Lops.AnyAsync(l => l.IDLop == "48.01.CNTT.B"))
            {
                context.Lops.Add(new Lop
                {
                    IDLop = "48.01.CNTT.B",
                    TenLop = "Công nghệ thông tin B",
                    IDKhoa = "CNTT",
                    IDGiangVien = "LMT"
                });

                await context.SaveChangesAsync();
            }
        }

        // ================= SINH VIÊN =================
        private static async Task SeedSinhVien(HoatDongSinhVienDbcontext context)
        {
            var students = new List<SinhVien>
            {
                new SinhVien
                {
                    MSSV = "48.01.104.059",
                    HoTen = "Nguyễn Trần Gia Huy",
                    Email = "4801104059@hcmue.edu.vn",
                    VaiTro = "BanCanSu",
                    IDLop = "48.01.CNTT.B"
                },
                new SinhVien
                {
                    MSSV = "48.01.104.106",
                    HoTen = "Nguyễn Hoàng Phong",
                    Email = "4801104106@hcmue.edu.vn",
                    VaiTro = "SinhVien",
                    IDLop = "48.01.CNTT.B"
                },
                new SinhVien
                {
                    MSSV = "48.01.104.005",
                    HoTen = "Quách Tuấn Anh",
                    Email = "4801104005@hcmue.edu.vn",
                    VaiTro = "SinhVien",
                    IDLop = "48.01.CNTT.B"
                },
                new SinhVien
                {
                    MSSV = "48.01.104.201",
                    HoTen = "Sinh viên A",
                    Email = "4801104201@hcmue.edu.vn",
                    VaiTro = "SinhVien",
                    IDLop = "48.01.CNTT.B"
                },
                new SinhVien
                {
                    MSSV = "48.01.104.202",
                    HoTen = "Sinh viên B",
                    Email = "4801104202@hcmue.edu.vn",
                    VaiTro = "SinhVien",
                    IDLop = "48.01.CNTT.B"
                },
                new SinhVien
                {
                    MSSV = "48.01.104.203",
                    HoTen = "Sinh viên C",
                    Email = "4801104203@hcmue.edu.vn",
                    VaiTro = "BanCanSu",
                    IDLop = "48.01.CNTT.B"
                }
            };

            foreach (var sv in students)
            {
                if (!await context.SinhViens.AnyAsync(x => x.MSSV == sv.MSSV))
                {
                    context.SinhViens.Add(sv);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedLinhVuc(HoatDongSinhVienDbcontext context)
        {
            var linhVucList = new List<LinhVuc>
            {
                new LinhVuc
                {
                    IDLinhVuc = "XaHoi",
                    TenLinhVuc = "Các hoạt động xã hội",
                    ThangDiem = 3.0
                },
                new LinhVuc
                {
                    IDLinhVuc = "CoVu",
                    TenLinhVuc = "Các hoạt động cổ vũ các kỳ thi, cuộc thi học thuật",
                    ThangDiem = 2.0
                },
                new LinhVuc
                {
                    IDLinhVuc = "HocThuat",
                    TenLinhVuc = "Các hoạt động học thuật: Hội thảo, tọa đàm, lớp hướng dẫn NCKH, hoạt động khảo sát của Trường",
                    ThangDiem = 3.0
                },
                new LinhVuc
                {
                    IDLinhVuc = "ChinhTri",
                    TenLinhVuc = "Đại hội, Hội nghị, tọa đàm về tình hình thời sự, chính trị",
                    ThangDiem = 4.0
                }
            };

            foreach (var linhVuc in linhVucList)
            {
                if (!await context.LinhVucs.AnyAsync(l => l.IDLinhVuc == linhVuc.IDLinhVuc))
                {
                    context.LinhVucs.Add(linhVuc);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedHocKy(HoatDongSinhVienDbcontext context)
        {
            var hocKyList = new List<HocKy>();

            // Chạy loop từ năm 2025 đến năm 2030
            // Năm 2030 sẽ sinh ra kỳ học cho năm 2030-2031
            for (int year = 2025; year <= 2030; year++)
            {
                string namHoc = $"{year}-{year + 1}";

                // Tạo Học kỳ 1
                hocKyList.Add(new HocKy
                {
                    IDHocKy = $"HK_{year}_{year + 1}_1",
                    TenHocKy = $"Học kỳ 1 ({namHoc})",
                    NamHoc = namHoc
                });

                // Tạo Học kỳ 2
                hocKyList.Add(new HocKy
                {
                    IDHocKy = $"HK_{year}_{year + 1}_2",
                    TenHocKy = $"Học kỳ 2 ({namHoc})",
                    NamHoc = namHoc
                });
            }

            foreach (var hk in hocKyList)
            {
                // Kiểm tra xem IDHocKy đã tồn tại chưa trước khi thêm
                if (!await context.HocKys.AnyAsync(h => h.IDHocKy == hk.IDHocKy))
                {
                    context.HocKys.Add(hk);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
