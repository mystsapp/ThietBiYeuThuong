using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class LoaiThietBiViewModel
    {
        public LoaiThietBi LoaiThietBi { get; set; }
        public IPagedList<LoaiThietBi> LoaiThietBis { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}