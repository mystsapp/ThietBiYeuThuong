using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Web.Models;
using ThietBiYeuThuong.Web.Services;

namespace ThietBiYeuThuong.Web.Controllers
{
    public class PhieuNXController : Controller
    {
        private readonly IPhieuNXService _phieuNXService;

        [BindProperty]
        public PhieuNXViewModel PhieuVM { get; set; }

        public PhieuNXController(IPhieuNXService phieuNXService)
        {
            _phieuNXService = phieuNXService;
            PhieuVM = new PhieuNXViewModel()
            {
                PhieuNX = new Data.Models.PhieuNX()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            PhieuVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            PhieuVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                PhieuVM.PhieuNX = await _phieuNXService.GetById(id);
                ViewBag.id = PhieuVM.PhieuNX.SoPhieu;
            }
            else
            {
                PhieuVM.PhieuNX = new Data.Models.PhieuNX();
            }
            PhieuVM.PhieuNXDtos = await _phieuNXService.ListPhieuNX(searchString, searchFromDate, searchToDate, page);
            return View(PhieuVM);
        }
    }
}