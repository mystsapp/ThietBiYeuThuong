using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class ThietBiYeuThuongDbContext : DbContext
    {
        public ThietBiYeuThuongDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PhieuNX> PhieuNXes { get; set; }
        public DbSet<CTPhieuNX> CTPhieuNXes { get; set; }
        public DbSet<TinhTon> TinhTons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BenhNhan> BenhNhans { get; set; }
        public DbSet<BenhNhanThietBi> BenhNhanThietBis { get; set; }
        public DbSet<LoaiThietBi> LoaiThietBis { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<TinhTrangBN> TinhTrangBNs { get; set; }
        public DbSet<TrangThai> TrangThais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BenhNhanThietBi>().HasKey(x => new { x.BenhNhanId, x.ThietBiId });
        }
    }
}