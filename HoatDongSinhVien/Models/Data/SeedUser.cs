using HoatDongSinhVien.Models.Application_User;
using HoatDongSinhVien.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HoatDongSinhVien.Data
{
    public static class SeedUser
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new HoatDongSinhVienDbcontext(
                serviceProvider.GetRequiredService<DbContextOptions<HoatDongSinhVienDbcontext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoles(roleManager);
            await SeedAdminUser(userManager);
            await SeedGiaoVienUser(context, userManager);
            await SeedSinhVienUsers(context, userManager);
        }

        // ================= ROLE =================
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "HocSinh", "BanCanSu", "GiaoVien" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // ================= ADMIN USER =================
        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            string adminUserName = "admin";
            var user = await userManager.FindByNameAsync(adminUserName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = "admin@hcmue.edu.vn",
                };

                var result = await userManager.CreateAsync(user, "admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        // ================= GIẢNG VIÊN =================
        private static async Task SeedGiaoVienUser(
          HoatDongSinhVienDbcontext context,
          UserManager<ApplicationUser> userManager)
        {
            var giangViens = await context.GiangViens.ToListAsync();

            foreach (var gv in giangViens)
            { 
                var user = await userManager.FindByNameAsync(gv.IDGiangVien);
                if (user != null) continue;

                user = new ApplicationUser
                {
                    UserName = gv.IDGiangVien,
                    Email = gv.Email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Gv@123456");
                if (!result.Succeeded) continue;

                await userManager.AddToRoleAsync(user, "GiaoVien");
            }

            await context.SaveChangesAsync();
        }
        // ================= SINH VIÊN =================
        private static async Task SeedSinhVienUsers(
            HoatDongSinhVienDbcontext context,
            UserManager<ApplicationUser> userManager)
        {
            var sinhViens = await context.SinhViens.ToListAsync();

            foreach (var sv in sinhViens)
            {
                var user = await userManager.FindByNameAsync(sv.MSSV);
                if (user != null) continue;

                user = new ApplicationUser
                {
                    UserName = sv.MSSV,
                    Email = sv.Email,
                };

                var result = await userManager.CreateAsync(user, "Sv@123456");
                if (!result.Succeeded) continue;

                await userManager.AddToRoleAsync(user, "HocSinh");

                if (sv.VaiTro == "BanCanSu")
                {
                    await userManager.AddToRoleAsync(user, "BanCanSu");
                }
            }
        }
    }
}
