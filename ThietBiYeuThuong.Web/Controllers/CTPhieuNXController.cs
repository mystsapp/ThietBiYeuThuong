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

        [BindProperty]
        public CTPhieuNXViewModel CTPhieuNXVM { get; set; }

        public CTPhieuNXController(ICTPhieuNXService cTPhieuNXService, IPhieuNXService phieuNXService)
        {
            _cTPhieuNXService = cTPhieuNXService;
            _phieuNXService = phieuNXService;
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

        public async Task<IActionResult> CTPhieuNX_Create_Partial_Post(string PhieuNXId, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            CTPhieuNXVM.PhieuNX = await _phieuNXService.GetById(PhieuNXId);
            CTPhieuNXVM.Page = page;

            if (!ModelState.IsValid)
            {
                // not valid

                return View(CTPhieuNXVM);
            }

            CTPhieuNXVM.CTPhieuNX.PhieuNXId = PhieuNXId;
            CTPhieuNXVM.CTPhieuNX.LapPhieu = user.Username;
            CTPhieuNXVM.CTPhieuNX.NgayNhap = DateTime.Now;

            // next sophieuct --> bat buoc phai co'
            switch (CTPhieuNXVM.PhieuNX.LoaiPhieu)
            {
                case "PN": // nhap
                    CTPhieuNXVM.CTPhieuNX.SoPhieuCT = _cTPhieuNXService.GetSoPhieuCT("CN");
                    break;

                default: // xuat
                    CTPhieuNXVM.PhieuNX.SoPhieu = _cTPhieuNXService.GetSoPhieuCT("CX");
                    break;
            }
            // next sophieuct

            // ghi log
            CTPhieuNXVM.CTPhieuNX.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _cTPhieuNXService.Create(CTPhieuNXVM.CTPhieuNX);

                SetAlert("Thêm mới thành công.", "success");
                return BackIndex(PhieuNXId, CTPhieuNXVM.Page); // redirect to Home/Index/?id
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(CTPhieuNXVM);
            }
        }

        public IActionResult BackIndex(string phieuNXId, int page)
        {
            return RedirectToAction(nameof(Index), "PhieuNX", new { id = phieuNXId, page });
        }
    }
}