using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("Lop")]
    public class Lop
    {
        [Key]
        [StringLength(20)]
        public string IDLop { get; set; }

        [Required]
        [StringLength(50)]
        public string TenLop { get; set; }

        // Khóa ngoại IDKhoa
        [StringLength(50)]
        [ForeignKey("Khoa")]
        public string IDKhoa { get; set; }
        public virtual Khoa Khoa { get; set; }

        // Khóa ngoại IDGiangVien
        [StringLength(20)]
        [ForeignKey("GiangVien")]
        public string IDGiangVien { get; set; }
        public virtual GiangVien GiangVien { get; set; }

        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}