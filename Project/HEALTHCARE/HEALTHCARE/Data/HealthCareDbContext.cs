using Microsoft.EntityFrameworkCore;
using HEALTHCARE.Models;

namespace HEALTHCARE.Data
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
        {
        }

        public DbSet<BacSi> BacSis { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<LichTrinh> LichTrinhs { get; set; }
        public DbSet<LichHen> LichHens { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình tên bảng
            modelBuilder.Entity<BacSi>().ToTable("BacSi");
            modelBuilder.Entity<DichVu>().ToTable("DichVu");
            modelBuilder.Entity<LichTrinh>().ToTable("LichTrinh");
            modelBuilder.Entity<LichHen>().ToTable("LichHen");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");

            // Cấu hình cụ thể cho DichVu
            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.Property(d => d.Gia).HasColumnType("decimal(10, 2)");
                // QUAN TRỌNG: Cần thêm lại cấu hình .HasColumnType("datetime2") cho các cột DateTime ở đây
                // và cho tất cả các entity khác để nhất quán với migrations đã thực hiện.
                // Ví dụ: entity.Property(e => e.ThoiGianTao).HasColumnType("datetime2");
            });

            // QUAN TRỌNG: Thêm cấu hình .HasColumnType("datetime2") cho các cột DateTime của các entity khác (BacSi, LichTrinh, LichHen, NguoiDung)
            // Ví dụ cho LichTrinh:
            /*
            modelBuilder.Entity<LichTrinh>(entity =>
            {
                entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime2");
                entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime2");
                entity.Property(e => e.ThoiGianTao).HasColumnType("datetime2");
                entity.Property(e => e.ThoiGianCapNhat).HasColumnType("datetime2");
            });
            */

            // Cấu hình mối quan hệ (nếu cần thiết để ghi đè quy ước)
            /*
            modelBuilder.Entity<LichTrinh>(entity =>
            {
                entity.HasOne(lt => lt.BacSi)
                    .WithMany(bs => bs.LichTrinhs)
                    .HasForeignKey(lt => lt.MaBacSi)
                    .OnDelete(DeleteBehavior.Cascade); // Hoặc hành vi xóa khác

                entity.HasOne(lt => lt.DichVu)
                    .WithMany(dv => dv.LichTrinhs)
                    .HasForeignKey(lt => lt.MaDichVu)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(lt => lt.LichHens)
                    .WithOne(lh => lh.LichTrinh)
                    .HasForeignKey(lh => lh.MaLichTrinh)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            */
        }
    }
}
