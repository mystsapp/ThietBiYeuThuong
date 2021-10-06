using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}