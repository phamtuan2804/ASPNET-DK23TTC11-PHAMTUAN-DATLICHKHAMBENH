using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HEALTHCARE.Models
{
    public class LichTrinh
    {
        [Key]
        public int MaLichTrinh { get; set; }

        [Required]
        public int MaBacSi { get; set; } // Khóa ngoại
        [ForeignKey("MaBacSi")]
        public virtual BacSi? BacSi { get; set; } // Navigation property

        [Required]
        public int MaDichVu { get; set; } // Khóa ngoại
        [ForeignKey("MaDichVu")]
        public virtual DichVu? DichVu { get; set; } // Navigation property

        [Required]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        public DateTime ThoiGianKetThuc { get; set; }

        public int? SoLuongSlotTrong { get; set; } // Nullable int

        [StringLength(10)]
        public string? ThuTrongTuan { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }

        // Navigation property
        public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();
    }
}