using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoatDongSinhVien.Models
{
    [Table("SinhVien")]
    public class SinhVien
    {
        [Key]
        [StringLength(20)]
        public string MSSV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(50)]
        public string VaiTro { get; set; } // 'BanCanSu', 'SinhVien'

    
        [StringLength(20)]
        [ForeignKey("Lop")]
        public string IDLop { get; set; }
        public virtual Lop Lop { get; set; }

        public virtual ICollection<DangKyHoatDong> DangKyHoatDongs { get; set; }
        public virtual ICollection<MinhChung> MinhChungs { get; set; }

        public virtual ICollection<KetQuaRenLuyen> KetQuaRenLuyens { get; set; }
    }
}