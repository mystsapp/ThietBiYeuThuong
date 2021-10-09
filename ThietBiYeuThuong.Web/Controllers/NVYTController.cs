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

        public IActionResult Index()
        {
            return View();
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