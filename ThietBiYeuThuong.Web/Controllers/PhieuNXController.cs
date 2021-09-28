using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
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
    public class PhieuNXController : BaseController
    {
        private readonly IPhieuNXService _phieuNXService;
        private readonly ICTPhieuNXService _cTPhieuNXService;

        [BindProperty]
        public PhieuNXViewModel PhieuVM { get; set; }

        public PhieuNXController(IPhieuNXService phieuNXService, ICTPhieuNXService cTPhieuNXService)
        {
            _phieuNXService = phieuNXService;
            _cTPhieuNXService = cTPhieuNXService;
            PhieuVM = new PhieuNXViewModel()
            {
                PhieuNX = new Data.Models.PhieuNX(),
                CTPhieuNX = new CTPhieuNX()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            PhieuVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            PhieuVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                PhieuVM.PhieuNX = await _phieuNXService.GetById(id);
                ViewBag.id = PhieuVM.PhieuNX.SoPhieu;
            }
            else
            {
                PhieuVM.PhieuNX = new Data.Models.PhieuNX();
            }
            PhieuVM.PhieuNXDtos = await _phieuNXService.ListPhieuNX(searchString, searchFromDate, searchToDate, page);
            return View(PhieuVM);
        }

        public IActionResult Create(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuVM.StrUrl = strUrl;

            PhieuVM.ListGT = ListGT();
            PhieuVM.ListLoaiPhieu = ListLoaiPhieu();

            return View(PhieuVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuVM = new PhieuNXViewModel()
                {
                    PhieuNX = new PhieuNX(),
                    ListGT = ListGT(),
                    ListLoaiPhieu = ListLoaiPhieu(),
                    StrUrl = strUrl
                };

                return View(PhieuVM);
            }

            PhieuVM.PhieuNX.NgayLap = DateTime.Now;
            PhieuVM.PhieuNX.LapPhieu = user.Username;

            // next sophieu --> bat buoc phai co'
            switch (PhieuVM.PhieuNX.LoaiPhieu)
            {
                case "PN": // nhap
                    PhieuVM.PhieuNX.SoPhieu = _phieuNXService.GetSoPhieu("PN");
                    break;

                default: // xuat
                    PhieuVM.PhieuNX.SoPhieu = _phieuNXService.GetSoPhieu("PX");
                    break;
            }
            // next sophieu

            // ghi log
            PhieuVM.PhieuNX.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNXService.CreateAsync(PhieuVM.PhieuNX); // save

                // if loaiphieu = pn --> save vao CTPhieu
                if (PhieuVM.PhieuNX.LoaiPhieu == "PN")
                {
                    PhieuVM.CTPhieuNX.PhieuNXId = PhieuVM.PhieuNX.SoPhieu;
                    PhieuVM.CTPhieuNX.LapPhieu = user.Username;
                    PhieuVM.CTPhieuNX.NgayNhap = DateTime.Now;

                    PhieuVM.CTPhieuNX.ThietBi = PhieuVM.CTPhieuNX.ThietBi;
                    PhieuVM.CTPhieuNX.SoLuong = PhieuVM.CTPhieuNX.SoLuong;
                    PhieuVM.CTPhieuNX.GhiChu = PhieuVM.CTPhieuNX.GhiChu;

                    // next sophieuct --> bat buoc phai co'
                    switch (PhieuVM.PhieuNX.LoaiPhieu)
                    {
                        case "PN": // nhap
                            PhieuVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CN");
                            break;

                        default: // xuat
                            PhieuVM.PhieuNX.SoPhieu = _cTPhieuNXService.GetSoPhieuCT("CX");
                            break;
                    }
                    // next sophieuct

                    // ghi log
                    PhieuVM.CTPhieuNX.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

                    await _cTPhieuNXService.Create(PhieuVM.CTPhieuNX);
                }

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuVM.PhieuNX.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            PhieuVM.PhieuNX = await _phieuNXService.GetById(id);

            if (PhieuVM.PhieuNX == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            PhieuVM.ListGT = ListGT();
            PhieuVM.ListLoaiPhieu = ListLoaiPhieu();

            return View(PhieuVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != PhieuVM.PhieuNX.SoPhieu)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                PhieuVM.PhieuNX.NgaySua = DateTime.Now;
                PhieuVM.PhieuNX.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _phieuNXService.GetByIdAsNoTracking(id);

                if (t.HoTenTN != PhieuVM.PhieuNX.HoTenTN)
                {
                    temp += String.Format("- HoTenTN thay đổi: {0}->{1}", t.HoTenTN, PhieuVM.PhieuNX.HoTenTN);
                }

                if (t.SDT_TN != PhieuVM.PhieuNX.SDT_TN)
                {
                    temp += String.Format("- SDT_TN thay đổi: {0}->{1}", t.SDT_TN, PhieuVM.PhieuNX.SDT_TN);
                }

                if (t.GT_TN != PhieuVM.PhieuNX.GT_TN)
                {
                    temp += String.Format("- GT_TN thay đổi: {0}->{1}", t.GT_TN, PhieuVM.PhieuNX.GT_TN);
                }

                if (t.HoTenBN != PhieuVM.PhieuNX.HoTenBN)
                {
                    temp += String.Format("- HoTenBN thay đổi: {0}->{1}", t.HoTenBN, PhieuVM.PhieuNX.HoTenBN);
                }

                if (t.NamSinh != PhieuVM.PhieuNX.NamSinh)
                {
                    temp += String.Format("- NamSinh thay đổi: {0}->{1}", t.NamSinh, PhieuVM.PhieuNX.NamSinh);
                }

                if (t.CMND_CCCD_BN != PhieuVM.PhieuNX.CMND_CCCD_BN)
                {
                    temp += String.Format("- CMND_CCCD_BN thay đổi: {0}->{1}", t.CMND_CCCD_BN, PhieuVM.PhieuNX.CMND_CCCD_BN);
                }

                if (t.DiaChi != PhieuVM.PhieuNX.DiaChi)
                {
                    temp += String.Format("- DiaChi thay đổi: {0}->{1}", t.DiaChi, PhieuVM.PhieuNX.DiaChi);
                }

                if (t.HoTenNVYTe != PhieuVM.PhieuNX.HoTenNVYTe)
                {
                    temp += String.Format("- HoTenNVYTe thay đổi: {0}->{1}", t.HoTenNVYTe, PhieuVM.PhieuNX.HoTenNVYTe);
                }

                if (t.SDT_NVYT != PhieuVM.PhieuNX.SDT_NVYT)
                {
                    temp += String.Format("- SDT_NVYT thay đổi: {0}->{1}", t.SDT_NVYT, PhieuVM.PhieuNX.SDT_NVYT);
                }

                if (t.DonVi != PhieuVM.PhieuNX.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, PhieuVM.PhieuNX.DonVi);
                }

                if (t.TinhTrangBN != PhieuVM.PhieuNX.TinhTrangBN)
                {
                    temp += String.Format("- TinhTrangBN thay đổi: {0}->{1}", t.TinhTrangBN, PhieuVM.PhieuNX.TinhTrangBN);
                }

                if (t.BenhNenBN != PhieuVM.PhieuNX.BenhNenBN)
                {
                    temp += String.Format("- BenhNenBN thay đổi: {0}->{1}", t.BenhNenBN, PhieuVM.PhieuNX.BenhNenBN);
                }

                if (t.ChiSoSPO2 != PhieuVM.PhieuNX.ChiSoSPO2)
                {
                    temp += String.Format("- ChiSoSPO2 thay đổi: {0}->{1}", t.ChiSoSPO2, PhieuVM.PhieuNX.ChiSoSPO2);
                }

                if (t.TinhTrangBNSauO2 != PhieuVM.PhieuNX.TinhTrangBNSauO2)
                {
                    temp += String.Format("- TinhTrangBNSauO2 thay đổi: {0}->{1}", t.TinhTrangBNSauO2, PhieuVM.PhieuNX.TinhTrangBNSauO2);
                }

                if (t.KetLuan != PhieuVM.PhieuNX.KetLuan)
                {
                    temp += String.Format("- KetLuan thay đổi: {0}->{1}", t.KetLuan, PhieuVM.PhieuNX.KetLuan);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật tour: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.LogFile = t.LogFile + log;
                    PhieuVM.PhieuNX.LogFile = t.LogFile;
                }

                try
                {
                    await _phieuNXService.UpdateAsync(PhieuVM.PhieuNX);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(PhieuVM);
                }
            }
            // not valid

            return View(PhieuVM);
        }

        public IActionResult DetailsRedirect(string strUrl/*, string tabActive*/)
        {
            //if (!string.IsNullOrEmpty(tabActive))
            //{
            //    strUrl = strUrl + "&tabActive=" + tabActive; // for redirect tab
            //}
            return Redirect(strUrl);
        }

        private List<ListViewModel> ListGT()
        {
            return new List<ListViewModel>()
            {
                new ListViewModel() { Name = "Nam" },
                new ListViewModel() { Name = "Nữ" }
            };
        }

        private List<ListViewModel> ListLoaiPhieu()
        {
            return new List<ListViewModel>()
            {
                new ListViewModel() { Name = "PN" },
                new ListViewModel() { Name = "PX" }
            };
        }
    }
}