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
    public class CTHoSoBNController : BaseController
    {
        private readonly ICTHoSoBNService _cTHoSoBNService;
        private readonly IHoSoBNService _hoSoBNService;
        private readonly ITinhTonService _tinhTonService;
        private readonly IThietBiService _thietBiService;
        private readonly IBenhNhanThietBiService _benhNhanThietBiService;

        [BindProperty]
        public CTHoSoBNViewModel CTHoSoBNVM { get; set; }

        public CTHoSoBNController(ICTHoSoBNService cTHoSoBNService,
                                  IHoSoBNService hoSoBNService,
                                  ITinhTonService tinhTonService,
                                  IThietBiService thietBiService,
                                  IBenhNhanThietBiService benhNhanThietBiService)
        {
            _cTHoSoBNService = cTHoSoBNService;
            _hoSoBNService = hoSoBNService;
            _tinhTonService = tinhTonService;
            _thietBiService = thietBiService;
            _benhNhanThietBiService = benhNhanThietBiService;
            CTHoSoBNVM = new CTHoSoBNViewModel()
            {
                CTHoSoBN = new Data.Models.CTHoSoBN(),
                HoSoBN = new Data.Models.HoSoBN()
            };
        }

        public async Task<IActionResult> CTHoSoBNPartial(string hoSoBNId, int page)
        {
            // CTHoSoBNVM
            CTHoSoBNVM.Page = page;
            CTHoSoBNVM.CTHoSoBNs = await _cTHoSoBNService.List_CTHoSoBN_By_HoSoBNId(hoSoBNId);
            CTHoSoBNVM.HoSoBN = await _hoSoBNService.GetById(hoSoBNId);

            return PartialView(CTHoSoBNVM);
        }

        public async Task<IActionResult> CTHoSoBN_Create_Partial(string hoSoBNId, string strUrl, int page, long id_Dong_Da_Click)
        {
            if (!ModelState.IsValid) // check id_Dong_Da_Click valid (da gang' = 0 trong home/index)
            {
                return View();
            }

            CTHoSoBNVM.HoSoBN = await _hoSoBNService.GetById(hoSoBNId);

            CTHoSoBNVM.StrUrl = strUrl;
            CTHoSoBNVM.Page = page; // page for redirect

            //// btnThemdong + copy dong da click
            //if (id_Dong_Da_Click > 0)
            //{
            //    var dongCu = await _kVCTPTCService.GetById(id_Dong_Da_Click);
            //    KVCTPCTVM.KVCTPTC = dongCu;
            //}

            return PartialView(CTHoSoBNVM);
        }

        public async Task<IActionResult> CTHoSoBN_Create_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            CTHoSoBNVM.HoSoBN = await _hoSoBNService.GetById(CTHoSoBNVM.HoSoBN.SoPhieu);

            if (!ModelState.IsValid)
            {
                // not valid

                CTHoSoBNVM.Page = CTHoSoBNVM.Page;

                return View(CTHoSoBNVM);
            }

            // CTHoSoBNVM.CTPhieuNX.PhieuNXId = CTHoSoBNVM.PhieuNX.SoPhieu;
            CTHoSoBNVM.CTHoSoBN.LapPhieu = user.Username;
            CTHoSoBNVM.CTHoSoBN.NgayTao = DateTime.Now;
            CTHoSoBNVM.CTHoSoBN.NgayXuat = DateTime.Now;
            CTHoSoBNVM.CTHoSoBN.SoPhieuCT = _cTHoSoBNService.GetSoPhieuCT("CH");

            //int tongNhap = 0, tongXuat = 0, ton = 0;
            //// next sophieuct --> bat buoc phai co'
            //switch (CTHoSoBNVM.PhieuNX.LoaiPhieu)
            //{
            //    case "PN": // nhap
            //        CTHoSoBNVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CN");
            //        break;

            //    default: // xuat
            //        {
            //            CTHoSoBNVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CX");

            //            // tinh ton
            //            var tinhTon = _tinhTonService.GetLast("", DateTime.Now.ToShortDateString());
            //            var tonDau = tinhTon == null ? 0 : tinhTon.SoLuongTon;
            //            var cTPhieuNXes = await _cTPhieuNXService.GetCTTrongNgay();
            //            //if (cTPhieuNXes == null || cTPhieuNXes.Count == 0)
            //            //{
            //            //    tongNhap = 0; tongXuat = 0; ton = 0;
            //            //}
            //            //else
            //            //{
            //            tongNhap = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PN"))
            //                                  .Sum(x => x.SoLuong);
            //            tongXuat = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PX"))
            //                                      .Sum(x => x.SoLuong);
            //            ton = tonDau + tongNhap - tongXuat;
            //            //}

            //            if (CTHoSoBNVM.CTPhieuNX.SoLuong > ton)
            //            {
            //                return Json(new
            //                {
            //                    status = false,
            //                    message = $"Số lượng trong kho không đủ({ton})"
            //                });
            //            }
            //        }
            //        break;
            //}
            //// next sophieuct

            // ghi log
            CTHoSoBNVM.CTHoSoBN.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _cTHoSoBNService.Create(CTHoSoBNVM.CTHoSoBN);

                // update thietbi -> tinhtrang == false
                ThietBi thietBi = await _thietBiService.GetById(CTHoSoBNVM.CTHoSoBN.ThietBiId);
                thietBi.TinhTrang = false;
                thietBi.LogFile += "\n" + " -User xuất: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString()
                                                   + ", " + "BN: " + CTHoSoBNVM.HoSoBN.BenhNhanId; // username
                await _thietBiService.UpdateAsync(thietBi);

                // save BenhNhanThietBi
                BenhNhanThietBi benhNhanThietBi = new BenhNhanThietBi()
                {
                    BenhNhanId = CTHoSoBNVM.HoSoBN.BenhNhanId,
                    ThietBiId = CTHoSoBNVM.CTHoSoBN.ThietBiId,
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Username,
                    CTHoSoBNId = CTHoSoBNVM.CTHoSoBN.SoPhieuCT
                };
                await _benhNhanThietBiService.CreateAsync(benhNhanThietBi);

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
                    message = "Lỗi thêm CT hồ sơ" + ex.Message
                });
            }
        }

        public async Task<IActionResult> CTHoSoBN_Edit_Partial(string phieunxid, string id, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            CTHoSoBNVM.Page = page;

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            CTHoSoBNVM.HoSoBN = await _hoSoBNService.GetById(phieunxid);
            CTHoSoBNVM.CTHoSoBN = await _cTHoSoBNService.GetById(id);

            if (CTHoSoBNVM.HoSoBN == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            //CTHoSoBNVM.ListGT = ListGT();
            //CTHoSoBNVM.ListLoaiPhieu = ListLoaiPhieu();

            return PartialView(CTHoSoBNVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CTHoSoBN_Edit_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            //if (id != CTHoSoBNVM.PhieuNX.SoPhieu)
            //{
            //    ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
            //    return View("~/Views/Shared/NotFound.cshtml");
            //}

            if (ModelState.IsValid)
            {
                CTHoSoBNVM.HoSoBN.NgaySua = DateTime.Now;
                CTHoSoBNVM.HoSoBN.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _cTHoSoBNService.GetByIdAsNoTracking(CTHoSoBNVM.CTHoSoBN.SoPhieuCT);

                if (t.ThietBiId != CTHoSoBNVM.CTHoSoBN.ThietBiId)
                {
                    temp += String.Format("- ThietBiId thay đổi: {0}->{1}", t.ThietBiId, CTHoSoBNVM.CTHoSoBN.ThietBiId);
                }

                if (t.DongHoGiao != CTHoSoBNVM.CTHoSoBN.DongHoGiao)
                {
                    temp += String.Format("- DongHoGiao thay đổi: {0}->{1}", t.DongHoGiao, CTHoSoBNVM.CTHoSoBN.DongHoGiao);
                }

                if (t.DongHoThu != CTHoSoBNVM.CTHoSoBN.DongHoThu)
                {
                    temp += String.Format("- DongHoThu thay đổi: {0}->{1}", t.DongHoThu, CTHoSoBNVM.CTHoSoBN.DongHoThu);
                }

                if (t.SoLuong != CTHoSoBNVM.CTHoSoBN.SoLuong)
                {
                    temp += String.Format("- SoLuong thay đổi: {0}->{1}", t.SoLuong, CTHoSoBNVM.CTHoSoBN.SoLuong);
                }

                if (t.NVGiaoBinh != CTHoSoBNVM.CTHoSoBN.NVGiaoBinh)
                {
                    temp += String.Format("- NVGiaoBinh thay đổi: {0}->{1}", t.NVGiaoBinh, CTHoSoBNVM.CTHoSoBN.NVGiaoBinh);
                }

                if (t.GhiChu != CTHoSoBNVM.CTHoSoBN.GhiChu)
                {
                    temp += String.Format("- GhiChu thay đổi: {0}->{1}", t.GhiChu, CTHoSoBNVM.CTHoSoBN.GhiChu);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.LogFile = t.LogFile + log;
                    CTHoSoBNVM.CTHoSoBN.LogFile = t.LogFile;
                }

                try
                {
                    await _cTHoSoBNService.UpdateAsync(CTHoSoBNVM.CTHoSoBN);

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

            return View(CTHoSoBNVM);
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
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (string.IsNullOrEmpty(id))
                return NotFound();

            var ctHoSoBN = await _cTHoSoBNService.GetById(id);
            if (ctHoSoBN == null)
                return NotFound();
            try
            {
                await _cTHoSoBNService.DeleteAsync(ctHoSoBN);

                // capnhat tinhtrang thietbi = true -> con hàng
                var thietBi = await _thietBiService.GetById(ctHoSoBN.ThietBiId);
                thietBi.TinhTrang = true;
                thietBi.LogFile += "\n" + " -User xoá CT: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString()
                                                   + ", " + "BN: " + CTHoSoBNVM.HoSoBN.BenhNhanId; // username
                await _thietBiService.UpdateAsync(thietBi);

                // save BenhNhanThietBi
                BenhNhanThietBi benhNhanThietBi = new BenhNhanThietBi()
                {
                    BenhNhanId = CTHoSoBNVM.HoSoBN.BenhNhanId,
                    ThietBiId = CTHoSoBNVM.CTHoSoBN.ThietBiId,
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Username
                };
                await _benhNhanThietBiService.DeleteAsync(benhNhanThietBi);

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
            return RedirectToAction(nameof(Index), "HoSoBN", new { id = phieuNXId, page });
        }

        public async Task<JsonResult> CheckTonDau(string tuNgay)
        {
            DateTime fromDate = DateTime.Parse(tuNgay);

            // tonquy truoc ngay fromdate => xem co ton dau` ko ( tranh truong hop chua tinh ton dau cho vai phieu )
            string kVCTPTCs1 = await _cTHoSoBNService.CheckTonDau(DateTime.Parse(tuNgay));
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