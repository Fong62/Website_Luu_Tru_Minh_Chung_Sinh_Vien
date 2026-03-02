using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("MinhChung")]
    public class MinhChung
    {
        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MSSV { get; set; } // Mã sinh viên (Khóa ngoại từ bảng SinhVien)

        [Key, Column(Order = 2)]
        [StringLength(20)]
        [ForeignKey("HoatDong")]
        public string IDHoatDong { get; set; } // Mã hoạt động (Khóa ngoại từ bảng HoatDong)

        [StringLength(256)]
        public string AnhTheSV { get; set; }

        public DateTime ThoiGianDiemDanh { get; set; }

        [StringLength(20)]
        public string TrangThaiHienThi { get; set; }

        [StringLength(256)]
        public string LyDoThatBai { get; set; }

        public virtual SinhVien SinhVien { get; set; } // Liên kết với bảng SinhVien
        public virtual HoatDong HoatDong { get; set; } // Liên kết với bảng HoatDong
    }
}   