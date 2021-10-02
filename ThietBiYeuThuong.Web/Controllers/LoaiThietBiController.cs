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
    public class LoaiThietBiController : BaseController
    {
        private readonly IPhieuNXService _phieuNXService;
        private readonly ICTPhieuNXService _cTPhieuNXService;
        private readonly ILoaiThietBiService _loaiThietBiService;

        [BindProperty]
        public LoaiThietBiViewModel LoaiVM { get; set; }

        public LoaiThietBiController(IPhieuNXService phieuNXService, ICTPhieuNXService cTPhieuNXService, ILoaiThietBiService loaiThietBiService)
        {
            _phieuNXService = phieuNXService;
            _cTPhieuNXService = cTPhieuNXService;
            _loaiThietBiService = loaiThietBiService;
            LoaiVM = new LoaiThietBiViewModel()
            {
                LoaiThietBi = new Data.Models.LoaiThietBi()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, int id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            LoaiVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            LoaiVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                LoaiVM.LoaiThietBi = await _loaiThietBiService.GetById(id);
                ViewBag.id = LoaiVM.LoaiThietBi.Id;
            }
            else
            {
                LoaiVM.LoaiThietBi = new Data.Models.LoaiThietBi();
            }
            LoaiVM.LoaiThietBis = await _loaiThietBiService.ListLoaiThietBi(searchString, searchFromDate, searchToDate, page);
            return View(LoaiVM);
        }

        public IActionResult Create(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            LoaiVM.StrUrl = strUrl;

            return View(LoaiVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                LoaiVM = new LoaiThietBiViewModel()
                {
                    LoaiThietBi = new LoaiThietBi(),
                    StrUrl = strUrl
                };

                return View(LoaiVM);
            }

            LoaiVM.LoaiThietBi.NgayTao = DateTime.Now;
            LoaiVM.LoaiThietBi.NguoiTao = user.Username;

            // ghi log
            LoaiVM.LoaiThietBi.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _loaiThietBiService.CreateAsync(LoaiVM.LoaiThietBi); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = LoaiVM.LoaiThietBi.Id, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(LoaiVM);
            }
        }

        public async Task<IActionResult> Edit(int id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            LoaiVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Loại này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            LoaiVM.LoaiThietBi = await _loaiThietBiService.GetById(id);

            if (LoaiVM.LoaiThietBi == null)
            {
                ViewBag.ErrorMessage = "Loại này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(LoaiVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != LoaiVM.LoaiThietBi.Id)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                LoaiVM.LoaiThietBi.NgaySua = DateTime.Now;
                LoaiVM.LoaiThietBi.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _loaiThietBiService.GetByIdAsNoTracking(id);

                if (t.Name != LoaiVM.LoaiThietBi.Name)
                {
                    temp += String.Format("- Name thay đổi: {0}->{1}", t.Name, LoaiVM.LoaiThietBi.Name);
                }

                if (t.Descripttion != LoaiVM.LoaiThietBi.Descripttion)
                {
                    temp += String.Format("- Descripttion thay đổi: {0}->{1}", t.Descripttion, LoaiVM.LoaiThietBi.Descripttion);
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
                    LoaiVM.LoaiThietBi.LogFile = t.LogFile;
                }

                try
                {
                    await _loaiThietBiService.UpdateAsync(LoaiVM.LoaiThietBi);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(LoaiVM);
                }
            }
            // not valid

            return View(LoaiVM);
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