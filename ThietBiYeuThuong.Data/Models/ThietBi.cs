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
    public class ThietBi
    {
        [Key]
        [DisplayName("Mã TB")]
        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string MaTB { get; set; }

        [DisplayName("Tên TB")]
        [MaxLength(100, ErrorMessage = "Chiều dài tối đa 100 ký tự"), Column(TypeName = "nvarchar(100)")]
        public string TenTB { get; set; }

        [DisplayName("Trạng thái")]
        public int TrangThaiId { get; set; }

        [ForeignKey("TrangThaiId")]
        public TrangThai TrangThai { get; set; }

        [DisplayName("Loại TB")]
        public int LoaiTBId { get; set; }

        [ForeignKey("LoaiTBId")]
        public LoaiThietBi LoaiThietBi { get; set; }

        [DisplayName("Tình trạng")]
        public bool TinhTrang { get; set; } // Đã giao ?

        public DateTime NgayTao { get; set; }

        [DisplayName("Người tạo")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string NguoiTao { get; set; }

        public DateTime NgaySua { get; set; }

        [DisplayName("Người sửa")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "varchar(50)")]
        public string NguoiSua { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string LogFile { get; set; }
    }
}