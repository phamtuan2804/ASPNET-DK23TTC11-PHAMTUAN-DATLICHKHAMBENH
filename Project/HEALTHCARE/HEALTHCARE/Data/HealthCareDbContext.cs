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

            modelBuilder.Entity<BacSi>().ToTable("BacSi");
            modelBuilder.Entity<DichVu>().ToTable("DichVu");
            modelBuilder.Entity<LichTrinh>().ToTable("LichTrinh");
            modelBuilder.Entity<LichHen>().ToTable("LichHen");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.Property(d => d.Gia).HasColumnType("decimal(10, 2)");
            });

        }
    }
}