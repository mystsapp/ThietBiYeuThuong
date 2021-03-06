using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Dtos;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.ViewModels;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Models
{
    public class PhieuNXViewModel
    {
        public PhieuNX PhieuNX { get; set; }
        public CTPhieuNX CTPhieuNX { get; set; }
        public IPagedList<PhieuNXDto> PhieuNXDtos { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }

        public List<ListViewModel> ListGT { get; set; }
        public List<ListViewModel> ListLoaiPhieu { get; set; }
    }
}