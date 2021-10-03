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
    public class CTPhieuController : BaseController
    {
        private readonly IPhieuNhapService _phieuNhapService;
        private readonly ICTPhieuService _cTPhieuService;

        [BindProperty]
        public CTPhieuViewModel CTPhieuVM { get; set; }

        public CTPhieuController(IPhieuNhapService phieuNhapService, ICTPhieuService cTPhieuService)
        {
            _phieuNhapService = phieuNhapService;
            _cTPhieuService = cTPhieuService;
            CTPhieuVM = new CTPhieuViewModel()
            {
                CTPhieu = new Data.Models.CTPhieu(),
                PhieuNhap = new Data.Models.PhieuNhap()
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CTPhieuPartial(string phieuNhapId, int page)
        {
            // CTPhieuVM
            CTPhieuVM.Page = page;
            CTPhieuVM.CTPhieus = await _cTPhieuService.List_CTPhieu_By_PhieuNhapId(phieuNhapId);
            CTPhieuVM.PhieuNhap = await _phieuNhapService.GetById(phieuNhapId);

            return PartialView(CTPhieuVM);
        }

        public async Task<IActionResult> CTPhieu_Create_Partial(string phieuNhapId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            CTPhieuVM.PhieuNhap = await _phieuNhapService.GetById(phieuNhapId);

            return PartialView(CTPhieuVM);
        }

        public async Task<IActionResult> CTPhieu_Create_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                // not valid

                CTPhieuVM.PhieuNhap = await _phieuNhapService.GetById(CTPhieuVM.PhieuNhap.SoPhieu);
                CTPhieuVM.Page = CTPhieuVM.Page;

                return View(CTPhieuVM);
            }

            // CTPhieuVM.CTPhieuNX.PhieuNXId = CTPhieuVM.PhieuNX.SoPhieu;
            CTPhieuVM.CTPhieu.LapPhieu = user.Username;
            CTPhieuVM.CTPhieu.NgayTao = DateTime.Now;

            // ghi log
            CTPhieuVM.CTPhieu.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _cTPhieuService.CreateAsync(CTPhieuVM.CTPhieu);

                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = "Lỗi thêm tình trạng BN:" + ex.Message
                });
            }
        }

        public async Task<IActionResult> CTPhieu_Edit_Partial(string phieuNhapId, string id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Chi tiết này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            CTPhieuVM.PhieuNhap = await _phieuNhapService.GetById(phieuNhapId);
            CTPhieuVM.CTPhieu = await _cTPhieuService.GetById(id);

            if (CTPhieuVM.PhieuNhap == null)
            {
                ViewBag.ErrorMessage = "Chi tiết này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(CTPhieuVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CTPhieu_Edit_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            //if (id != CTPhieuVM.PhieuNX.SoPhieu)
            //{
            //    ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
            //    return View("~/Views/Shared/NotFound.cshtml");
            //}

            if (ModelState.IsValid)
            {
                CTPhieuVM.PhieuNhap.NgaySua = DateTime.Now;
                CTPhieuVM.PhieuNhap.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _cTPhieuService.GetByIdAsNoTracking(CTPhieuVM.CTPhieu.SoPhieuCT);

                if (t.ThietBiId != CTPhieuVM.CTPhieu.ThietBiId)
                {
                    temp += String.Format("- ThietBiId thay đổi: {0}->{1}", t.ThietBiId, CTPhieuVM.CTPhieu.ThietBiId);
                }

                if (t.DongHoGiao != CTPhieuVM.CTPhieu.DongHoGiao)
                {
                    temp += String.Format("- DongHoGiao thay đổi: {0}->{1}", t.DongHoGiao, CTPhieuVM.CTPhieu.DongHoGiao);
                }

                if (t.DongHoThu != CTPhieuVM.CTPhieu.DongHoThu)
                {
                    temp += String.Format("- DongHoThu thay đổi: {0}->{1}", t.DongHoThu, CTPhieuVM.CTPhieu.DongHoThu);
                }

                if (t.NVGiaoBinh != CTPhieuVM.CTPhieu.NVGiaoBinh)
                {
                    temp += String.Format("- NVGiaoBinh thay đổi: {0}->{1}", t.NVGiaoBinh, CTPhieuVM.CTPhieu.NVGiaoBinh);
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
                    CTPhieuVM.CTPhieu.LogFile = t.LogFile;
                }

                try
                {
                    await _cTPhieuService.UpdateAsync(CTPhieuVM.CTPhieu);

                    return Json(new
                    {
                        status = true
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Lỗi cập nhật"
                    });
                }
            }
            // not valid

            return View(CTPhieuVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Chi tiết này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var cTPhieu = await _cTPhieuService.GetById(id);
            if (cTPhieu == null)
                return NotFound();
            try
            {
                await _cTPhieuService.DeleteAsync(cTPhieu);

                //SetAlert("Xóa thành công.", "success");
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                //SetAlert(ex.Message, "error");

                return Json(new
                {
                    status = false
                });
            }
        }
    }
}