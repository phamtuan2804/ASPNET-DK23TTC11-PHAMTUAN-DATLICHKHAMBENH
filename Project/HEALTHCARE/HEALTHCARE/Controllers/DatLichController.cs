using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HEALTHCARE.Models;
using HEALTHCARE.Data;

public class DatLichController : Controller
{
    private readonly HealthCareDbContext _context;
    private readonly IConfiguration _configuration;

    public DatLichController(HealthCareDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult TaoMoi() // GET: Trang chủ (hoặc trang khởi tạo đặt lịch)
    {
        ViewData["Title"] = "Trang Chủ - Tập Đoàn Y Khoa HealthCare";
        return View(); // Trả về View mặc định (có thể là Index.cshtml)
    }

    [HttpGet]
    public async Task<IActionResult> DatLichNgay() // GET: Hiển thị form đặt lịch chi tiết
    {
        ViewData["Title"] = "Đặt Lịch Khám Bệnh";
        var viewModel = new DatLichKhamViewModel();

        // Lấy danh sách bác sĩ và dịch vụ cho dropdowns
        viewModel.DanhSachBacSi = await _context.BacSis
            .OrderBy(bs => bs.TenBacSi)
            .Select(bs => new SelectListItem { Value = bs.MaBacSi.ToString(), Text = bs.TenBacSi })
            .ToListAsync();

        viewModel.DanhSachDichVu = await _context.DichVus
            .OrderBy(dv => dv.TenDichVu)
            .Select(dv => new SelectListItem { Value = dv.MaDichVu.ToString(), Text = dv.TenDichVu })
            .ToListAsync();

        viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Vui lòng chọn ngày, bác sĩ, dịch vụ --" } };

        return View("DatLichNgay", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TaoMoi(DatLichKhamViewModel viewModel) // POST: Xử lý submit form đặt lịch
    {
        // Tải lại dữ liệu cho dropdowns nếu ModelState không hợp lệ
        if (viewModel.DanhSachBacSi == null || !viewModel.DanhSachBacSi.Any())
        {
            viewModel.DanhSachBacSi = await _context.BacSis.OrderBy(bs => bs.TenBacSi).Select(bs => new SelectListItem { Value = bs.MaBacSi.ToString(), Text = bs.TenBacSi }).ToListAsync();
        }
        if (viewModel.DanhSachDichVu == null || !viewModel.DanhSachDichVu.Any())
        {
            viewModel.DanhSachDichVu = await _context.DichVus.OrderBy(dv => dv.TenDichVu).Select(dv => new SelectListItem { Value = dv.MaDichVu.ToString(), Text = dv.TenDichVu }).ToListAsync();
        }
        if (viewModel.DanhSachKhungGio == null)
        {
            viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Vui lòng chọn ngày, bác sĩ, dịch vụ --" } };
        }

        // Validate ngày khám
        if (viewModel.NgayKham.Date < DateTime.Today)
        {
            ModelState.AddModelError("NgayKham", "Ngày khám không được chọn trong quá khứ.");
        }

        LichTrinh? lichTrinhChon = null;
        int actualMaLichTrinh = 0;

        if (!string.IsNullOrEmpty(viewModel.MaLichTrinhChon))
        {
            if (viewModel.MaLichTrinhChon.StartsWith("SUGGEST_")) // Xử lý slot đề xuất
            {
                string suggestedTimeStr = viewModel.MaLichTrinhChon.Split('_')[1];
                if (TimeSpan.TryParse(suggestedTimeStr, out TimeSpan suggestedTime))
                {
                    DateTime thoiGianBatDauDeXuat = viewModel.NgayKham.Date + suggestedTime;
                    DateTime thoiGianKetThucDeXuat = thoiGianBatDauDeXuat.AddMinutes(30);

                    lichTrinhChon = await _context.LichTrinhs
                        .FirstOrDefaultAsync(lt => lt.MaBacSi == viewModel.MaBacSiChon &&
                                               lt.MaDichVu == viewModel.MaDichVuChon &&
                                               lt.ThoiGianBatDau == thoiGianBatDauDeXuat);

                    if (lichTrinhChon == null) // Tạo LichTrinh mới nếu slot đề xuất chưa có
                    {
                        lichTrinhChon = new LichTrinh
                        {
                            MaBacSi = viewModel.MaBacSiChon,
                            MaDichVu = viewModel.MaDichVuChon,
                            ThoiGianBatDau = thoiGianBatDauDeXuat,
                            ThoiGianKetThuc = thoiGianKetThucDeXuat,
                            SoLuongSlotTrong = 1,
                            ThuTrongTuan = ConvertDayOfWeekToDbFormat(viewModel.NgayKham.DayOfWeek),
                            ThoiGianTao = DateTime.Now,
                            ThoiGianCapNhat = DateTime.Now
                        };
                        _context.LichTrinhs.Add(lichTrinhChon);
                        await _context.SaveChangesAsync(); // Lưu để lấy MaLichTrinh
                        actualMaLichTrinh = lichTrinhChon.MaLichTrinh;
                    }
                    else if (lichTrinhChon.SoLuongSlotTrong.HasValue && lichTrinhChon.SoLuongSlotTrong.Value <= 0)
                    {
                        ModelState.AddModelError("MaLichTrinhChon", "Khung giờ đề xuất này vừa có người đặt. Vui lòng thử lại.");
                    }
                    else
                    {
                        actualMaLichTrinh = lichTrinhChon.MaLichTrinh;
                    }
                }
                else
                {
                    ModelState.AddModelError("MaLichTrinhChon", "Khung giờ đề xuất không hợp lệ.");
                }
            }
            else if (int.TryParse(viewModel.MaLichTrinhChon, out int parsedMaLichTrinh)) // Xử lý MaLichTrinh bình thường
            {
                actualMaLichTrinh = parsedMaLichTrinh;
                lichTrinhChon = await _context.LichTrinhs
                                        .FirstOrDefaultAsync(lt => lt.MaLichTrinh == actualMaLichTrinh);
                if (lichTrinhChon == null)
                {
                    ModelState.AddModelError("MaLichTrinhChon", "Khung giờ đã chọn không hợp lệ. Vui lòng tải lại và chọn lại.");
                }
                else if (lichTrinhChon.SoLuongSlotTrong.HasValue && lichTrinhChon.SoLuongSlotTrong.Value <= 0)
                {
                    ModelState.AddModelError("MaLichTrinhChon", "Khung giờ này đã hết chỗ. Vui lòng chọn khung giờ khác.");
                }
            }
            else
            {
                ModelState.AddModelError("MaLichTrinhChon", "Lựa chọn khung giờ không hợp lệ.");
            }
        }

        if (ModelState.IsValid)
        {
            var lichHen = new LichHen // Tạo đối tượng LichHen mới
            {
                MaLichTrinh = actualMaLichTrinh,
                TenBenhNhan = viewModel.TenBenhNhan ?? string.Empty,
                SoDienThoaiBenhNhan = viewModel.SoDienThoai ?? string.Empty,
                EmailBenhNhan = viewModel.Email,
                ThoiGianDatHen = DateTime.Now,
                TrangThai = "MoiDat",
                GhiChu = viewModel.GhiChu,
                ThoiGianTao = DateTime.Now,
                ThoiGianCapNhat = DateTime.Now
            };

            _context.LichHens.Add(lichHen);

            if (lichTrinhChon != null && lichTrinhChon.SoLuongSlotTrong.HasValue && lichTrinhChon.SoLuongSlotTrong > 0)
            {
                lichTrinhChon.SoLuongSlotTrong -= 1; // Giảm số slot trống
                lichTrinhChon.ThoiGianCapNhat = DateTime.Now;
                _context.LichTrinhs.Update(lichTrinhChon);
            }

            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            TempData["SuccessMessage"] = $"Đặt lịch thành công! Mã lịch hẹn của bạn là #{lichHen.MaLichHen}.";
            return RedirectToAction("XacNhanDatLich", new { id = lichHen.MaLichHen });
        }

        // Nếu ModelState không hợp lệ, tải lại DanhSachKhungGio cho View
        if (viewModel.MaBacSiChon > 0 && viewModel.MaDichVuChon > 0 && viewModel.NgayKham != default(DateTime))
        {
            var slotsResult = await GetAvailableSlots(
                                    viewModel.MaBacSiChon,
                                    viewModel.MaDichVuChon,
                                    viewModel.NgayKham.ToString("yyyy-MM-dd")
                                ) as JsonResult;
            if (slotsResult?.Value is IEnumerable<SelectListItem> slots && slots.Any())
            {
                viewModel.DanhSachKhungGio = slots;
            }
            else
            {
                var suggestedSlots = new List<SelectListItem>();
                suggestedSlots.Add(new SelectListItem { Value = "SUGGEST_18:00", Text = "18:00 - 18:30 (Đề xuất)" });
                viewModel.DanhSachKhungGio = suggestedSlots;
            }
        }
        else if (viewModel.DanhSachKhungGio == null || !viewModel.DanhSachKhungGio.Any())
        {
            viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Vui lòng chọn ngày, bác sĩ, dịch vụ --" } };
        }

        return View("DatLichNgay", viewModel);
    }

    public async Task<IActionResult> XacNhanDatLich(int? id) // GET: Hiển thị trang xác nhận đặt lịch
    {
        if (id == null)
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View("XacNhanDatLich");
            }
            return NotFound();
        }

        var lichHen = await _context.LichHens // Lấy thông tin lịch hẹn chi tiết
                                .Include(lh => lh.LichTrinh)
                                    .ThenInclude(lt => lt.BacSi)
                                .Include(lh => lh.LichTrinh)
                                    .ThenInclude(lt => lt.DichVu)
                                .FirstOrDefaultAsync(lh => lh.MaLichHen == id);

        if (lichHen == null) { return NotFound(); }

        ViewBag.SuccessMessage = TempData["SuccessMessage"] ?? "Chi tiết lịch hẹn của bạn:";

        return View(lichHen);
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots(int maBacSi, int maDichVu, string ngayKham) // API: Lấy khung giờ trống
    {
        if (maBacSi <= 0 || maDichVu <= 0 || string.IsNullOrWhiteSpace(ngayKham))
        {
            return Json(new List<SelectListItem>());
        }

        if (!DateTime.TryParse(ngayKham, out DateTime selectedDate) || selectedDate.Date < DateTime.Today)
        {
            return Json(new List<SelectListItem> { new SelectListItem { Value = "", Text = "Ngày khám không hợp lệ" } });
        }

        DayOfWeek dayOfWeekSelected = selectedDate.DayOfWeek;
        string thuTrongTuanDbFormat = ConvertDayOfWeekToDbFormat(dayOfWeekSelected);

        var slots = await _context.LichTrinhs
            .Where(lt => lt.MaBacSi == maBacSi &&
                         lt.MaDichVu == maDichVu &&
                         lt.ThoiGianBatDau.Date == selectedDate.Date &&
                         lt.ThoiGianBatDau.Hour >= 18 && lt.ThoiGianBatDau.Hour < 21 && // Điều kiện giờ làm việc (ví dụ)
                         lt.SoLuongSlotTrong.HasValue && lt.SoLuongSlotTrong > 0 &&
                         lt.ThuTrongTuan == thuTrongTuanDbFormat)
            .OrderBy(lt => lt.ThoiGianBatDau)
            .Select(lt => new SelectListItem
            {
                Value = lt.MaLichTrinh.ToString(),
                Text = $"{lt.ThoiGianBatDau:HH:mm} - {lt.ThoiGianKetThuc:HH:mm}"
            }).ToListAsync();

        return Json(slots ?? new List<SelectListItem>());
    }

    [HttpGet]
    public async Task<IActionResult> GetServicePrice(int maDichVu) // API: Lấy giá dịch vụ
    {
        if (maDichVu <= 0) { return Json(null); }
        var dichVu = await _context.DichVus.FindAsync(maDichVu);
        if (dichVu != null && dichVu.Gia.HasValue)
        {
            return Json(new { gia = dichVu.Gia.Value, tenDichVu = dichVu.TenDichVu });
        }
        return Json(null);
    }

    private string ConvertDayOfWeekToDbFormat(DayOfWeek dayOfWeek) // Chuyển đổi DayOfWeek sang chuỗi
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday: return "Thứ Hai";
            case DayOfWeek.Tuesday: return "Thứ Ba";
            case DayOfWeek.Wednesday: return "Thứ Tư";
            case DayOfWeek.Thursday: return "Thứ Năm";
            case DayOfWeek.Friday: return "Thứ Sáu";
            case DayOfWeek.Saturday: return "Thứ Bảy";
            case DayOfWeek.Sunday: return "Chủ Nhật";
            default: return string.Empty;
        }
    }

    public IActionResult KiemTraKetNoiTrucTiep() // Action: Kiểm tra kết nối cơ sở dữ liệu
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        string message;
        if (string.IsNullOrEmpty(connectionString)) { message = "LỖI: Không tìm thấy chuỗi kết nối 'DefaultConnection'."; }
        else
        {
            try
            {
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    message = $"THÀNH CÔNG! Kết nối đến SQL Server: {connection.DataSource}, Database: {connection.Database}";
                    connection.Close();
                }
            }
            catch (Exception ex) { message = $"LỖI KẾT NỐI: {ex.Message}"; }
        }
        ViewBag.Message = message;
        return View("ThongBaoKetNoi");
    }
}

public class HomeController : Controller
{
    public IActionResult GioiThieu()
    {
        ViewData["Title"] = "Giới Thiệu - Tập Đoàn Y Khoa HealthCare";
        return View();
    }
}
