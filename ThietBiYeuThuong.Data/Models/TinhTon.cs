using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class TinhTon
    {
        public long Id { get; set; }

        [DisplayName("Ngày CT")]
        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Ngày không được để trống")]
        public DateTime? NgayCT { get; set; }

        [DisplayName("Tồn")]
        public int SoLuongTon { get; set; }

        [DisplayName("Tên TB")]
        [MaxLength(100, ErrorMessage = "Chiều dài tối đa 100 ký tự"), Column(TypeName = "varchar(100)")]
        public string TenTB { get; set; }

        [DisplayName("Người tạo")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "nvarchar(50)")]
        public string NguoiTao { get; set; }

        //[DisplayName("Ngày khoá")]
        [Column(TypeName = "datetime")]
        public DateTime? NgayTao { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string LogFile { get; set; }
    }
}