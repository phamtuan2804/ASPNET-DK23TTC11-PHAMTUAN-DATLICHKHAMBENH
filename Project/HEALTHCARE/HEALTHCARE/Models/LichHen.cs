using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HEALTHCARE.Models
{
    public class LichHen
    {
        [Key]
        public int MaLichHen { get; set; }

        [Required]
        public int MaLichTrinh { get; set; } // Khóa ngoại
        [ForeignKey("MaLichTrinh")]
        public virtual LichTrinh? LichTrinh { get; set; } // Navigation property

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

        [Column(TypeName = "TEXT")]
        public string? GhiChu { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
    }
}