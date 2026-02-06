using HoatDongSinhVien.Models.Application_User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HoatDongSinhVien.Models.Data
{
    public class HoatDongSinhVienDbcontext: IdentityDbContext<ApplicationUser>
    {
        public HoatDongSinhVienDbcontext(DbContextOptions<HoatDongSinhVienDbcontext> options) : base(options) { }

        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<HoatDong> HoatDongs { get; set; }
        public DbSet<Khoa> Khoas { get; set; }
        public DbSet<LinhVuc> LinhVucs { get; set; }
        public DbSet<Lop> Lops { get; set; }
        public DbSet<MinhChung> MinhChungs { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DangKyHoatDong> DangKyHoatDongs { get; set; }
        public DbSet<HocKy> HocKys { get; set; }
        public DbSet<KetQuaRenLuyen> KetQuaRenLuyens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DangKyHoatDong>()
                .HasKey(d => new { d.MSSV, d.IDHoatDong });

            modelBuilder.Entity<MinhChung>()
                .HasKey(d => new { d.MSSV, d.IDHoatDong });

            modelBuilder.Entity<KetQuaRenLuyen>()
                .HasKey(k => new { k.MSSV, k.IDHocKy });

            // Cấu hình các quan hệ và ràng buộc
            // Quan hệ GiangVien - Lop (1-n)
            modelBuilder.Entity<GiangVien>()
                .HasMany(g => g.Lops)
                .WithOne(l => l.GiangVien)
                .HasForeignKey(l => l.IDGiangVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Khoa - Lop (1-n)
            modelBuilder.Entity<Khoa>()
                .HasMany(k => k.Lops)
                .WithOne(l => l.Khoa)
                .HasForeignKey(l => l.IDKhoa)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Lop - SinhVien (1-n)
            modelBuilder.Entity<Lop>()
                .HasMany(l => l.SinhViens)
                .WithOne(s => s.Lop)
                .HasForeignKey(s => s.IDLop)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ LinhVuc - HoatDong (1-n)
            modelBuilder.Entity<LinhVuc>()
                .HasMany(lv => lv.HoatDongs)
                .WithOne(hd => hd.LinhVuc)
                .HasForeignKey(hd => hd.IDLinhVuc)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ HoatDong - MinhChung (1-n)
            modelBuilder.Entity<HoatDong>()
                .HasMany(hd => hd.MinhChungs)
                .WithOne(mc => mc.HoatDong)
                .HasForeignKey(mc => mc.IDHoatDong)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ SinhVien - MinhChung (1-n)
            modelBuilder.Entity<SinhVien>()
                .HasMany(s => s.MinhChungs)
                .WithOne(mc => mc.SinhVien)
                .HasForeignKey(mc => mc.MSSV)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ SinhVien - DangKyHoatDong (1-n)
            modelBuilder.Entity<SinhVien>()
                .HasMany(s => s.DangKyHoatDongs)
                .WithOne(d => d.SinhVien)
                .HasForeignKey(d => d.MSSV)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ HoatDong - DangKyHoatDong (1-n)
            modelBuilder.Entity<HoatDong>()
                .HasMany(hd => hd.DangKyHoatDongs)
                .WithOne(d => d.HoatDong)
                .HasForeignKey(d => d.IDHoatDong)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HocKy>()
                .HasMany(hk => hk.HoatDongs)
                .WithOne(hd => hd.HocKy)
                .HasForeignKey(hd => hd.IDHocKy)
                .OnDelete(DeleteBehavior.Restrict);

            // --- MỚI THÊM: Quan hệ SinhVien - KetQuaRenLuyen (1-n) ---
            modelBuilder.Entity<SinhVien>()
                .HasMany(sv => sv.KetQuaRenLuyens)
                .WithOne(kq => kq.SinhVien)
                .HasForeignKey(kq => kq.MSSV)
                .OnDelete(DeleteBehavior.Restrict);

            // --- MỚI THÊM: Quan hệ HocKy - KetQuaRenLuyen (1-n) ---
            modelBuilder.Entity<HocKy>()
                .HasMany(hk => hk.KetQuaRenLuyens)
                .WithOne(kq => kq.HocKy)
                .HasForeignKey(kq => kq.IDHocKy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
