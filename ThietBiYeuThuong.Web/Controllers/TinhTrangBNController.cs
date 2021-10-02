using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Web.Controllers
{
    public class TinhTrangBNController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
