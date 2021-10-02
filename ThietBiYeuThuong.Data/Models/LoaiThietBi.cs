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
    public class LoaiThietBi
    {
        public int Id { get; set; }

        [DisplayName("Tên loại")]
        [MaxLength(100, ErrorMessage = "Chiều dài tối đa 100 ký tự"), Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [MaxLength(200, ErrorMessage = "Chiều dài tối đa 200 ký tự"), Column(TypeName = "nvarchar(200)")]
        public string Descripttion { get; set; }

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