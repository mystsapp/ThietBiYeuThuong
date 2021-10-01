using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Dtos
{
    public class PhieuNXDto
    {
        [Key]
        [DisplayName("Số phiếu")]
        public string SoPhieu { get; set; }

        [DisplayName("Loại phiếu")]
        public string LoaiPhieu { get; set; }

        [DisplayName("Bệnh nhân")]
        public string TenBN { get; set; }

        [DisplayName("Người lập phiếu")]
        public string LapPhieu { get; set; }

        [DisplayName("Ngày lập")]
        public DateTime? NgayLap { get; set; }

        [DisplayName("Người sửa")]
        public string NguoiSua { get; set; }

        [DisplayName("Ngày sửa")]
        public DateTime? NgaySua { get; set; }

        public string LogFile { get; set; }

        /// <summary>
        /// ////////////////////
        /// </summary>
        ///

        [DisplayName("Họ tên NV Y Tế")]
        public string HoTenNVYTe { get; set; }

        [DisplayName("SĐT")]
        public string SDT_NVYT { get; set; }

        [DisplayName("Đơn vị")]
        public string DonVi { get; set; }

        public int STT { get; set; }

        [DisplayName("NV trực")]
        public string NVTruc { get; set; }
    }
}