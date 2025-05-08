using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEALTHCARE.Models
{
    public class DatLichKhamViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        [Display(Name = "Họ và Tên")]
        public string? TenBenhNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Địa chỉ Email")]
        public string? Email { get; set; }

        [Display(Name = "Chọn Bác sĩ")]
        [Required(ErrorMessage = "Vui lòng chọn bác sĩ.")]
        public int MaBacSiChon { get; set; }
        public IEnumerable<SelectListItem>? DanhSachBacSi { get; set; }

        [Display(Name = "Nội dung cần khám (Dịch vụ)")]
        [Required(ErrorMessage = "Vui lòng chọn dịch vụ khám.")]
        public int MaDichVuChon { get; set; }
        public IEnumerable<SelectListItem>? DanhSachDichVu { get; set; }

        [Display(Name = "Ngày khám")]
        [Required(ErrorMessage = "Vui lòng chọn ngày khám.")]
        [DataType(DataType.Date)]
        public DateTime NgayKham { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Khung giờ khám (18h00 - 21h00)")]
        [Required(ErrorMessage = "Vui lòng chọn khung giờ.")]
        public int MaLichTrinhChon { get; set; } 
        public IEnumerable<SelectListItem>? DanhSachKhungGio { get; set; } 

        [Display(Name = "Giá dịch vụ (dự kiến)")]
        [DataType(DataType.Currency)]
        public decimal? GiaDichVuHienThi { get; set; }

        [Display(Name = "Ghi chú thêm (nếu có)")]
        [DataType(DataType.MultilineText)]
        public string? GhiChu { get; set; }

        public DatLichKhamViewModel()
        {
            TenBenhNhan = string.Empty;
            SoDienThoai = string.Empty;
            Email = string.Empty;
            GhiChu = string.Empty;
            DanhSachBacSi = new List<SelectListItem>();
            DanhSachDichVu = new List<SelectListItem>();
            DanhSachKhungGio = new List<SelectListItem>();
        }
    }
}