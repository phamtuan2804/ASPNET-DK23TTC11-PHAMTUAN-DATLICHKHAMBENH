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
        public int MaBacSi { get; set; } 
        [ForeignKey("MaBacSi")]
        public virtual BacSi? BacSi { get; set; } 

        [Required]
        public int MaDichVu { get; set; } 
        [ForeignKey("MaDichVu")]
        public virtual DichVu? DichVu { get; set; } 

        [Required]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        public DateTime ThoiGianKetThuc { get; set; }

        public int? SoLuongSlotTrong { get; set; } 

        [StringLength(10)]
        public string? ThuTrongTuan { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }

        public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();
    }
}