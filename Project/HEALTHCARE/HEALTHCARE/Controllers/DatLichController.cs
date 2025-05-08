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
    public async Task<IActionResult> TaoMoi()
    {
        var viewModel = new DatLichKhamViewModel();

        viewModel.DanhSachBacSi = await _context.BacSis
            .OrderBy(bs => bs.TenBacSi)
            .Select(bs => new SelectListItem { Value = bs.MaBacSi.ToString(), Text = bs.TenBacSi })
            .ToListAsync();

        viewModel.DanhSachDichVu = await _context.DichVus
            .OrderBy(dv => dv.TenDichVu)
            .Select(dv => new SelectListItem { Value = dv.MaDichVu.ToString(), Text = dv.TenDichVu })
            .ToListAsync();

        viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Vui lòng chọn ngày, bác sĩ, dịch vụ --" } };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TaoMoi(DatLichKhamViewModel viewModel)
    {
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

        if (ModelState.IsValid)
        {
            var lichTrinhChon = await _context.LichTrinhs
                                            .FirstOrDefaultAsync(lt => lt.MaLichTrinh == viewModel.MaLichTrinhChon);

            if (lichTrinhChon == null)
            {
                ModelState.AddModelError("MaLichTrinhChon", "Khung giờ đã chọn không hợp lệ. Vui lòng tải lại và chọn lại.");
            }
            else if (lichTrinhChon.SoLuongSlotTrong.HasValue && lichTrinhChon.SoLuongSlotTrong.Value <= 0)
            {
                ModelState.AddModelError("MaLichTrinhChon", "Khung giờ này đã hết chỗ. Vui lòng chọn khung giờ khác.");
            }

            if (viewModel.NgayKham.Date < DateTime.Today)
            {
                ModelState.AddModelError("NgayKham", "Ngày khám không được chọn trong quá khứ.");
            }

            if (ModelState.IsValid)
            {
                var lichHen = new LichHen
                {
                    MaLichTrinh = viewModel.MaLichTrinhChon,
                    TenBenhNhan = viewModel.TenBenhNhan ?? string.Empty,
                    SoDienThoaiBenhNhan = viewModel.SoDienThoai ?? string.Empty,
                    EmailBenhNhan = viewModel.Email,
                    ThoiGianDatHen = DateTime.Now,
                    TrangThai = "MoiDat",
                    GhiChu = viewModel.GhiChu,
                };

                _context.LichHens.Add(lichHen);

                if (lichTrinhChon.SoLuongSlotTrong.HasValue)
                {
                    lichTrinhChon.SoLuongSlotTrong -= 1;
                    _context.LichTrinhs.Update(lichTrinhChon);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đặt lịch thành công!";
                return RedirectToAction("XacNhanDatLich", new { id = lichHen.MaLichHen });
            }
        }

        if (viewModel.MaBacSiChon > 0 && viewModel.MaDichVuChon > 0 && viewModel.NgayKham != default(DateTime))
        {
            var slotsResult = await GetAvailableSlots(
                                       viewModel.MaBacSiChon,
                                       viewModel.MaDichVuChon,
                                       viewModel.NgayKham.ToString("yyyy-MM-dd")
                                   ) as JsonResult;
            if (slotsResult?.Value is IEnumerable<SelectListItem> slots)
            {
                viewModel.DanhSachKhungGio = slots;
            }
            else
            {
                viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Không có khung giờ trống --" } };
            }
        }
        else if (viewModel.DanhSachKhungGio == null || !viewModel.DanhSachKhungGio.Any())
        {
            viewModel.DanhSachKhungGio = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Vui lòng chọn ngày, bác sĩ, dịch vụ --" } };
        }

        return View(viewModel);
    }


    public async Task<IActionResult> XacNhanDatLich(int? id)
    {
        if (id == null)
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View();
            }
            return NotFound();
        }

        var lichHen = await _context.LichHens
                                .Include(lh => lh.LichTrinh)
                                    .ThenInclude(lt => lt.BacSi)
                                .Include(lh => lh.LichTrinh)
                                    .ThenInclude(lt => lt.DichVu)
                                .FirstOrDefaultAsync(lh => lh.MaLichHen == id);

        if (lichHen == null) { return NotFound(); }

        ViewBag.SuccessMessage = TempData["SuccessMessage"] ?? "Chi tiết lịch hẹn:";

        return View(lichHen);
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots(int maBacSi, int maDichVu, string ngayKham)
    {
        if (maBacSi <= 0 || maDichVu <= 0 || string.IsNullOrWhiteSpace(ngayKham))
        {
            return Json(new List<SelectListItem>());
        }

        if (!DateTime.TryParse(ngayKham, out DateTime selectedDate) || selectedDate.Date < DateTime.Today)
        {
            return Json(new List<SelectListItem>());
        }

        DayOfWeek dayOfWeekSelected = selectedDate.DayOfWeek;
        string thuTrongTuanDbFormat = ConvertDayOfWeekToDbFormat(dayOfWeekSelected);

        var slots = await _context.LichTrinhs
            .Where(lt => lt.MaBacSi == maBacSi &&
                         lt.MaDichVu == maDichVu &&
                         lt.ThoiGianBatDau.Date == selectedDate.Date &&
                         lt.ThoiGianBatDau.Hour >= 18 && lt.ThoiGianBatDau.Hour < 21 &&
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
    public async Task<IActionResult> GetServicePrice(int maDichVu)
    {
        if (maDichVu <= 0) { return Json(null); }
        var dichVu = await _context.DichVus.FindAsync(maDichVu);
        if (dichVu != null && dichVu.Gia.HasValue)
        {
            return Json(new { gia = dichVu.Gia.Value, tenDichVu = dichVu.TenDichVu });
        }
        return Json(null);
    }

    private string ConvertDayOfWeekToDbFormat(DayOfWeek dayOfWeek)
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

    public IActionResult KiemTraKetNoiTrucTiep()
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
