using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class TinhTonViewModel
    {
        public TinhTon TinhTon { get; set; }
        public List<CTPhieuNX> CTPhieuNXes { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }

        public int TonDau { get; set; }
        public int TonCuoi { get; set; }
        public int CongPhatSinhNhap { get; set; }
        public int CongPhatSinhXuat { get; set; }
    }
}