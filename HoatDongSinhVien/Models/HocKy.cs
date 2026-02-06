using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("HocKy")]
    public class HocKy
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "Mã học kỳ")]
        public string IDHocKy { get; set; }

        [StringLength(256)]
        [Display(Name = "Tên học kỳ")]
        public string TenHocKy { get; set; }

        [StringLength(20)]
        [Display(Name = "Năm học")]
        public string NamHoc { get; set; }

        // Quan hệ 1-N: Một học kỳ có nhiều hoạt động
        public virtual ICollection<HoatDong> HoatDongs { get; set; }

        // Quan hệ 1-N: Một học kỳ có nhiều bảng điểm rèn luyện của sinh viên
        public virtual ICollection<KetQuaRenLuyen> KetQuaRenLuyens { get; set; }
    }
}