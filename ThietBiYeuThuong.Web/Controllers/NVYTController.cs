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
    public class NVYTController : BaseController
    {
        private readonly INVYTService _nVYTService;

        [BindProperty]
        public NVYTViewModel NVYT_VM { get; set; }

        public NVYTController(INVYTService nVYTService)
        {
            _nVYTService = nVYTService;
            NVYT_VM = new NVYTViewModel()
            {
                NhanVienYTe = new NhanVienYTe()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            NVYT_VM.StrUrl = UriHelper.GetDisplayUrl(Request);
            NVYT_VM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                NVYT_VM.NhanVienYTe = await _nVYTService.GetById(id);
                ViewBag.id = NVYT_VM.NhanVienYTe.MaNVYT;
            }
            else
            {
                NVYT_VM.NhanVienYTe = new Data.Models.NhanVienYTe();
            }
            NVYT_VM.NhanVienYTes = await _nVYTService.ListNhanVienYTe(searchString, searchFromDate, searchToDate, page);
            return View(NVYT_VM);
        }

        public IActionResult Create(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            NVYT_VM.StrUrl = strUrl;

            return View(NVYT_VM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                NVYT_VM = new NVYTViewModel()
                {
                    NhanVienYTe = new NhanVienYTe(),
                    StrUrl = strUrl
                };

                return View(NVYT_VM);
            }

            NVYT_VM.NhanVienYTe.NgayTao = DateTime.Now;
            NVYT_VM.NhanVienYTe.NguoiTao = user.Username;

            // next sophieu --> bat buoc phai co'
            NVYT_VM.NhanVienYTe.MaNVYT = _nVYTService.GetMaNVYT("NV");
            // next sophieu

            // ghi log
            NVYT_VM.NhanVienYTe.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _nVYTService.CreateAsync(NVYT_VM.NhanVienYTe); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = NVYT_VM.NhanVienYTe.MaNVYT, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(NVYT_VM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            NVYT_VM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "NV Y Tế này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            NVYT_VM.NhanVienYTe = await _nVYTService.GetById(id);

            if (NVYT_VM.NhanVienYTe == null)
            {
                ViewBag.ErrorMessage = "NV Y Tế này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(NVYT_VM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != NVYT_VM.NhanVienYTe.MaNVYT)
            {
                ViewBag.ErrorMessage = "NV Y Tế này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                NVYT_VM.NhanVienYTe.NgaySua = DateTime.Now;
                NVYT_VM.NhanVienYTe.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _nVYTService.GetByIdAsNoTracking(id);

                if (t.HoTenNVYTe != NVYT_VM.NhanVienYTe.HoTenNVYTe)
                {
                    temp += String.Format("- HoTenNVYTe thay đổi: {0}->{1}", t.HoTenNVYTe, NVYT_VM.NhanVienYTe.HoTenNVYTe);
                }

                if (t.SDT_NVYT != NVYT_VM.NhanVienYTe.SDT_NVYT)
                {
                    temp += String.Format("- SDT_NVYT thay đổi: {0}->{1}", t.SDT_NVYT, NVYT_VM.NhanVienYTe.SDT_NVYT);
                }

                if (t.DonVi != NVYT_VM.NhanVienYTe.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, NVYT_VM.NhanVienYTe.DonVi);
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
                    NVYT_VM.NhanVienYTe.LogFile = t.LogFile;
                }

                try
                {
                    await _nVYTService.UpdateAsync(NVYT_VM.NhanVienYTe);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(NVYT_VM);
                }
            }
            // not valid

            return View(NVYT_VM);
        }

        public IActionResult SearchNVYTs_Code_Edit(string code)
        {
            code ??= "";
            NVYT_VM.IEnumNVYT = _nVYTService.SearchNVYTs_Code(code);
            NVYT_VM.MaNVYTText = code;
            return PartialView(NVYT_VM);
        }

        public IActionResult SearchNVYTs_Code(string code)
        {
            code ??= "";
            NVYT_VM.IEnumNVYT = _nVYTService.SearchNVYTs_Code(code);
            NVYT_VM.MaNVYTText = code;
            return PartialView(NVYT_VM);
        }

        public JsonResult GetThietBis_By_Code(string code)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            NhanVienYTe nhanVienYTe = _nVYTService.GetNVYTByCode(code);
            if (nhanVienYTe != null)
            {
                return Json(new
                {
                    status = true,
                    data = nhanVienYTe
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}