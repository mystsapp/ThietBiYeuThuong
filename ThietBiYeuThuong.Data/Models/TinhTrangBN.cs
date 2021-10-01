using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class TinhTrangBN
    {
        public long Id { get; set; }

        [DisplayName("Tình trạng B.N")]
        [MaxLength(200, ErrorMessage = "Chiều dài tối đa 200 ký tự"), Column(TypeName = "nvarchar(200)")]
        public string TinhTrang { get; set; }

        [DisplayName("Bệnh nền B.N")]
        [MaxLength(200, ErrorMessage = "Chiều dài tối đa 200 ký tự"), Column(TypeName = "nvarchar(200)")]
        public string BenhNenBN { get; set; }

        [DisplayName("Chỉ số SPO2 B.N")]
        [MaxLength(50, ErrorMessage = "Chiều dài tối đa 50 ký tự"), Column(TypeName = "nvarchar(50)")]
        public string ChiSoSPO2 { get; set; }

        [DisplayName("Tình trạng BN sau khi thở OXY")]
        [MaxLength(250, ErrorMessage = "Chiều dài tối đa 250 ký tự"), Column(TypeName = "nvarchar(250)")]
        public string TinhTrangBNSauO2 { get; set; }

        [DisplayName("Kết luận")]
        [MaxLength(250, ErrorMessage = "Chiều dài tối đa 250 ký tự"), Column(TypeName = "nvarchar(250)")]
        public string KetLuan { get; set; }
    }
}