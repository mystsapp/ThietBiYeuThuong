using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class PhieuNhapViewModel
    {
        public PhieuNhap PhieuNhap { get; set; }
        public IPagedList<PhieuNhap> PhieuNhaps { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}