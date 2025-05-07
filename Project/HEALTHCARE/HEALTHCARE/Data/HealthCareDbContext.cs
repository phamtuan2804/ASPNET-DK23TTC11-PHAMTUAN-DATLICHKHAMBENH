using Microsoft.EntityFrameworkCore;
using HEALTHCARE.Models; // Namespace cho Entity Models

namespace HEALTHCARE.Data // Namespace cho DbContext
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

            // Ánh xạ Entity tới tên bảng cụ thể.
            modelBuilder.Entity<BacSi>().ToTable("BacSi");
            modelBuilder.Entity<DichVu>().ToTable("DichVu");
            modelBuilder.Entity<LichTrinh>().ToTable("LichTrinh");
            modelBuilder.Entity<LichHen>().ToTable("LichHen");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");

            // Cấu hình thuộc tính cụ thể (ví dụ).
            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.Property(d => d.Gia).HasColumnType("decimal(10, 2)");
            });

            // Thêm các cấu hình Fluent API khác tại đây nếu cần.
        }
    }
}