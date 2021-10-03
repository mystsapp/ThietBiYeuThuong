using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.ViewModels;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class BenhNhanViewModel
    {
        public BenhNhan BenhNhan { get; set; }
        public IPagedList<BenhNhan> BenhNhans { get; set; }
        public TinhTrangBN TinhTrangBN { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
        public List<ListViewModel> ListGT { get; set; }
    }
}