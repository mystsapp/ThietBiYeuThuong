﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Migrations
{
    [DbContext(typeof(ThietBiYeuThuongDbContext))]
    [Migration("20210928084440_fixTonTbl")]
    partial class fixTonTbl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTPhieuNX", b =>
                {
                    b.Property<string>("SoPhieuCT")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

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

                    b.Property<DateTime?>("NgayXuat")
                        .HasColumnType("datetime");

                    b.Property<string>("PhieuNXId")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<int>("SoLuongHienTai")
                        .HasColumnType("int");

                    b.Property<string>("ThietBi")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("SoPhieuCT");

                    b.HasIndex("PhieuNXId");

                    b.ToTable("CTPhieuNXes");
                });

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.PhieuNX", b =>
                {
                    b.Property<string>("SoPhieu")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("BenhNenBN")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CMND_CCCD_BN")
                        .HasColumnType("int");

                    b.Property<string>("ChiSoSPO2")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DiaChi")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("DonVi")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("GT_TN")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("HoTenBN")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("HoTenNVYTe")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("HoTenTN")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("KetLuan")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("LapPhieu")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LoaiPhieu")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("LogFile")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("NVTruc")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("NamSinh")
                        .HasColumnType("int");

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

                    b.Property<string>("SDT_TN")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.Property<string>("TinhTrangBN")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("TinhTrangBNSauO2")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("SoPhieu");

                    b.ToTable("PhieuNXes");
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

            modelBuilder.Entity("ThietBiYeuThuong.Data.Models.CTPhieuNX", b =>
                {
                    b.HasOne("ThietBiYeuThuong.Data.Models.PhieuNX", "PhieuNX")
                        .WithMany()
                        .HasForeignKey("PhieuNXId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhieuNX");
                });
#pragma warning restore 612, 618
        }
    }
}