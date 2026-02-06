using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("DangKyHoatDong")]
    public class DangKyHoatDong
    {
        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MSSV { get; set; } 

        [Key, Column(Order = 2)]
        [StringLength(20)]
        [ForeignKey("HoatDong")]
        public string IDHoatDong { get; set; } 

        public DateTime ThoiGianDangKy { get; set; } 

        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(50)]
        public string TenKhoa { get; set; } 
        public virtual SinhVien SinhVien { get; set; } 
        public virtual HoatDong HoatDong { get; set; } 
    }
}
