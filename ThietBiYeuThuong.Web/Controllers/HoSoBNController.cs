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
    public class HoSoBNController : BaseController
    {
        private readonly IHoSoBNService _hoSoBNService;
        private readonly ICTHoSoBNService _cTHoSoBNService;

        [BindProperty]
        public HoSoBNViewModel HoSoBNVM { get; set; }

        public HoSoBNController(IHoSoBNService hoSoBNService, ICTHoSoBNService cTHoSoBNService)
        {
            _hoSoBNService = hoSoBNService;
            _cTHoSoBNService = cTHoSoBNService;
            HoSoBNVM = new HoSoBNViewModel()
            {
                HoSoBN = new Data.Models.HoSoBN(),
                CTHoSoBN = new CTHoSoBN()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            HoSoBNVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            HoSoBNVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                HoSoBNVM.HoSoBN = await _hoSoBNService.GetById(id);
                ViewBag.id = HoSoBNVM.HoSoBN.SoPhieu;
            }
            else
            {
                HoSoBNVM.HoSoBN = new Data.Models.HoSoBN();
            }
            HoSoBNVM.HoSoBNDtos = await _hoSoBNService.ListHoSoBN(searchString, searchFromDate, searchToDate, page);
            return View(HoSoBNVM);
        }

        public IActionResult Create(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            HoSoBNVM.StrUrl = strUrl;

            //HoSoBNVM.ListGT = ListGT();
            //HoSoBNVM.ListLoaiPhieu = ListLoaiPhieu();

            return View(HoSoBNVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                HoSoBNVM = new HoSoBNViewModel()
                {
                    HoSoBN = new HoSoBN(),
                    //ListGT = ListGT(),
                    //ListLoaiPhieu = ListLoaiPhieu(),
                    StrUrl = strUrl
                };

                return View(HoSoBNVM);
            }

            HoSoBNVM.HoSoBN.NgayLap = DateTime.Now;
            HoSoBNVM.HoSoBN.LapPhieu = user.Username;

            HoSoBNVM.HoSoBN.SoPhieu = _hoSoBNService.GetSoPhieu("PN");
            //// next sophieu --> bat buoc phai co'
            //switch (HoSoBNVM.PhieuNX.LoaiPhieu)
            //{
            //    case "PN": // nhap
            //        HoSoBNVM.PhieuNX.SoPhieu = _phieuNXService.GetSoPhieu("PN");
            //        break;

            //    default: // xuat
            //        HoSoBNVM.PhieuNX.SoPhieu = _phieuNXService.GetSoPhieu("PX");
            //        break;
            //}
            //// next sophieu

            // ghi log
            HoSoBNVM.HoSoBN.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _hoSoBNService.CreateAsync(HoSoBNVM.HoSoBN); // save

                // if loaiphieu = pn --> save vao CTPhieu
                if (HoSoBNVM.HoSoBN.LoaiPhieu == "PN")
                {
                    HoSoBNVM.CTHoSoBN.HoSoBNId = HoSoBNVM.HoSoBN.SoPhieu;
                    HoSoBNVM.CTHoSoBN.LapPhieu = user.Username;
                    HoSoBNVM.CTHoSoBN.NgayNhap = DateTime.Now;

                    HoSoBNVM.CTHoSoBN.ThietBiId = HoSoBNVM.CTHoSoBN.ThietBiId;
                    HoSoBNVM.CTHoSoBN.SoLuong = HoSoBNVM.CTHoSoBN.SoLuong;
                    HoSoBNVM.CTHoSoBN.GhiChu = HoSoBNVM.CTHoSoBN.GhiChu;

                    // next sophieuct --> bat buoc phai co'
                    switch (HoSoBNVM.HoSoBN.LoaiPhieu)
                    {
                        case "PN": // nhap
                            HoSoBNVM.CTHoSoBN.SoPhieuCT = _cTHoSoBNService.GetSoPhieuCT("CN");
                            break;

                        default: // xuat
                            HoSoBNVM.HoSoBN.SoPhieu = _cTHoSoBNService.GetSoPhieuCT("CX");
                            break;
                    }
                    // next sophieuct

                    // ghi log
                    HoSoBNVM.CTHoSoBN.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

                    await _cTHoSoBNService.Create(HoSoBNVM.CTHoSoBN);
                }

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = HoSoBNVM.HoSoBN.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(HoSoBNVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            HoSoBNVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            HoSoBNVM.HoSoBN = await _hoSoBNService.GetById(id);

            if (HoSoBNVM.HoSoBN == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            HoSoBNVM.ListGT = ListGT();
            HoSoBNVM.ListLoaiPhieu = ListLoaiPhieu();

            return View(HoSoBNVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != HoSoBNVM.HoSoBN.SoPhieu)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                HoSoBNVM.HoSoBN.NgaySua = DateTime.Now;
                HoSoBNVM.HoSoBN.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _hoSoBNService.GetByIdAsNoTracking(id);

                if (t.BenhNhanId != HoSoBNVM.HoSoBN.BenhNhanId)
                {
                    temp += String.Format("- BenhNhanId thay đổi: {0}->{1}", t.BenhNhanId, HoSoBNVM.HoSoBN.BenhNhanId);
                }

                if (t.HoTenNVYTe != HoSoBNVM.HoSoBN.HoTenNVYTe)
                {
                    temp += String.Format("- HoTenNVYTe thay đổi: {0}->{1}", t.HoTenNVYTe, HoSoBNVM.HoSoBN.HoTenNVYTe);
                }

                if (t.SDT_NVYT != HoSoBNVM.HoSoBN.SDT_NVYT)
                {
                    temp += String.Format("- SDT_NVYT thay đổi: {0}->{1}", t.SDT_NVYT, HoSoBNVM.HoSoBN.SDT_NVYT);
                }

                if (t.DonVi != HoSoBNVM.HoSoBN.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, HoSoBNVM.HoSoBN.DonVi);
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
                    HoSoBNVM.HoSoBN.LogFile = t.LogFile;
                }

                try
                {
                    await _hoSoBNService.UpdateAsync(HoSoBNVM.HoSoBN);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(HoSoBNVM);
                }
            }
            // not valid

            return View(HoSoBNVM);
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