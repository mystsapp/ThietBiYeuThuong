﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThietBiYeuThuong.Data.Models
{
    public class CTHoSoBN
    {
        [Key]
        [DisplayName("Số phiếu CT")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string SoPhieuCT { get; set; }

        [DisplayName("Số phiếu")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string HoSoBNId { get; set; }

        [ForeignKey("HoSoBNId")]
        public virtual HoSoBN HoSoBN { get; set; }

        //[DisplayName("Tên TB")]
        //[MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        //public string ThietBiId { get; set; }

        //[ForeignKey("ThietBiId")]
        //public ThietBi ThietBi { get; set; }

        [DisplayName("Thiết bị")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "nvarchar(50)")]
        public string ThietBi { get; set; }

        [DisplayName("Người lập phiếu")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string LapPhieu { get; set; }

        [DisplayName("Ngày nhập")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayNhap { get; set; }

        public DateTime? NgayTao { get; set; }

        [DisplayName("Ngày xuất")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayXuat { get; set; }

        [DisplayName("Đ.hồ giao")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string DongHoGiao { get; set; }

        [DisplayName("Đ.hồ thu")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string DongHoThu { get; set; }

        [DisplayName("NV giao bình")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string NVGiaoBinh { get; set; }

        [DisplayName("Ghi chú")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string GhiChu { get; set; }

        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }

        [DisplayName("Số lượng HT")]
        public int SoLuongHienTai { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string LogFile { get; set; }
    }
}