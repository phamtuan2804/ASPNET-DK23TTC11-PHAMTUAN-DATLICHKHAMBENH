using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEALTHCARE.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // QUAN TRỌNG: Phương thức Up() được để trống (các lệnh tạo schema đã được comment out).
            // Migration này dùng để thiết lập "baseline" cho cơ sở dữ liệu đã tồn tại.
            // Sẽ không thay đổi schema hiện tại.
            // Khi chạy "Update-Database InitialDatabaseSetup", EF Core chỉ ghi nhận migration này
            // vào bảng __EFMigrationsHistory.

            /*
            migrationBuilder.CreateTable(
                name: "BacSi",
                columns: table => new
                {
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBacSi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChuyenKhoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "TEXT", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacSi", x => x.MaBacSi);
                });

            migrationBuilder.CreateTable(
                name: "DichVu",
                columns: table => new
                {
                    MaDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDichVu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(type: "TEXT", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVu", x => x.MaDichVu);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "LichTrinh",
                columns: table => new
                {
                    MaLichTrinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBacSi = table.Column<int>(type: "int", nullable: false),
                    MaDichVu = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongSlotTrong = table.Column<int>(type: "int", nullable: true),
                    ThuTrongTuan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichTrinh", x => x.MaLichTrinh);
                    table.ForeignKey(
                        name: "FK_LichTrinh_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichTrinh_DichVu_MaDichVu",
                        column: x => x.MaDichVu,
                        principalTable: "DichVu",
                        principalColumn: "MaDichVu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichHen",
                columns: table => new
                {
                    MaLichHen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLichTrinh = table.Column<int>(type: "int", nullable: false),
                    TenBenhNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoDienThoaiBenhNhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailBenhNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThoiGianDatHen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "TEXT", nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichHen", x => x.MaLichHen);
                    table.ForeignKey(
                        name: "FK_LichHen_LichTrinh_MaLichTrinh",
                        column: x => x.MaLichTrinh,
                        principalTable: "LichTrinh",
                        principalColumn: "MaLichTrinh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichHen_MaLichTrinh",
                table: "LichHen",
                column: "MaLichTrinh");

            migrationBuilder.CreateIndex(
                name: "IX_LichTrinh_MaBacSi",
                table: "LichTrinh",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_LichTrinh_MaDichVu",
                table: "LichTrinh",
                column: "MaDichVu");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Phương thức Down() chứa các lệnh để hoàn tác (drop các bảng).
            // Cần thiết nếu rollback migration này.
            migrationBuilder.DropTable(
                name: "LichHen");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "LichTrinh");

            migrationBuilder.DropTable(
                name: "BacSi");

            migrationBuilder.DropTable(
                name: "DichVu");
        }
    }
}
