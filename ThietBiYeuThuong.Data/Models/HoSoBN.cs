﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class HoSoBN
    {
        [Key]
        [DisplayName("Số phiếu")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string SoPhieu { get; set; }

        [DisplayName("Bệnh nhân")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string BenhNhanId { get; set; }

        [ForeignKey("BenhNhanId")]
        public BenhNhan BenhNhan { get; set; }

        [DisplayName("Người lập phiếu")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string LapPhieu { get; set; }

        [DisplayName("Ngày lập")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayLap { get; set; }

        [DisplayName("Người sửa")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string NguoiSua { get; set; }

        [DisplayName("Ngày sửa")]
        [Column(TypeName = "datetime")]
        public DateTime? NgaySua { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string LogFile { get; set; }

        /// <summary>
        /// ////////////////////
        /// </summary>
        ///

        //[DisplayName("Họ tên NV Y Tế")]
        //[MaxLength(250, ErrorMessage = "Chiều dài tối đa 250 ký tự"), Column(TypeName = "nvarchar(250)")]
        //public string HoTenNVYTe { get; set; }

        //[DisplayName("SĐT")]
        //[MaxLength(20, ErrorMessage = "Chiều dài tối đa 20 ký tự"), Column(TypeName = "varchar(20)")]
        //public string SDT_NVYT { get; set; }

        //[DisplayName("Đơn vị")]
        //[MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        //public string DonVi { get; set; }

        [DisplayName("Mã NVYT")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string MaNVYT { get; set; }

        [ForeignKey("MaNVYT")]
        public NhanVienYTe NhanVienYTe { get; set; }

        public int STT { get; set; }

        [DisplayName("NV trực")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string NVTruc { get; set; }
    }
}