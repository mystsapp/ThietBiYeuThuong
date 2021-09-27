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
        [DisplayName("Số phiếu")]
        public string SoPhieu { get; set; }

        [DisplayName("Loại phiếu")]
        public string LoaiPhieu { get; set; }

        [DisplayName("Người lập phiếu")]
        public string LapPhieu { get; set; }

        [DisplayName("Ngày lập")]
        public DateTime? NgayLap { get; set; }

        public int STT { get; set; }

        [DisplayName("NV trực")]
        public string NVTruc { get; set; }

        [DisplayName("Họ tên T.N")]
        public string HoTenTN { get; set; }

        [DisplayName("SĐT")]
        public string SDT_TN { get; set; }

        [DisplayName("GioiTinh")]
        public string GT_TN { get; set; }

        [DisplayName("Họ tên B.N")]
        public string HoTenBN { get; set; }

        [DisplayName("Năm sinh")]
        public int NamSinh { get; set; }

        [DisplayName("CMND/CCCD")]
        public int CMND_CCCD_BN { get; set; }

        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [DisplayName("Họ tên NV Y Tế")]
        public string HoTenNVYTe { get; set; }

        [DisplayName("SĐT")]
        public string SDT_NVYT { get; set; }

        [DisplayName("Đơn vị")]
        public string DonVi { get; set; }

        [DisplayName("Tình trạng B.N")]
        public string TinhTrangBN { get; set; }

        [DisplayName("Bệnh nền B.N")]
        public string BenhNenBN { get; set; }

        [DisplayName("Chỉ số SPO2 B.N")]
        public string ChiSoSPO2 { get; set; }

        [DisplayName("Tình trạng BN sau khi thở OXY")]
        public string TinhTrangBNSauO2 { get; set; }

        [DisplayName("Kết luận")]
        public string KetLuan { get; set; }
    }
}