using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEALTHCARE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimeColumnsToDateTime2Properly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- Bảng BacSi ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "BacSi",
                type: "datetime2", // Kiểu dữ liệu mới
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime"); 

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "BacSi",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            // --- Bảng DichVu ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "DichVu",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "DichVu",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            // --- Bảng LichTrinh ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianBatDau",
                table: "LichTrinh",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianKetThuc",
                table: "LichTrinh",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "LichTrinh",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "LichTrinh",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            // --- Bảng LichHen ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianDatHen",
                table: "LichHen",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "LichHen",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "LichHen",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            // --- Bảng NguoiDung ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "NguoiDung",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "NguoiDung",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Phương thức Down sẽ hoàn tác các thay đổi, chuyển từ datetime2 về datetime
            // --- Bảng BacSi ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "BacSi",
                type: "datetime", // Quay lại kiểu datetime
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "BacSi",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // --- Bảng DichVu ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "DichVu",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "DichVu",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // --- Bảng LichTrinh ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianBatDau",
                table: "LichTrinh",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianKetThuc",
                table: "LichTrinh",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "LichTrinh",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "LichTrinh",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // --- Bảng LichHen ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianDatHen",
                table: "LichHen",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "LichHen",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "LichHen",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // --- Bảng NguoiDung ---
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianTao",
                table: "NguoiDung",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianCapNhat",
                table: "NguoiDung",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
