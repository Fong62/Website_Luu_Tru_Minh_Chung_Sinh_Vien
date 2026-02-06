using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("GiangVien")]
    public class GiangVien
    {
        [Key]
        [StringLength(20)]
        public string IDGiangVien { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(256)]
        public string Email { get; set; }


        //Quan hệ 1-n: GV cố vấn nhiều lớp
        public virtual ICollection<Lop> Lops { get; set; }
    }
}
