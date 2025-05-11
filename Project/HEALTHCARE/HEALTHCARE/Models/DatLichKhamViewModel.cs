using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HEALTHCARE.Models
{
    public class DatLichKhamViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn.")]
        [Display(Name = "Tên của bạn")]
        public string TenBenhNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bác sĩ.")]
        [Display(Name = "Chọn bác sĩ")]
        public int MaBacSiChon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn dịch vụ.")]
        [Display(Name = "Chọn dịch vụ khám")]
        public int MaDichVuChon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày khám.")]
        [Display(Name = "Ngày khám")]
        [DataType(DataType.Date)]
        public DateTime NgayKham { get; set; } = DateTime.Today.AddDays(1); // Giá trị mặc định là ngày mai

        [Required(ErrorMessage = "Vui lòng chọn khung giờ khám.")]
        [Display(Name = "Khung giờ khám (18h00 - 21h00)")]
        public string MaLichTrinhChon { get; set; }

        [Display(Name = "Ghi chú thêm")]
        public string? GhiChu { get; set; } // Cho phép null nếu không bắt buộc

        // Danh sách cho các dropdown, khởi tạo để tránh null
        public IEnumerable<SelectListItem> DanhSachBacSi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> DanhSachDichVu { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> DanhSachKhungGio { get; set; } = new List<SelectListItem>();
    }
}
