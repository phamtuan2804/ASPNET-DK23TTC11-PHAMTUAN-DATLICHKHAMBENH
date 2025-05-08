// File: Models/LichHen.cs (Phiên bản đúng cho logic chọn khung giờ)
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HEALTHCARE.Models
{
    public class LichHen
    {
        [Key]
        public int MaLichHen { get; set; }

        // --- KHÔI PHỤC LẠI KHÓA NGOẠI ĐẾN LichTrinh ---
        [Required]
        public int MaLichTrinh { get; set; } // Khóa ngoại đến LichTrinh
        [ForeignKey("MaLichTrinh")]
        public virtual LichTrinh? LichTrinh { get; set; } // Navigation property đến LichTrinh
        // ---------------------------------------------

        // --- XÓA CÁC TRƯỜNG ĐÃ THÊM NHẦM Ở BƯỚC TRƯỚC (NẾU CÓ) ---
        // public int MaBacSi { get; set; } 
        // public virtual BacSi? BacSi { get; set; } 
        // public int MaDichVu { get; set; } 
        // public virtual DichVu? DichVu { get; set; } 
        // public DateTime ThoiGianKhamDuKien { get; set; }
        // public int SoThuTuTrongNgay { get; set; }
        // --------------------------------------------------------

        // Các trường thông tin bệnh nhân và khác giữ nguyên
        [Required(ErrorMessage = "Tên bệnh nhân là bắt buộc.")]
        [StringLength(255)]
        public string TenBenhNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại bệnh nhân là bắt buộc.")]
        [StringLength(20)]
        public string SoDienThoaiBenhNhan { get; set; } = string.Empty;

        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string? EmailBenhNhan { get; set; }

        [Required]
        public DateTime ThoiGianDatHen { get; set; }

        [StringLength(50)]
        public string? TrangThai { get; set; }

        [Column(TypeName = "TEXT")] // Hoặc NVARCHAR(MAX)
        public string? GhiChu { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
    }
}