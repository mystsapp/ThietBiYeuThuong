using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Utilities;
using ThietBiYeuThuong.Data.ViewModels;
using ThietBiYeuThuong.Web.Models;
using ThietBiYeuThuong.Web.Services;

namespace ThietBiYeuThuong.Web.Controllers
{
    public class TinhTonController : BaseController
    {
        private readonly ITinhTonService _tinhTonService;
        private readonly ICTHoSoBNService _cTPhieuNXService;

        [BindProperty]
        public TinhTonViewModel TinhVM { get; set; }

        public TinhTonController(ITinhTonService tinhTonService, ICTHoSoBNService cTPhieuNXService)
        {
            TinhVM = new TinhTonViewModel()
            {
                TinhTon = new Data.Models.TinhTon()
            };
            _tinhTonService = tinhTonService;
            _cTPhieuNXService = cTPhieuNXService;
        }

        public async Task<IActionResult> Index(string searchFromDate, string searchToDate)
        {
            // moi load vao
            if (string.IsNullOrEmpty(searchFromDate) && string.IsNullOrEmpty(searchToDate))
            {
                ViewBag.searchFromDate = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.searchToDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ViewBag.searchFromDate = searchFromDate;
                ViewBag.searchToDate = searchToDate;
            }
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            TinhVM.StrUrl = UriHelper.GetDisplayUrl(Request);

            TinhVM.CTHoSoBNs = await _tinhTonService.ListCTPhieuNX(searchFromDate, searchToDate);

            if (TinhVM.CTHoSoBNs.Count > 0)
            {
                var tinhTonLast = _tinhTonService.GetLast("", DateTime.Parse(searchFromDate).AddDays(-1).ToShortDateString());
                TinhVM.TonDau = tinhTonLast == null ? 0 : tinhTonLast.SoLuongTon;
                //TinhVM.CongPhatSinhNhap = TinhVM.CTPhieuNXes.Where(x => x.PhieuNX.LoaiPhieu == "PN").Sum(x => x.SoLuong); // tong nhap
                //TinhVM.CongPhatSinhXuat = TinhVM.CTPhieuNXes.Where(x => x.PhieuNX.LoaiPhieu == "PX").Sum(x => x.SoLuong); // tong xuat
                TinhVM.TonCuoi = TinhVM.TonDau + TinhVM.CongPhatSinhNhap - TinhVM.CongPhatSinhXuat;

                //SetAlert("")
                // save vao tinhton
                var tinhTon = new TinhTon()
                {
                    NgayCT = DateTime.Parse(searchToDate),
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Username,
                    SoLuongNhap = TinhVM.CongPhatSinhNhap,
                    SoLuongXuat = TinhVM.CongPhatSinhXuat,
                    SoLuongTon = TinhVM.TonCuoi
                };

                var tinhTons = _tinhTonService.Find_Equal_By_Date(tinhTon.NgayCT.Value);
                if (tinhTons.Count > 0) // co ton tai
                {
                    var tinhTon1 = await _tinhTonService.GetById(tinhTons.FirstOrDefault().Id);
                    tinhTon1.LogFile += "==== người chạy lại " + user.Username + " lúc: " + DateTime.Now;
                    tinhTon1.SoLuongTon = TinhVM.TonCuoi;
                    tinhTon1.SoLuongNhap = TinhVM.CongPhatSinhNhap;
                    tinhTon1.SoLuongXuat = TinhVM.CongPhatSinhXuat;

                    await _tinhTonService.UpdateAsync(tinhTon1);
                }
                else
                {
                    // ghi log
                    tinhTon.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

                    await _tinhTonService.CreateAsync(tinhTon);
                }

                SetAlert("Tính thành công!", "success");
            }

            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                string dateString = await _cTPhieuNXService.CheckTonDau(DateTime.Parse(searchFromDate));
                if (!string.IsNullOrEmpty(dateString))
                {
                    ModelState.AddModelError("", "Ngày " + dateString + " chưa tính tồn!");
                }
            }

            return View(TinhVM);
        }

        public async Task<JsonResult> CheckTonDau(string tuNgay)
        {
            DateTime fromDate = DateTime.Parse(tuNgay);

            // tonquy truoc ngay fromdate => xem co ton dau` ko ( tranh truong hop chua tinh ton dau cho vai phieu )
            string kVCTPTCs1 = await _cTPhieuNXService.CheckTonDau(DateTime.Parse(tuNgay));
            if (!string.IsNullOrEmpty(kVCTPTCs1))
            {
                return Json(new
                {
                    status = false,
                    message = "Ngày " + kVCTPTCs1 + " chưa tính tồn!"
                });
            }

            return Json(new
            {
                status = true,
                message = "Good job!"
            });
        }

        public async Task<JsonResult> CheckNgayTonQuy(string tuNgay, string denNgay)
        {
            DateTime fromDate = DateTime.Parse(tuNgay);
            DateTime toDate = DateTime.Parse(denNgay);

            if (fromDate > DateTime.Now || toDate > DateTime.Now)
            {
                return Json(new
                {
                    status = false,
                    message = "Ngày tháng không hợp lệ"
                });
            }

            if (fromDate > toDate) // dao nguoc lai
            {
                return Json(new
                {
                    status = false,
                    message = "Từ ngày <b> không được lớn hơn </b> đến ngày"
                });
            }

            // tonquy truoc ngay fromdate => xem co ton dau` ko ( tranh truong hop chua tinh ton dau cho vai phieu )
            string kVCTPTCs1 = await _tinhTonService.CheckTonDauStatus(DateTime.Parse(tuNgay));
            if (!string.IsNullOrEmpty(kVCTPTCs1))
            {
                return Json(new
                {
                    status = false,
                    message = "Ngày " + kVCTPTCs1 + " chưa tính tồn quỹ"
                });
            }

            //bool boolDate1 = _tonQuyService.Find_Equal_By_Date(toDate);
            //if (boolDate1) // co rồi
            //{
            //    return Json(new
            //    {
            //        status = false,
            //        message = "Ngày " + toDate.ToString("dd/MM/yyyy") + " đã tính tồn quỹ"
            //    });
            //}

            return Json(new
            {
                status = true,
                message = "Good job!"
            });
        }
    }
}