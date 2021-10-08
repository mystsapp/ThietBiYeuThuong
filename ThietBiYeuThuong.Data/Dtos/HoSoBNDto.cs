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
    public class HoSoBNDto
    {
        [Key]
        [DisplayName("Số phiếu")]
        public string SoPhieu { get; set; }

        [DisplayName("Loại phiếu")]
        public string LoaiPhieu { get; set; }

        [DisplayName("Bệnh nhân")]
        public string TenBN { get; set; }

        [DisplayName("Nhân viên y tế")]
        public string NVYT { get; set; }

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

        public int STT { get; set; }

        [DisplayName("NV trực")]
        public string NVTruc { get; set; }
    }
}