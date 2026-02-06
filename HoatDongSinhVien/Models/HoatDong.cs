using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("HoatDong")]
    public class HoatDong
    {
        [Key]
        [StringLength(20)]
        public string IDHoatDong { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên hoạt động")]
        public string TenHoatDong { get; set; }

        [Display(Name = "Ngày tổ chức")]
        public DateTime NgayToChuc { get; set; } // Timestamp map sang DateTime

        [StringLength(256)]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [StringLength(1024)]
        public string QRCodeDangKy { get; set; }

        [StringLength(1024)]
        public string QRCodeDiemDanh { get; set; }

        [StringLength(20)]
        public string NguoiTao { get; set; }

        [StringLength(20)]
        public string NguoiDuyet { get; set; }

        [StringLength(20)]
        public string TrangThaiDuyet { get; set; }

        [StringLength(20)]
        public string TrangThaiHienThi { get; set; }

        // Khóa ngoại IDLinhVuc
        [StringLength(20)]
        [ForeignKey("LinhVuc")]
        [Display(Name = "Lĩnh vực")]
        public string IDLinhVuc { get; set; }
        public virtual LinhVuc LinhVuc { get; set; }

        [StringLength(20)]
        [ForeignKey("HocKy")]
        [Display(Name = "Học kỳ")]
        public string IDHocKy { get; set; }
        public virtual HocKy HocKy { get; set; }

        public virtual ICollection<DangKyHoatDong> DangKyHoatDongs { get; set; }
        public virtual ICollection<MinhChung> MinhChungs { get; set; }
    }
}