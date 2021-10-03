using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Web.Models
{
    public class CTPhieuViewModel
    {
        public PhieuNhap PhieuNhap { get; set; }
        public CTPhieu CTPhieu { get; set; }
        public IEnumerable<CTPhieu> CTPhieus { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}