using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HEALTHCARE.Models
{
    public class BacSi
    {
        [Key]
        public int MaBacSi { get; set; }

        [Required(ErrorMessage = "Tên bác sĩ là bắt buộc.")]
        [StringLength(255)]
        public string TenBacSi { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ChuyenKhoa { get; set; }

        [Column(TypeName = "TEXT")]
        public string? MoTa { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }

        // Navigation property
        public virtual ICollection<LichTrinh> LichTrinhs { get; set; } = new List<LichTrinh>();
    }
}