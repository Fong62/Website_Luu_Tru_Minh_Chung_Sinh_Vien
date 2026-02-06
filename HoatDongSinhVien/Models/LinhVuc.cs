using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("LinhVuc")]
    public class LinhVuc
    {
        [Key]
        [StringLength(20)]
        public string IDLinhVuc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLinhVuc { get; set; }

        public double ThangDiem { get; set; } // Kiểu float trong DB map sang double/float

        public virtual ICollection<HoatDong> HoatDongs { get; set; }
    }
}