using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HEALTHCARE.Models
{
    public class DichVu
    {
        [Key]
        public int MaDichVu { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc.")]
        [StringLength(255)]
        public string TenDichVu { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? MoTa { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Gia { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }

        public virtual ICollection<LichTrinh> LichTrinhs { get; set; } = new List<LichTrinh>();
    }
}