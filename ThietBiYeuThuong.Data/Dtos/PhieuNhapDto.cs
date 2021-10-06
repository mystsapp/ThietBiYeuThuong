using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Dtos
{
    public class PhieuNhapDto
    {
        [DisplayName("Số phiếu")]
        public string SoPhieu { get; set; }

        [DisplayName("Đơn vị")]
        public string DonVi { get; set; }

        [DisplayName("Trạng thái TB")]
        public string TrangThai { get; set; }

        [DisplayName("Người nhập")]
        public string NguoiNhap { get; set; }

        [DisplayName("Ngày nhập")]
        public DateTime? NgayNhap { get; set; }

        [DisplayName("Người sửa")]
        public string NguoiSua { get; set; }

        [DisplayName("Ngày sửa")]
        public DateTime? NgaySua { get; set; }

        public string LogFile { get; set; }
    }
}