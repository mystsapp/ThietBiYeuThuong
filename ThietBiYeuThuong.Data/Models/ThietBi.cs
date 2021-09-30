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

        [DisplayName("Trạng thái")]
        public int TrangThaiId { get; set; }

        [DisplayName("Loại TB")]
        public int LoaiTBId { get; set; }

        [ForeignKey("LoaiTBId")]
        public LoaiThietBi LoaiThietBi { get; set; }
    }
}