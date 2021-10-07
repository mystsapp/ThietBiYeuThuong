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
        private readonly ILoaiThietBiService _loaiThietBiService;
        private readonly IThietBiService _thietBiService;

        [BindProperty]
        public CTPhieuViewModel CTPhieuVM { get; set; }

        public CTPhieuController(IPhieuNhapService phieuNhapService,
                                 ICTPhieuService cTPhieuService,
                                 ILoaiThietBiService loaiThietBiService,
                                 IThietBiService thietBiService)
        {
            _phieuNhapService = phieuNhapService;
            _cTPhieuService = cTPhieuService;
            _loaiThietBiService = loaiThietBiService;
            _thietBiService = thietBiService;
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
            CTPhieuVM.LoaiThietBis = await _loaiThietBiService.GetAll();

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

            // save TB
            CTPhieuVM.ThietBi.MaTB = _thietBiService.GetMaTB("TB");
            var thietBi = new ThietBi()
            {
                MaTB = _thietBiService.GetMaTB("TB"),
                TenTB = CTPhieuVM.ThietBi.TenTB,
                LoaiTBId = CTPhieuVM.ThietBi.LoaiTBId,
                TrangThaiId = 1, // đầy
                NgayTao = DateTime.Now,
                NguoiTao = user.Username,
                LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString() // user.Username
            };
            await _thietBiService.CreateAsync(thietBi); // save thietbi

            // save ctphieu
            CTPhieuVM.CTPhieu.LapPhieu = user.Username;
            CTPhieuVM.CTPhieu.NgayTao = DateTime.Now;
            CTPhieuVM.CTPhieu.ThietBiId = thietBi.MaTB;
            CTPhieuVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");

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
            CTPhieuVM.ThietBi = await _thietBiService.GetById(CTPhieuVM.CTPhieu.ThietBiId);
            CTPhieuVM.LoaiThietBis = await _loaiThietBiService.GetAll();

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

            string temp = "", log = "", temp1 = "", log1 = "";

            //if (id != CTPhieuVM.PhieuNX.SoPhieu)
            //{
            //    ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
            //    return View("~/Views/Shared/NotFound.cshtml");
            //}

            if (ModelState.IsValid)
            {
                // sua thietbi
                CTPhieuVM.ThietBi.NgaySua = DateTime.Now;
                CTPhieuVM.ThietBi.NguoiSua = user.Username;
                // log
                var t1 = _thietBiService.GetByIdAsNoTracking(CTPhieuVM.ThietBi.MaTB);

                if (t1.TenTB != CTPhieuVM.ThietBi.TenTB)
                {
                    temp1 += String.Format("- TenTB thay đổi: {0}->{1}", t1.TenTB, CTPhieuVM.ThietBi.TenTB);
                }

                if (t1.TrangThaiId != CTPhieuVM.ThietBi.TrangThaiId)
                {
                    temp1 += String.Format("- TrangThaiId thay đổi: {0}->{1}", t1.TrangThaiId, CTPhieuVM.ThietBi.TrangThaiId);
                }

                if (t1.TinhTrang != CTPhieuVM.ThietBi.TinhTrang)
                {
                    temp1 += String.Format("- TinhTrang thay đổi: {0}->{1}", t1.TinhTrang, CTPhieuVM.ThietBi.TinhTrang);
                }

                if (t1.LoaiTBId != CTPhieuVM.ThietBi.LoaiTBId)
                {
                    temp1 += String.Format("- LoaiTBId thay đổi: {0}->{1}", t1.LoaiTBId, CTPhieuVM.ThietBi.LoaiTBId);
                }

                // kiem tra thay doi
                if (temp1.Length > 0)
                {
                    log1 = System.Environment.NewLine;
                    log1 += "=============";
                    log1 += System.Environment.NewLine;
                    log1 += temp1 + " -User cập nhật tour: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t1.LogFile = t1.LogFile + log1;
                    CTPhieuVM.ThietBi.LogFile = t1.LogFile;
                }

                try
                {
                    await _thietBiService.UpdateAsync(CTPhieuVM.ThietBi);
                }
                catch (Exception ex)
                {
                    throw;
                }

                //// save ctphieu
                //CTPhieuVM.PhieuNhap.NgaySua = DateTime.Now;
                //CTPhieuVM.PhieuNhap.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

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
            var thietBi = await _thietBiService.GetById(cTPhieu.ThietBiId);

            if (cTPhieu == null || thietBi == null)
                return NotFound();
            try
            {
                await _cTPhieuService.DeleteAsync(cTPhieu);
                await _thietBiService.DeleteAsync(thietBi);

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