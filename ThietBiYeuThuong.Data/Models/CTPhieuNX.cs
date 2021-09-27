using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThietBiYeuThuong.Data.Models
{
    public class CTPhieuNX
    {
        [Key]
        [DisplayName("Số phiếu CT")]
        [MaxLength(10, ErrorMessage = "Chiều dài tối đa 10 ký tự"), Column(TypeName = "varchar(10)")]
        public string SoPhieuCT { get; set; }

        [DisplayName("PhieuNX")]
        [Required(ErrorMessage = "Trường này không được để trống")]
        public string PhieuNXId { get; set; }

        [ForeignKey("PhieuNXId")]
        public virtual PhieuNX PhieuNX { get; set; }

        [DisplayName("Thiết bị")]
        [MaxLength(100, ErrorMessage = "Chiều dài tối đa 100 ký tự"), Column(TypeName = "varchar(100)")]
        public string ThietBi { get; set; }

        [DisplayName("Người lập phiếu")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string LapPhieu { get; set; }

        [DisplayName("Ngày nhập")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayNhap { get; set; }

        [DisplayName("Ngày xuất")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayXuat { get; set; }

        [DisplayName("Đ.hồ")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string DongHoGiao { get; set; }

        [DisplayName("Đ.hồ")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string DongHoThu { get; set; }

        [DisplayName("NV giao bình")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string NVGiaoBinh { get; set; }

        [DisplayName("NV giao bình")]
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