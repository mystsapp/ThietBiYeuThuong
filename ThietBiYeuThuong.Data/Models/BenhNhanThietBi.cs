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
    public class BenhNhanThietBi
    {
        public string BenhNhanId { get; set; }

        public BenhNhan BenhNhan { get; set; }

        public string ThietBiId { get; set; }

        public ThietBi ThietBi { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }

        [MaxLength(12, ErrorMessage = "Chiều dài tối đa 12 ký tự"), Column(TypeName = "varchar(12)")]
        public string CTHoSoBNId { get; set; }
    }
}