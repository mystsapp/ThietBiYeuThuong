using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Models
{
    public class BenhNhanThietBi
    {
        public int BenhNhanId { get; set; }
        public BenhNhan BenhNhan { get; set; }

        public int ThietBiId { get; set; }
        public ThietBi ThietBi { get; set; }
    }
}