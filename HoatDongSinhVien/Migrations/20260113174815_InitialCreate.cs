using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoatDongSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiangVien",
                columns: table => new
                {
                    IDGiangVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangVien", x => x.IDGiangVien);
                });

            migrationBuilder.CreateTable(
                name: "HocKy",
                columns: table => new
                {
                    IDHocKy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenHocKy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NamHoc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocKy", x => x.IDHocKy);
                });

            migrationBuilder.CreateTable(
                name: "Khoa",
                columns: table => new
                {
                    IDKhoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenKhoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoa", x => x.IDKhoa);
                });

            migrationBuilder.CreateTable(
                name: "LinhVuc",
                columns: table => new
                {
                    IDLinhVuc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenLinhVuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThangDiem = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhVuc", x => x.IDLinhVuc);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lop",
                columns: table => new
                {
                    IDLop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenLop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IDKhoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IDGiangVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lop", x => x.IDLop);
                    table.ForeignKey(
                        name: "FK_Lop_GiangVien_IDGiangVien",
                        column: x => x.IDGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "IDGiangVien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lop_Khoa_IDKhoa",
                        column: x => x.IDKhoa,
                        principalTable: "Khoa",
                        principalColumn: "IDKhoa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoatDong",
                columns: table => new
                {
                    IDHoatDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenHoatDong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayToChuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    QRCodeDangKy = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    QRCodeDiemDanh = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NguoiDuyet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThaiDuyet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThaiHienThi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDLinhVuc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDHocKy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoatDong", x => x.IDHoatDong);
                    table.ForeignKey(
                        name: "FK_HoatDong_HocKy_IDHocKy",
                        column: x => x.IDHocKy,
                        principalTable: "HocKy",
                        principalColumn: "IDHocKy",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoatDong_LinhVuc_IDLinhVuc",
                        column: x => x.IDLinhVuc,
                        principalTable: "LinhVuc",
                        principalColumn: "IDLinhVuc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    MSSV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IDLop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVien", x => x.MSSV);
                    table.ForeignKey(
                        name: "FK_SinhVien_Lop_IDLop",
                        column: x => x.IDLop,
                        principalTable: "Lop",
                        principalColumn: "IDLop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DangKyHoatDong",
                columns: table => new
                {
                    MSSV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDHoatDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThoiGianDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenKhoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyHoatDong", x => new { x.MSSV, x.IDHoatDong });
                    table.ForeignKey(
                        name: "FK_DangKyHoatDong_HoatDong_IDHoatDong",
                        column: x => x.IDHoatDong,
                        principalTable: "HoatDong",
                        principalColumn: "IDHoatDong",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DangKyHoatDong_SinhVien_MSSV",
                        column: x => x.MSSV,
                        principalTable: "SinhVien",
                        principalColumn: "MSSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaRenLuyen",
                columns: table => new
                {
                    MSSV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDHocKy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiemRenLuyen = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ThanhTich = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaRenLuyen", x => new { x.MSSV, x.IDHocKy });
                    table.ForeignKey(
                        name: "FK_KetQuaRenLuyen_HocKy_IDHocKy",
                        column: x => x.IDHocKy,
                        principalTable: "HocKy",
                        principalColumn: "IDHocKy",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KetQuaRenLuyen_SinhVien_MSSV",
                        column: x => x.MSSV,
                        principalTable: "SinhVien",
                        principalColumn: "MSSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MinhChung",
                columns: table => new
                {
                    MSSV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDHoatDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AnhTheSV = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ThoiGianDiemDanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiHienThi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LyDoThatBai = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinhChung", x => new { x.MSSV, x.IDHoatDong });
                    table.ForeignKey(
                        name: "FK_MinhChung_HoatDong_IDHoatDong",
                        column: x => x.IDHoatDong,
                        principalTable: "HoatDong",
                        principalColumn: "IDHoatDong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MinhChung_SinhVien_MSSV",
                        column: x => x.MSSV,
                        principalTable: "SinhVien",
                        principalColumn: "MSSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyHoatDong_IDHoatDong",
                table: "DangKyHoatDong",
                column: "IDHoatDong");

            migrationBuilder.CreateIndex(
                name: "IX_HoatDong_IDHocKy",
                table: "HoatDong",
                column: "IDHocKy");

            migrationBuilder.CreateIndex(
                name: "IX_HoatDong_IDLinhVuc",
                table: "HoatDong",
                column: "IDLinhVuc");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaRenLuyen_IDHocKy",
                table: "KetQuaRenLuyen",
                column: "IDHocKy");

            migrationBuilder.CreateIndex(
                name: "IX_Lop_IDGiangVien",
                table: "Lop",
                column: "IDGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_Lop_IDKhoa",
                table: "Lop",
                column: "IDKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_MinhChung_IDHoatDong",
                table: "MinhChung",
                column: "IDHoatDong");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_IDLop",
                table: "SinhVien",
                column: "IDLop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DangKyHoatDong");

            migrationBuilder.DropTable(
                name: "KetQuaRenLuyen");

            migrationBuilder.DropTable(
                name: "MinhChung");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "HoatDong");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "HocKy");

            migrationBuilder.DropTable(
                name: "LinhVuc");

            migrationBuilder.DropTable(
                name: "Lop");

            migrationBuilder.DropTable(
                name: "GiangVien");

            migrationBuilder.DropTable(
                name: "Khoa");
        }
    }
}
