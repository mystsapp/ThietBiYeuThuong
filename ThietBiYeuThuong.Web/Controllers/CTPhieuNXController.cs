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
                        var tonDau = _tinhTonService.GetLast() == null ? 0 : _tinhTonService.GetLast().SoLuongTon;
                        var cTPhieuNXes = await _cTPhieuNXService.GetCTTrongNgay();
                        if (cTPhieuNXes == null || cTPhieuNXes.Count == 0)
                        {
                            tongNhap = 0; tongXuat = 0; ton = 0;
                        }
                        else
                        {
                            tongNhap = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PN"))
                                                  .Sum(x => x.SoLuong);
                            tongXuat = cTPhieuNXes.Where(x => x.PhieuNXId.Contains("PX"))
                                                      .Sum(x => x.SoLuong);
                            ton = tonDau + tongNhap - tongXuat;
                        }

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

        public IActionResult BackIndex(string phieuNXId, int page)
        {
            return RedirectToAction(nameof(Index), "PhieuNX", new { id = phieuNXId, page });
        }
    }
}