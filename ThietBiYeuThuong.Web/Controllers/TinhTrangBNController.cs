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
    public class TinhTrangBNController : BaseController
    {
        private readonly ITinhTrangBNService _tinhTrangBNService;
        private readonly IBenhNhanService _benhNhanService;

        [BindProperty]
        public TinhTrangBNViewModel TinhTrangBNVM { get; set; }

        public TinhTrangBNController(ITinhTrangBNService tinhTrangBNService, IBenhNhanService benhNhanService)
        {
            TinhTrangBNVM = new TinhTrangBNViewModel()
            {
                TinhTrangBN = new Data.Models.TinhTrangBN()
            };
            _tinhTrangBNService = tinhTrangBNService;
            _benhNhanService = benhNhanService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TinhTrangBNPartial(string benhNhanId, int page)
        {
            // TinhTrangBNVM
            TinhTrangBNVM.Page = page;
            TinhTrangBNVM.TinhTrangBNs = await _tinhTrangBNService.List_TinhTrang_By_BenhNhanId(benhNhanId);
            TinhTrangBNVM.BenhNhan = await _benhNhanService.GetById(benhNhanId);

            return PartialView(TinhTrangBNVM);
        }

        public async Task<IActionResult> TinhTrangBN_Create_Partial(string benhNhanId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            TinhTrangBNVM.BenhNhan = await _benhNhanService.GetById(benhNhanId);

            return PartialView(TinhTrangBNVM);
        }

        public async Task<IActionResult> TinhTrangBN_Create_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                // not valid

                TinhTrangBNVM.BenhNhan = await _benhNhanService.GetById(TinhTrangBNVM.BenhNhan.MaBN);
                TinhTrangBNVM.Page = TinhTrangBNVM.Page;

                return View(TinhTrangBNVM);
            }

            // TinhTrangBNVM.CTPhieuNX.PhieuNXId = TinhTrangBNVM.PhieuNX.SoPhieu;
            TinhTrangBNVM.TinhTrangBN.NguoiTao = user.Username;
            TinhTrangBNVM.TinhTrangBN.NgayTao = DateTime.Now;

            // ghi log
            TinhTrangBNVM.TinhTrangBN.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _tinhTrangBNService.CreateAsync(TinhTrangBNVM.TinhTrangBN);

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

        public async Task<IActionResult> TinhTrangBN_Edit_Partial(string benhNhanId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Tình trạng này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TinhTrangBNVM.BenhNhan = await _benhNhanService.GetById(benhNhanId);
            TinhTrangBNVM.TinhTrangBN = await _tinhTrangBNService.GetById(id);

            if (TinhTrangBNVM.BenhNhan == null)
            {
                ViewBag.ErrorMessage = "Tình trạng này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(TinhTrangBNVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> TinhTrangBN_Edit_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            //if (id != TinhTrangBNVM.PhieuNX.SoPhieu)
            //{
            //    ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
            //    return View("~/Views/Shared/NotFound.cshtml");
            //}

            if (ModelState.IsValid)
            {
                TinhTrangBNVM.BenhNhan.NgaySua = DateTime.Now;
                TinhTrangBNVM.BenhNhan.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _tinhTrangBNService.GetByIdAsNoTracking(TinhTrangBNVM.TinhTrangBN.Id);

                if (t.TinhTrang != TinhTrangBNVM.TinhTrangBN.TinhTrang)
                {
                    temp += String.Format("- TinhTrang thay đổi: {0}->{1}", t.TinhTrang, TinhTrangBNVM.TinhTrangBN.TinhTrang);
                }

                if (t.BenhNenBN != TinhTrangBNVM.TinhTrangBN.BenhNenBN)
                {
                    temp += String.Format("- BenhNenBN thay đổi: {0}->{1}", t.BenhNenBN, TinhTrangBNVM.TinhTrangBN.BenhNenBN);
                }

                if (t.ChiSoSPO2 != TinhTrangBNVM.TinhTrangBN.ChiSoSPO2)
                {
                    temp += String.Format("- ChiSoSPO2 thay đổi: {0}->{1}", t.ChiSoSPO2, TinhTrangBNVM.TinhTrangBN.ChiSoSPO2);
                }

                if (t.TinhTrangBNSauO2 != TinhTrangBNVM.TinhTrangBN.TinhTrangBNSauO2)
                {
                    temp += String.Format("- TinhTrangBNSauO2 thay đổi: {0}->{1}", t.TinhTrangBNSauO2, TinhTrangBNVM.TinhTrangBN.TinhTrangBNSauO2);
                }

                if (t.KetLuan != TinhTrangBNVM.TinhTrangBN.KetLuan)
                {
                    temp += String.Format("- KetLuan thay đổi: {0}->{1}", t.KetLuan, TinhTrangBNVM.TinhTrangBN.KetLuan);
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
                    TinhTrangBNVM.TinhTrangBN.LogFile = t.LogFile;
                }

                try
                {
                    await _tinhTrangBNService.UpdateAsync(TinhTrangBNVM.TinhTrangBN);

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

            return View(TinhTrangBNVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Tình trạng này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var tinhTrangBN = await _tinhTrangBNService.GetById(id);
            if (tinhTrangBN == null)
                return NotFound();
            try
            {
                await _tinhTrangBNService.DeleteAsync(tinhTrangBN);

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