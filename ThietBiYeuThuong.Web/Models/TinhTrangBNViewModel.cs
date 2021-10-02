using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Web.Models
{
    public class TinhTrangBNViewModel
    {
        public TinhTrangBN TinhTrangBN { get; set; }
        public IEnumerable<TinhTrangBN> TinhTrangBNs { get; set; }
        public string StrUrl { get; set; }
        public int Page { get; set; }
    }
}