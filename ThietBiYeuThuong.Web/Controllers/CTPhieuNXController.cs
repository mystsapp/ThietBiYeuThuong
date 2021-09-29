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
    public class CTPhieuNXController : BaseController
    {
        private readonly ICTPhieuNXService _cTPhieuNXService;
        private readonly IPhieuNXService _phieuNXService;
        private readonly ITinhTonService _tinhTonService;

        [BindProperty]
        public CTPhieuNXViewModel CTPhieuNXVM { get; set; }

        public CTPhieuNXController(ICTPhieuNXService cTPhieuNXService, IPhieuNXService phieuNXService, ITinhTonService tinhTonService)
        {
            _cTPhieuNXService = cTPhieuNXService;
            _phieuNXService = phieuNXService;
            _tinhTonService = tinhTonService;
            CTPhieuNXVM = new CTPhieuNXViewModel()
            {
                CTPhieuNX = new Data.Models.CTPhieuNX(),
                PhieuNX = new Data.Models.PhieuNX()
            };
        }

        public async Task<IActionResult> CTPhieuNXPartial(string PhieuNXId, int page)
        {
            // CTPhieuNXVM
            CTPhieuNXVM.Page = page;
            CTPhieuNXVM.CTPhieuNXes = await _cTPhieuNXService.List_CTPhieuNX_By_PhieuNXId(PhieuNXId);
            CTPhieuNXVM.PhieuNX = await _phieuNXService.GetById(PhieuNXId);

            return PartialView(CTPhieuNXVM);
        }

        public async Task<IActionResult> CTPhieuNX_Create_Partial(string PhieuNXId, string strUrl, int page, long id_Dong_Da_Click)
        {
            if (!ModelState.IsValid) // check id_Dong_Da_Click valid (da gang' = 0 trong home/index)
            {
                return View();
            }

            CTPhieuNXVM.PhieuNX = await _phieuNXService.GetById(PhieuNXId);

            CTPhieuNXVM.StrUrl = strUrl;
            CTPhieuNXVM.Page = page; // page for redirect

            //// btnThemdong + copy dong da click
            //if (id_Dong_Da_Click > 0)
            //{
            //    var dongCu = await _kVCTPTCService.GetById(id_Dong_Da_Click);
            //    KVCTPCTVM.KVCTPTC = dongCu;
            //}

            return PartialView(CTPhieuNXVM);
        }

        public async Task<IActionResult> CTPhieuNX_Create_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                // not valid

                CTPhieuNXVM.PhieuNX = await _phieuNXService.GetById(CTPhieuNXVM.PhieuNX.SoPhieu);
                CTPhieuNXVM.Page = CTPhieuNXVM.Page;

                return View(CTPhieuNXVM);
            }

            // CTPhieuNXVM.CTPhieuNX.PhieuNXId = CTPhieuNXVM.PhieuNX.SoPhieu;
            CTPhieuNXVM.CTPhieuNX.LapPhieu = user.Username;
            CTPhieuNXVM.CTPhieuNX.NgayNhap = DateTime.Now;
            CTPhieuNXVM.CTPhieuNX.NgayTao = DateTime.Now;

            int tongNhap = 0, tongXuat = 0, ton = 0;
            // next sophieuct --> bat buoc phai co'
            switch (CTPhieuNXVM.PhieuNX.LoaiPhieu)
            {
                case "PN": // nhap
                    CTPhieuNXVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CN");
                    break;

                default: // xuat
                    {
                        CTPhieuNXVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CX");

                        // tinh ton
                        var tinhTon = _tinhTonService.GetLast("", DateTime.Now.ToShortDateString());
                        var tonDau = tinhTon == null ? 0 : tinhTon.SoLuongTon;
                        var cTPhieuNXes = await _cTPhieuNXService.GetCTTrongNgay();
                        //if (cTPhieuNXes == null || cTPhieuNXes.Count == 0)
                        //{
                        //    tongNhap = 0; tongXuat = 0; ton = 0;
                        //}
                        //else
                        //{
                        tongNhap = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PN"))
                                              .Sum(x => x.SoLuong);
                        tongXuat = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PX"))
                                                  .Sum(x => x.SoLuong);
                        ton = tonDau + tongNhap - tongXuat;
                        //}

                        if (CTPhieuNXVM.CTPhieuNX.SoLuong > ton)
                        {
                            return Json(new
                            {
                                status = false,
                                message = $"Số lượng trong kho không đủ({ton})"
                            });
                        }
                    }
                    break;
            }
            // next sophieuct

            // ghi log
            CTPhieuNXVM.CTPhieuNX.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _cTPhieuNXService.Create(CTPhieuNXVM.CTPhieuNX);

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
                    message = "Lỗi thêm CT phiếu" + ex.Message
                });
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            CTPhieuNXVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            CTPhieuNXVM.PhieuNX = await _phieuNXService.GetById(id);

            if (CTPhieuNXVM.PhieuNX == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            //CTPhieuNXVM.ListGT = ListGT();
            //CTPhieuNXVM.ListLoaiPhieu = ListLoaiPhieu();

            return View(CTPhieuNXVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != CTPhieuNXVM.PhieuNX.SoPhieu)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                CTPhieuNXVM.PhieuNX.NgaySua = DateTime.Now;
                CTPhieuNXVM.PhieuNX.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _phieuNXService.GetByIdAsNoTracking(id);

                if (t.HoTenTN != CTPhieuNXVM.PhieuNX.HoTenTN)
                {
                    temp += String.Format("- HoTenTN thay đổi: {0}->{1}", t.HoTenTN, CTPhieuNXVM.PhieuNX.HoTenTN);
                }

                if (t.SDT_TN != CTPhieuNXVM.PhieuNX.SDT_TN)
                {
                    temp += String.Format("- SDT_TN thay đổi: {0}->{1}", t.SDT_TN, CTPhieuNXVM.PhieuNX.SDT_TN);
                }

                if (t.GT_TN != CTPhieuNXVM.PhieuNX.GT_TN)
                {
                    temp += String.Format("- GT_TN thay đổi: {0}->{1}", t.GT_TN, CTPhieuNXVM.PhieuNX.GT_TN);
                }

                if (t.HoTenBN != CTPhieuNXVM.PhieuNX.HoTenBN)
                {
                    temp += String.Format("- HoTenBN thay đổi: {0}->{1}", t.HoTenBN, CTPhieuNXVM.PhieuNX.HoTenBN);
                }

                if (t.NamSinh != CTPhieuNXVM.PhieuNX.NamSinh)
                {
                    temp += String.Format("- NamSinh thay đổi: {0}->{1}", t.NamSinh, CTPhieuNXVM.PhieuNX.NamSinh);
                }

                if (t.CMND_CCCD_BN != CTPhieuNXVM.PhieuNX.CMND_CCCD_BN)
                {
                    temp += String.Format("- CMND_CCCD_BN thay đổi: {0}->{1}", t.CMND_CCCD_BN, CTPhieuNXVM.PhieuNX.CMND_CCCD_BN);
                }

                if (t.DiaChi != CTPhieuNXVM.PhieuNX.DiaChi)
                {
                    temp += String.Format("- DiaChi thay đổi: {0}->{1}", t.DiaChi, CTPhieuNXVM.PhieuNX.DiaChi);
                }

                if (t.HoTenNVYTe != CTPhieuNXVM.PhieuNX.HoTenNVYTe)
                {
                    temp += String.Format("- HoTenNVYTe thay đổi: {0}->{1}", t.HoTenNVYTe, CTPhieuNXVM.PhieuNX.HoTenNVYTe);
                }

                if (t.SDT_NVYT != CTPhieuNXVM.PhieuNX.SDT_NVYT)
                {
                    temp += String.Format("- SDT_NVYT thay đổi: {0}->{1}", t.SDT_NVYT, CTPhieuNXVM.PhieuNX.SDT_NVYT);
                }

                if (t.DonVi != CTPhieuNXVM.PhieuNX.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, CTPhieuNXVM.PhieuNX.DonVi);
                }

                if (t.TinhTrangBN != CTPhieuNXVM.PhieuNX.TinhTrangBN)
                {
                    temp += String.Format("- TinhTrangBN thay đổi: {0}->{1}", t.TinhTrangBN, CTPhieuNXVM.PhieuNX.TinhTrangBN);
                }

                if (t.BenhNenBN != CTPhieuNXVM.PhieuNX.BenhNenBN)
                {
                    temp += String.Format("- BenhNenBN thay đổi: {0}->{1}", t.BenhNenBN, CTPhieuNXVM.PhieuNX.BenhNenBN);
                }

                if (t.ChiSoSPO2 != CTPhieuNXVM.PhieuNX.ChiSoSPO2)
                {
                    temp += String.Format("- ChiSoSPO2 thay đổi: {0}->{1}", t.ChiSoSPO2, CTPhieuNXVM.PhieuNX.ChiSoSPO2);
                }

                if (t.TinhTrangBNSauO2 != CTPhieuNXVM.PhieuNX.TinhTrangBNSauO2)
                {
                    temp += String.Format("- TinhTrangBNSauO2 thay đổi: {0}->{1}", t.TinhTrangBNSauO2, CTPhieuNXVM.PhieuNX.TinhTrangBNSauO2);
                }

                if (t.KetLuan != CTPhieuNXVM.PhieuNX.KetLuan)
                {
                    temp += String.Format("- KetLuan thay đổi: {0}->{1}", t.KetLuan, CTPhieuNXVM.PhieuNX.KetLuan);
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
                    CTPhieuNXVM.PhieuNX.LogFile = t.LogFile;
                }

                try
                {
                    await _phieuNXService.UpdateAsync(CTPhieuNXVM.PhieuNX);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(CTPhieuNXVM);
                }
            }
            // not valid

            return View(CTPhieuNXVM);
        }

        public IActionResult DetailsRedirect(string strUrl/*, string tabActive*/)
        {
            //if (!string.IsNullOrEmpty(tabActive))
            //{
            //    strUrl = strUrl + "&tabActive=" + tabActive; // for redirect tab
            //}
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var kVCTPCT = await _cTPhieuNXService.GetById(id);
            if (kVCTPCT == null)
                return NotFound();
            try
            {
                await _cTPhieuNXService.DeleteAsync(kVCTPCT);

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

        public IActionResult BackIndex(string phieuNXId, int page)
        {
            return RedirectToAction(nameof(Index), "PhieuNX", new { id = phieuNXId, page });
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
    }
}