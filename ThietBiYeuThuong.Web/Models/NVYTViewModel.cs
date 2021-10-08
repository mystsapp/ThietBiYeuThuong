using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.ViewModels;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class NVYTViewModel
    {
        public NhanVienYTe NhanVienYTe { get; set; }
        public IPagedList<NhanVienYTe> NhanVienYTes { get; set; }
        public IEnumerable<NhanVienYTe> IEnumNVYT { get; set; }
        public string StrUrl { get; set; }
        public string MaNVYTText { get; set; }
        public int Page { get; set; }
    }
}