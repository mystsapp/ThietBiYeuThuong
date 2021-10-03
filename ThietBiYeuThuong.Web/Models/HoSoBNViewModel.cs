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
    public class HoSoBNViewModel
    {
        public HoSoBN HoSoBN { get; set; }
        public CTHoSoBN CTHoSoBN { get; set; }
        public IPagedList<HoSoBNDto> HoSoBNDtos { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }

        //public List<ListViewModel> ListGT { get; set; }
        //public List<ListViewModel> ListLoaiPhieu { get; set; }
    }
}