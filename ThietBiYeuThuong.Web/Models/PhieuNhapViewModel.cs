using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class PhieuNhapViewModel
    {
        public PhieuNhap PhieuNhap { get; set; }
        public CTPhieu CTPhieu { get; set; }
        public ThietBi ThietBi { get; set; }
        public List<LoaiThietBi> LoaiThietBis { get; set; }
        public List<ThietBi> ThietBis { get; set; }
        public List<TrangThai> TrangThais { get; set; }
        public IPagedList<PhieuNhap> PhieuNhaps { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }

        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
    }
}