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
    public class BenhNhanController : BaseController
    {
        private readonly IPhieuNXService _phieuNXService;
        private readonly ICTPhieuNXService _cTPhieuNXService;
        private readonly IBenhNhanService _benhNhanService;
        private readonly ITinhTrangBNService _tinhTrangBNService;

        [BindProperty]
        public BenhNhanViewModel BenhNhanVM { get; set; }

        public BenhNhanController(IPhieuNXService phieuNXService,
                                  ICTPhieuNXService cTPhieuNXService,
                                  IBenhNhanService benhNhanService,
                                  ITinhTrangBNService tinhTrangBNService)
        {
            _phieuNXService = phieuNXService;
            _cTPhieuNXService = cTPhieuNXService;
            _benhNhanService = benhNhanService;
            _tinhTrangBNService = tinhTrangBNService;
            BenhNhanVM = new BenhNhanViewModel()
            {
                BenhNhan = new Data.Models.BenhNhan(),
                TinhTrangBN = new TinhTrangBN()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            BenhNhanVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            BenhNhanVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                BenhNhanVM.BenhNhan = await _benhNhanService.GetById(id);
                ViewBag.id = BenhNhanVM.BenhNhan.MaBN;
            }
            else
            {
                BenhNhanVM.BenhNhan = new Data.Models.BenhNhan();
            }
            BenhNhanVM.BenhNhans = await _benhNhanService.ListBenhNhan(searchString, searchFromDate, searchToDate, page);
            return View(BenhNhanVM);
        }

        public IActionResult Create(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            BenhNhanVM.StrUrl = strUrl;
            BenhNhanVM.ListGT = ListGT();

            return View(BenhNhanVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                BenhNhanVM = new BenhNhanViewModel()
                {
                    BenhNhan = new BenhNhan(),
                    StrUrl = strUrl
                };

                return View(BenhNhanVM);
            }

            BenhNhanVM.BenhNhan.NgayTao = DateTime.Now;
            BenhNhanVM.BenhNhan.NguoiTao = user.Username;

            // next sophieu --> bat buoc phai co'
            BenhNhanVM.BenhNhan.MaBN = _benhNhanService.GetMaBN("BN");
            // next sophieu

            // ghi log
            BenhNhanVM.BenhNhan.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            // them tinh trang
            BenhNhanVM.TinhTrangBN.BenhNhanId = BenhNhanVM.BenhNhan.MaBN;
            BenhNhanVM.TinhTrangBN.NgayTao = DateTime.Now;
            BenhNhanVM.TinhTrangBN.NguoiTao = user.Username;
            BenhNhanVM.TinhTrangBN.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _benhNhanService.CreateAsync(BenhNhanVM.BenhNhan); // save BN
                await _tinhTrangBNService.CreateAsync(BenhNhanVM.TinhTrangBN); // save TinhTrangBN

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = BenhNhanVM.BenhNhan.MaBN, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(BenhNhanVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            BenhNhanVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Bệnh nhân này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            BenhNhanVM.BenhNhan = await _benhNhanService.GetById(id);

            if (BenhNhanVM.BenhNhan == null)
            {
                ViewBag.ErrorMessage = "Bệnh nhân này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            BenhNhanVM.ListGT = ListGT();

            return View(BenhNhanVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != BenhNhanVM.BenhNhan.MaBN)
            {
                ViewBag.ErrorMessage = "Bệnh nhân này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                BenhNhanVM.BenhNhan.NgaySua = DateTime.Now;
                BenhNhanVM.BenhNhan.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _benhNhanService.GetByIdAsNoTracking(id);

                if (t.HoTenTN != BenhNhanVM.BenhNhan.HoTenTN)
                {
                    temp += String.Format("- HoTenTN thay đổi: {0}->{1}", t.HoTenTN, BenhNhanVM.BenhNhan.HoTenTN);
                }

                if (t.SDT_TN != BenhNhanVM.BenhNhan.SDT_TN)
                {
                    temp += String.Format("- Descripttion thay đổi: {0}->{1}", t.SDT_TN, BenhNhanVM.BenhNhan.SDT_TN);
                }

                if (t.GT_TN != BenhNhanVM.BenhNhan.GT_TN)
                {
                    temp += String.Format("- GT_TN thay đổi: {0}->{1}", t.GT_TN, BenhNhanVM.BenhNhan.GT_TN);
                }

                if (t.HoTenBN != BenhNhanVM.BenhNhan.HoTenBN)
                {
                    temp += String.Format("- HoTenBN thay đổi: {0}->{1}", t.HoTenBN, BenhNhanVM.BenhNhan.HoTenBN);
                }

                if (t.NamSinh != BenhNhanVM.BenhNhan.NamSinh)
                {
                    temp += String.Format("- NamSinh thay đổi: {0}->{1}", t.NamSinh, BenhNhanVM.BenhNhan.NamSinh);
                }

                if (t.CMND_CCCD_BN != BenhNhanVM.BenhNhan.CMND_CCCD_BN)
                {
                    temp += String.Format("- CMND_CCCD_BN thay đổi: {0}->{1}", t.CMND_CCCD_BN, BenhNhanVM.BenhNhan.CMND_CCCD_BN);
                }

                if (t.DiaChi != BenhNhanVM.BenhNhan.DiaChi)
                {
                    temp += String.Format("- DiaChi thay đổi: {0}->{1}", t.DiaChi, BenhNhanVM.BenhNhan.DiaChi);
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
                    BenhNhanVM.BenhNhan.LogFile = t.LogFile;
                }

                try
                {
                    await _benhNhanService.UpdateAsync(BenhNhanVM.BenhNhan);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(BenhNhanVM);
                }
            }
            // not valid

            return View(BenhNhanVM);
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