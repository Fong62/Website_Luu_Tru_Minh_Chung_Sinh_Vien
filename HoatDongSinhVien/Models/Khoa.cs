using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("Khoa")]
    public class Khoa
    {
        [Key]
        [StringLength(50)]
        public string IDKhoa { get; set; }

        [Required]
        [StringLength(50)]
        public string TenKhoa { get; set; }

        // Quan hệ 1-n: Một khoa có nhiều lớp
        public virtual ICollection<Lop> Lops { get; set; }
    }
}
