using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("KetQuaRenLuyen")]
    public class KetQuaRenLuyen
    {
        // Khóa ngoại 1: Trỏ về Sinh Viên
        [StringLength(20)]
        [ForeignKey("SinhVien")]
        public string MSSV { get; set; }

        // Khóa ngoại 2: Trỏ về Học Kỳ
        [StringLength(20)]
        [ForeignKey("HocKy")]
        public string IDHocKy { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Điểm rèn luyện")]
        public decimal DiemRenLuyen { get; set; }

        [StringLength(256)]
        [Display(Name = "Thành tích")]
        public string ThanhTich { get; set; }

        // Navigation properties
        public virtual SinhVien SinhVien { get; set; }
        public virtual HocKy HocKy { get; set; }
    }
}