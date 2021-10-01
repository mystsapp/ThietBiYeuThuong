using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class BenhNhan
    {
        public int Id { get; set; }

        [DisplayName("Họ tên T.N")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string HoTenTN { get; set; }

        [DisplayName("SĐT")]
        [MaxLength(20, ErrorMessage = "Chiều dài tối đa 20 ký tự"), Column(TypeName = "varchar(20)")]
        public string SDT_TN { get; set; }

        [DisplayName("GT_TN")]
        [MaxLength(20, ErrorMessage = "Chiều dài tối đa 20 ký tự"), Column(TypeName = "nvarchar(20)")]
        public string GT_TN { get; set; }

        [DisplayName("Họ tên B.N")]
        [MaxLength(150, ErrorMessage = "Chiều dài tối đa 150 ký tự"), Column(TypeName = "nvarchar(150)")]
        public string HoTenBN { get; set; }

        [DisplayName("Năm sinh")]
        public int NamSinh { get; set; }

        [DisplayName("CMND/CCCD")]
        public int CMND_CCCD_BN { get; set; }

        [DisplayName("Địa chỉ")]
        [MaxLength(250, ErrorMessage = "Chiều dài tối đa 250 ký tự"), Column(TypeName = "nvarchar(250)")]
        public string DiaChi { get; set; }
    }
}