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
        [MaxLength(10, ErrorMessage = "Chiều dài tối đa 10 ký tự"), Column(TypeName = "varchar(10)")]
        public string MaTB { get; set; }

        [DisplayName("Tên TB")]
        [MaxLength(100, ErrorMessage = "Chiều dài tối đa 100 ký tự"), Column(TypeName = "varchar(100)")]
        public string TenTB { get; set; }

        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
    }
}