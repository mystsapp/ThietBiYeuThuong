﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Migrations
{
    [DbContext(typeof(ThietBiYeuThuongDbContext))]
    partial class ThietBiYeuThuongDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.BenhNhan", b =>
                {
                    b.Property<string>("MaBN")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<int>("CMND_CCCD_BN")
                        .HasColumnType("int");

                    b.Property<string>("DiaChi")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("GT_TN")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("HoTenBN")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("HoTenTN")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<int>("NamSinh")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgaySua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SDT_TN")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("MaBN");

                    b.ToTable("BenhNhans");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.BenhNhanThietBi", b =>
                {
                    b.Property<string>("BenhNhanId")
                        .HasColumnType("varchar(12)");

                    b.Property<string>("ThietBiId")
                        .HasColumnType("varchar(12)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiTao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BenhNhanId", "ThietBiId");

                    b.HasIndex("ThietBiId");

                    b.ToTable("BenhNhanThietBis");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTHoSoBN", b =>
                {
                    b.Property<string>("SoPhieuCT")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("DongHoGiao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DongHoThu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("GhiChu")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("HoSoBNId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("LapPhieu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("NVGiaoBinh")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("NgayNhap")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayXuat")
                        .HasColumnType("datetime");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongHienTai")
                        .HasColumnType("int");

                    b.Property<string>("ThietBiId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.HasKey("SoPhieuCT");

                    b.HasIndex("HoSoBNId");

                    b.HasIndex("ThietBiId");

                    b.ToTable("CTHoSoBNs");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTPhieu", b =>
                {
                    b.Property<string>("SoPhieuCT")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("DongHoGiao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DongHoThu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("GhiChu")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("LapPhieu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("NVGiaoBinh")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("NgayNhap")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayXuat")
                        .HasColumnType("datetime");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongHienTai")
                        .HasColumnType("int");

                    b.Property<string>("SoPhieu")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("ThietBiId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.HasKey("SoPhieuCT");

                    b.HasIndex("ThietBiId");

                    b.ToTable("CTPhieus");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.HoSoBN", b =>
                {
                    b.Property<string>("SoPhieu")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("BenhNhanId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("DonVi")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("HoTenNVYTe")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("LapPhieu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("NVTruc")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("NgayLap")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgaySua")
                        .HasColumnType("datetime");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SDT_NVYT")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.HasKey("SoPhieu");

                    b.HasIndex("BenhNhanId");

                    b.ToTable("HoSoBNs");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.LoaiThietBi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripttion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("NgaySua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("LoaiThietBis");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.PhieuNhap", b =>
                {
                    b.Property<string>("SoPhieu")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("DonVi")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime?>("NgayNhap")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgaySua")
                        .HasColumnType("datetime");

                    b.Property<string>("NguoiNhap")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TrangThaiId")
                        .HasColumnType("int");

                    b.HasKey("SoPhieu");

                    b.ToTable("PhieuNhaps");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.PhieuXuat", b =>
                {
                    b.Property<string>("SoPhieu")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("BenhNhanId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime?>("NgaySua")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgayXuat")
                        .HasColumnType("datetime");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiXuat")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("SoPhieu");

                    b.HasIndex("BenhNhanId");

                    b.ToTable("PhieuXuats");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Trangthai")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.ThietBi", b =>
                {
                    b.Property<string>("MaTB")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<int>("LoaiTBId")
                        .HasColumnType("int");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime>("NgaySua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TenTB")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("TinhTrang")
                        .HasColumnType("bit");

                    b.Property<int>("TrangThaiId")
                        .HasColumnType("int");

                    b.HasKey("MaTB");

                    b.HasIndex("LoaiTBId");

                    b.HasIndex("TrangThaiId");

                    b.ToTable("ThietBis");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.TinhTon", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime?>("NgayCT")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgayTao")
                        .HasColumnType("datetime");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SLNhap_Day")
                        .HasColumnType("int");

                    b.Property<int>("SLNhap_ThuHoi")
                        .HasColumnType("int");

                    b.Property<int>("SLNhap_VuaBomVe")
                        .HasColumnType("int");

                    b.Property<int>("SLXuat_BenhNhan")
                        .HasColumnType("int");

                    b.Property<int>("SLXuat_GoiBom")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongNhap")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongTon")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongXuat")
                        .HasColumnType("int");

                    b.Property<string>("TenTB")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TinhTons");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.TinhTrangBN", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BenhNenBN")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("BenhNhanId")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("ChiSoSPO2")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("KetLuan")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime?>("NgaySua")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("NgayTao")
                        .HasColumnType("datetime");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TinhTrang")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("TinhTrangBNSauO2")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("BenhNhanId");

                    b.ToTable("TinhTrangBNs");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.TrangThai", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("NgaySua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiSua")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NguoiTao")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TrangThais");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Dienthoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hoten")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Macn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Maphong")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Ngaytao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nguoitao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Trangthai")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.BenhNhanThietBi", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.BenhNhan", "BenhNhan")
                        .WithMany()
                        .HasForeignKey("BenhNhanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThietBiYeuThuong.Data.Models.ThietBi", "ThietBi")
                        .WithMany()
                        .HasForeignKey("ThietBiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BenhNhan");

                    b.Navigation("ThietBi");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTHoSoBN", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.HoSoBN", "HoSoBN")
                        .WithMany()
                        .HasForeignKey("HoSoBNId");

                    b.HasOne("ThietBiYeuThuong.Data.Models.ThietBi", "ThietBi")
                        .WithMany()
                        .HasForeignKey("ThietBiId");

                    b.Navigation("HoSoBN");

                    b.Navigation("ThietBi");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTPhieu", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.ThietBi", "ThietBi")
                        .WithMany()
                        .HasForeignKey("ThietBiId");

                    b.Navigation("ThietBi");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.HoSoBN", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.BenhNhan", "BenhNhan")
                        .WithMany()
                        .HasForeignKey("BenhNhanId");

                    b.Navigation("BenhNhan");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.PhieuXuat", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.BenhNhan", "BenhNhan")
                        .WithMany()
                        .HasForeignKey("BenhNhanId");

                    b.Navigation("BenhNhan");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.ThietBi", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.LoaiThietBi", "LoaiThietBi")
                        .WithMany()
                        .HasForeignKey("LoaiTBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThietBiYeuThuong.Data.Models.TrangThai", "TrangThai")
                        .WithMany()
                        .HasForeignKey("TrangThaiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiThietBi");

                    b.Navigation("TrangThai");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.TinhTrangBN", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.BenhNhan", "BenhNhan")
                        .WithMany()
                        .HasForeignKey("BenhNhanId");

                    b.Navigation("BenhNhan");
                });
#pragma warning restore 612, 618
        }
    }
}
