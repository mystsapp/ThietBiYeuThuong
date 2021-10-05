using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.ViewModels;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class ThietBiViewModel
    {
        public ThietBi ThietBi { get; set; }
        public IPagedList<ThietBi> ThietBis { get; set; }
        public IEnumerable<ThietBi> IEnumThietBi { get; set; }
        public TrangThai TrangThai { get; set; }
        public string StrUrl { get; set; }
        public string MaTBText { get; set; }
        public int Page { get; set; }
    }
}