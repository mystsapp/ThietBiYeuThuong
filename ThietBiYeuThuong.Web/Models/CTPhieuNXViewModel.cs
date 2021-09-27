using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Web.Models
{
    public class CTPhieuNXViewModel
    {
        public PhieuNX PhieuNX { get; set; }
        public CTPhieuNX CTPhieuNX { get; set; }
        public IEnumerable<CTPhieuNX> CTPhieuNXes { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}