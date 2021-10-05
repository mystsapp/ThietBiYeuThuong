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
    public class ThietBiController : BaseController
    {
        private readonly IThietBiService _thietBiService;

        [BindProperty]
        public ThietBiViewModel ThietBiVM { get; set; }

        public ThietBiController(IThietBiService thietBiService)
        {
            _thietBiService = thietBiService;
            ThietBiVM = new ThietBiViewModel()
            {
                ThietBi = new ThietBi()
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchThietBis_Code(string code)
        {
            code ??= "";
            ThietBiVM.IEnumThietBi = await _thietBiService.SearchThietBis_Code(code);
            ThietBiVM.MaTBText = code;
            return PartialView(ThietBiVM);
        }

        public JsonResult GetThietBis_By_Code(string code)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            ThietBi thietBi = _thietBiService.GetThietBiByCode(code);
            if (thietBi != null)
            {
                return Json(new
                {
                    status = true,
                    data = thietBi
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