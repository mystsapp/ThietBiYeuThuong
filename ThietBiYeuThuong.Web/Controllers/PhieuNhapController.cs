using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Utilities;
using ThietBiYeuThuong.Web.Models;
using ThietBiYeuThuong.Web.Services;

namespace ThietBiYeuThuong.Web.Controllers
{
    public class PhieuNhapController : BaseController
    {
        private readonly IPhieuNhapService _phieuNhapService;
        private readonly ILoaiThietBiService _loaiThietBiService;
        private readonly ITrangThaiService _trangThaiService;
        private readonly IThietBiService _thietBiService;
        private readonly ICTPhieuService _cTPhieuService;

        [BindProperty]
        public PhieuNhapViewModel PhieuNhapVM { get; set; }

        public PhieuNhapController(IPhieuNhapService phieuNhapService,
                                   ILoaiThietBiService loaiThietBiService,
                                   ITrangThaiService trangThaiService,
                                   IThietBiService thietBiService,
                                   ICTPhieuService cTPhieuService)
        {
            PhieuNhapVM = new PhieuNhapViewModel()
            {
                PhieuNhap = new Data.Models.PhieuNhap()
            };
            _phieuNhapService = phieuNhapService;
            _loaiThietBiService = loaiThietBiService;
            _trangThaiService = trangThaiService;
            _thietBiService = thietBiService;
            _cTPhieuService = cTPhieuService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            PhieuNhapVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            PhieuNhapVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                PhieuNhapVM.PhieuNhap = await _phieuNhapService.GetById(id);
                ViewBag.id = PhieuNhapVM.PhieuNhap.SoPhieu;
            }
            else
            {
                PhieuNhapVM.PhieuNhap = new Data.Models.PhieuNhap();
            }
            PhieuNhapVM.PhieuNhaps = await _phieuNhapService.ListPhieuNhap(searchString, searchFromDate, searchToDate, page);
            return View(PhieuNhapVM);
        }

        public async Task<IActionResult> Create_Day(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            PhieuNhapVM.Page = page;
            PhieuNhapVM.LoaiThietBis = await _loaiThietBiService.GetAll();
            PhieuNhapVM.TrangThais = await _trangThaiService.GetAll();

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Create_Day")]
        public async Task<IActionResult> Create_Day_Post(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuNhapVM = new PhieuNhapViewModel()
                {
                    PhieuNhap = new PhieuNhap(),
                    StrUrl = strUrl
                };

                return View(PhieuNhapVM);
            }

            PhieuNhapVM.PhieuNhap.NgayNhap = DateTime.Now;
            PhieuNhapVM.PhieuNhap.NguoiNhap = user.Username;
            PhieuNhapVM.PhieuNhap.SoPhieu = _phieuNhapService.GetSoPhieu("PN");

            // SL
            if (PhieuNhapVM.SoLuong == 0)
            {
                PhieuNhapVM.SoLuong = 1;
            }
            if (PhieuNhapVM.SoLuong == 1)
            {
                PhieuNhapVM.ThietBi.MaTB = _thietBiService.GetMaTB("TB");
                var thietBi = new ThietBi()
                {
                    MaTB = _thietBiService.GetMaTB("TB"),
                    TenTB = PhieuNhapVM.ThietBi.TenTB,
                    LoaiTBId = PhieuNhapVM.ThietBi.LoaiTBId,
                    TrangThaiId = 3, // đầy
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Username
                };
                await _thietBiService.CreateAsync(thietBi); // save thietbi

                // save CTPhieu
                PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
                PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
                PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
                PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
                PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
            }
            else
            {
                for (int i = 1; i <= PhieuNhapVM.SoLuong; i++)
                {
                    var thietBi = new ThietBi()
                    {
                        MaTB = _thietBiService.GetMaTB("TB"),
                        TenTB = PhieuNhapVM.ThietBi.TenTB,
                        LoaiTBId = PhieuNhapVM.ThietBi.LoaiTBId,
                        TrangThaiId = 3 // đầy
                    };
                    await _thietBiService.CreateAsync(thietBi); // save thietbi

                    // save CTPhieu
                    PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
                    PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
                    PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
                    PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
                    PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
                }
            }
            // ghi log
            PhieuNhapVM.PhieuNhap.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNhapService.CreateAsync(PhieuNhapVM.PhieuNhap); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuNhapVM.PhieuNhap.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuNhapVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            PhieuNhapVM.PhieuNhap = await _phieuNhapService.GetById(id);

            if (PhieuNhapVM.PhieuNhap == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != PhieuNhapVM.PhieuNhap.SoPhieu)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                PhieuNhapVM.PhieuNhap.NgaySua = DateTime.Now;
                PhieuNhapVM.PhieuNhap.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _phieuNhapService.GetByIdAsNoTracking(id);

                if (t.DonVi != PhieuNhapVM.PhieuNhap.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, PhieuNhapVM.PhieuNhap.DonVi);
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
                    PhieuNhapVM.PhieuNhap.LogFile = t.LogFile;
                }

                try
                {
                    await _phieuNhapService.UpdateAsync(PhieuNhapVM.PhieuNhap);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(PhieuNhapVM);
                }
            }
            // not valid

            return View(PhieuNhapVM);
        }

        public IActionResult DetailsRedirect(string strUrl/*, string tabActive*/)
        {
            //if (!string.IsNullOrEmpty(tabActive))
            //{
            //    strUrl = strUrl + "&tabActive=" + tabActive; // for redirect tab
            //}
            return Redirect(strUrl);
        }
    }
}