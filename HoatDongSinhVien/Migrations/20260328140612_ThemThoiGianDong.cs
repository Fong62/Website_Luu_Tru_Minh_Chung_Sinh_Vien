using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoatDongSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class ThemThoiGianDong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianDongDiemDanh",
                table: "HoatDong",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiGianDongDiemDanh",
                table: "HoatDong");
        }
    }
}
