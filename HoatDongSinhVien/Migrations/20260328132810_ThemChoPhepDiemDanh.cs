using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoatDongSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class ThemChoPhepDiemDanh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChoPhepDiemDanh",
                table: "HoatDong",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoPhepDiemDanh",
                table: "HoatDong");
        }
    }
}
