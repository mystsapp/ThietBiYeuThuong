using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Web.Models
{
    public class CTHoSoBNViewModel
    {
        public HoSoBN HoSoBN { get; set; }
        public CTHoSoBN CTHoSoBN { get; set; }
        public IEnumerable<CTHoSoBN> CTHoSoBNs { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}