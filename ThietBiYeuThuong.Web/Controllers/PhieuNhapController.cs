using Microsoft.AspNetCore.Http.Extensions;
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
    public class PhieuNhapController : BaseController
    {
        private readonly IPhieuNhapService _phieuNhapService;
        private readonly ILoaiThietBiService _loaiThietBiService;
        private readonly ITrangThaiService _trangThaiService;
        private readonly IThietBiService _thietBiService;
        private readonly ICTPhieuService _cTPhieuService;
        private readonly IBenhNhanThietBiService _benhNhanThietBiService;
        private readonly ICTHoSoBNService _cTHoSoBNService;

        [BindProperty]
        public PhieuNhapViewModel PhieuNhapVM { get; set; }

        public PhieuNhapController(IPhieuNhapService phieuNhapService,
                                   ILoaiThietBiService loaiThietBiService,
                                   ITrangThaiService trangThaiService,
                                   IThietBiService thietBiService,
                                   ICTPhieuService cTPhieuService,
                                   IBenhNhanThietBiService benhNhanThietBiService,
                                   ICTHoSoBNService cTHoSoBNService)
        {
            PhieuNhapVM = new PhieuNhapViewModel()
            {
                PhieuNhap = new Data.Models.PhieuNhap()
            };
            _phieuNhapService = phieuNhapService;
            _loaiThietBiService = loaiThietBiService;
            _trangThaiService = trangThaiService;
            _thietBiService = thietBiService;
            _cTPhieuService = cTPhieuService;
            _benhNhanThietBiService = benhNhanThietBiService;
            _cTHoSoBNService = cTHoSoBNService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            PhieuNhapVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            PhieuNhapVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                PhieuNhapVM.PhieuNhap = await _phieuNhapService.GetById(id);
                ViewBag.id = PhieuNhapVM.PhieuNhap.SoPhieu;
            }
            else
            {
                PhieuNhapVM.PhieuNhap = new Data.Models.PhieuNhap();
            }
            PhieuNhapVM.PhieuNhapDtos = await _phieuNhapService.ListPhieuNhap(searchString, searchFromDate, searchToDate, page);
            return View(PhieuNhapVM);
        }

        #region Create_Day

        public async Task<IActionResult> Create_Day(string strUrl, int page)
        {
            //ViewBag.trangThaiId = 1;

            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            PhieuNhapVM.Page = page;
            PhieuNhapVM.LoaiThietBis = await _loaiThietBiService.GetAll();
            PhieuNhapVM.TrangThais = await _trangThaiService.GetAll();

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Create_Day")]
        public async Task<IActionResult> Create_Day_Post(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuNhapVM = new PhieuNhapViewModel()
                {
                    PhieuNhap = new PhieuNhap(),
                    StrUrl = strUrl
                };

                return View(PhieuNhapVM);
            }

            PhieuNhapVM.PhieuNhap.NgayNhap = DateTime.Now;
            PhieuNhapVM.PhieuNhap.NguoiNhap = user.Username;
            PhieuNhapVM.PhieuNhap.SoPhieu = _phieuNhapService.GetSoPhieu("PN");
            PhieuNhapVM.PhieuNhap.TrangThaiId = 1; // đầy

            // SL
            if (PhieuNhapVM.SoLuong == 0)
            {
                PhieuNhapVM.SoLuong = 1;
            }
            if (PhieuNhapVM.SoLuong == 1)
            {
                PhieuNhapVM.ThietBi.MaTB = _thietBiService.GetMaTB("TB");
                var thietBi = new ThietBi()
                {
                    MaTB = _thietBiService.GetMaTB("TB"),
                    TenTB = PhieuNhapVM.ThietBi.TenTB,
                    LoaiTBId = PhieuNhapVM.ThietBi.LoaiTBId,
                    TrangThaiId = 1, // đầy
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Username,
                    TinhTrang = true, // chưa giao
                    LogFile = "-User tạo(nhập đầy): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString() // user.Username
                };
                await _thietBiService.CreateAsync(thietBi); // save thietbi

                // save CTPhieu
                PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
                PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
                PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
                PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
                PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
                PhieuNhapVM.CTPhieu.NgayTao = DateTime.Now;
                PhieuNhapVM.CTPhieu.LogFile = "-User tạo(nhập đầy): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
                await _cTPhieuService.CreateAsync(PhieuNhapVM.CTPhieu); // save ctphieu
            }
            else
            {
                for (int i = 1; i <= PhieuNhapVM.SoLuong; i++)
                {
                    var thietBi = new ThietBi()
                    {
                        MaTB = _thietBiService.GetMaTB("TB"),
                        TenTB = PhieuNhapVM.ThietBi.TenTB,
                        LoaiTBId = PhieuNhapVM.ThietBi.LoaiTBId,
                        TrangThaiId = 1, // đầy
                        NgayTao = DateTime.Now,
                        TinhTrang = true, // chưa giao
                        NguoiTao = user.Username,
                        LogFile = "-User tạo(nhập đầy): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString() // user.Username
                    };
                    await _thietBiService.CreateAsync(thietBi); // save thietbi

                    // save CTPhieu
                    PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
                    PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
                    PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
                    PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
                    PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
                    PhieuNhapVM.CTPhieu.NgayTao = DateTime.Now;
                    PhieuNhapVM.CTPhieu.LogFile = "-User tạo(nhập đầy): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
                    await _cTPhieuService.CreateAsync(PhieuNhapVM.CTPhieu); // save ctphieu
                }
            }
            // ghi log
            PhieuNhapVM.PhieuNhap.LogFile = "-User tạo(nhập đầy): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNhapService.CreateAsync(PhieuNhapVM.PhieuNhap); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuNhapVM.PhieuNhap.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuNhapVM);
            }
        }

        #endregion Create_Day

        #region Create_GoiBom : xuat

        public async Task<IActionResult> Create_GoiBom(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            PhieuNhapVM.Page = page;

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Create_GoiBom")]
        public async Task<IActionResult> Create_GoiBom_Post(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuNhapVM = new PhieuNhapViewModel()
                {
                    PhieuNhap = new PhieuNhap(),
                    StrUrl = strUrl
                };

                return View(PhieuNhapVM);
            }

            PhieuNhapVM.PhieuNhap.NgayNhap = DateTime.Now;
            PhieuNhapVM.PhieuNhap.NguoiNhap = user.Username;
            PhieuNhapVM.PhieuNhap.SoPhieu = _phieuNhapService.GetSoPhieu("PX"); // xuất gởi bơm
            PhieuNhapVM.PhieuNhap.TrangThaiId = 4; // Gởi bơm

            // capnhat thietbi trangthai Vừa bơm về
            var thietBi = await _thietBiService.GetById(PhieuNhapVM.CTPhieu.ThietBiId);
            thietBi.TrangThaiId = 4; // Gởi bơm
            thietBi.LogFile += "\n" + "-User xuất gởi bơm: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _thietBiService.UpdateAsync(thietBi);

            // save CTPhieu
            PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
            PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
            PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
            PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
            PhieuNhapVM.CTPhieu.NgayXuat = DateTime.Now;
            PhieuNhapVM.CTPhieu.NgayTao = DateTime.Now;
            PhieuNhapVM.CTPhieu.LogFile = "-User xuất gởi bơm: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _cTPhieuService.CreateAsync(PhieuNhapVM.CTPhieu); // save ctphieu

            // ghi log
            PhieuNhapVM.PhieuNhap.LogFile = "-User xuất gởi bơm: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNhapService.CreateAsync(PhieuNhapVM.PhieuNhap); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuNhapVM.PhieuNhap.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuNhapVM);
            }
        }

        #endregion Create_GoiBom : xuat

        #region Create_ThuHoi

        public async Task<IActionResult> Create_ThuHoi(string strUrl, int page)
        {
            //ViewBag.trangThaiId = 1;

            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            PhieuNhapVM.Page = page;

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Create_ThuHoi")]
        public async Task<IActionResult> Create_ThuHoi_Post(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuNhapVM = new PhieuNhapViewModel()
                {
                    PhieuNhap = new PhieuNhap(),
                    StrUrl = strUrl
                };

                return View(PhieuNhapVM);
            }

            PhieuNhapVM.PhieuNhap.NgayNhap = DateTime.Now;
            PhieuNhapVM.PhieuNhap.NguoiNhap = user.Username;
            PhieuNhapVM.PhieuNhap.SoPhieu = _phieuNhapService.GetSoPhieu("PN");
            PhieuNhapVM.PhieuNhap.TrangThaiId = 2; // thu hồi

            // capnhat thietbi trangthai thu hồi
            var thietBi = await _thietBiService.GetById(PhieuNhapVM.CTPhieu.ThietBiId);
            thietBi.TrangThaiId = 2;
            thietBi.TinhTrang = true;
            thietBi.LogFile += "\n" + "-User thu hồi: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _thietBiService.UpdateAsync(thietBi);

            // xoá BenhNhanThietBi
            BenhNhanThietBi benhNhanThietBi = await _benhNhanThietBiService.GetById(PhieuNhapVM.MaBN, thietBi.MaTB);
            // capnhat CTHoSoBN(Đ.Hồ thu)
            CTHoSoBN cTHoSoBN = await _cTHoSoBNService.GetById(benhNhanThietBi.CTHoSoBNId);
            cTHoSoBN.DongHoThu = PhieuNhapVM.CTPhieu.DongHoThu;
            cTHoSoBN.LogFile += "\n" + " -User thu hồi: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString();
            await _cTHoSoBNService.UpdateAsync(cTHoSoBN);
            await _benhNhanThietBiService.DeleteAsync(benhNhanThietBi);

            // save CTPhieu
            PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
            PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
            PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
            PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
            PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
            PhieuNhapVM.CTPhieu.NgayTao = DateTime.Now;
            PhieuNhapVM.CTPhieu.DongHoGiao = cTHoSoBN.DongHoGiao;
            PhieuNhapVM.CTPhieu.NVGiaoBinh = cTHoSoBN.NVGiaoBinh;
            PhieuNhapVM.CTPhieu.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _cTPhieuService.CreateAsync(PhieuNhapVM.CTPhieu); // save ctphieu

            // ghi log
            PhieuNhapVM.PhieuNhap.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNhapService.CreateAsync(PhieuNhapVM.PhieuNhap); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuNhapVM.PhieuNhap.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuNhapVM);
            }
        }

        #endregion Create_ThuHoi

        #region Create_VuaBomVe

        public async Task<IActionResult> Create_VuaBomVe(string strUrl, int page)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            PhieuNhapVM.Page = page;

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Create_VuaBomVe")]
        public async Task<IActionResult> Create_VuaBomVe_Post(string strUrl, int page)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                PhieuNhapVM = new PhieuNhapViewModel()
                {
                    PhieuNhap = new PhieuNhap(),
                    StrUrl = strUrl
                };

                return View(PhieuNhapVM);
            }

            PhieuNhapVM.PhieuNhap.NgayNhap = DateTime.Now;
            PhieuNhapVM.PhieuNhap.NguoiNhap = user.Username;
            PhieuNhapVM.PhieuNhap.SoPhieu = _phieuNhapService.GetSoPhieu("PN");
            PhieuNhapVM.PhieuNhap.TrangThaiId = 3; // Vừa bơm về

            // capnhat thietbi trangthai Vừa bơm về
            var thietBi = await _thietBiService.GetById(PhieuNhapVM.CTPhieu.ThietBiId);
            thietBi.TrangThaiId = 3; // Vừa bơm về
            thietBi.LogFile += "\n" + "-User nhập vừa bơm về: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _thietBiService.UpdateAsync(thietBi);

            // save CTPhieu
            PhieuNhapVM.CTPhieu.SoPhieu = PhieuNhapVM.PhieuNhap.SoPhieu;
            PhieuNhapVM.CTPhieu.SoPhieuCT = _cTPhieuService.GetSoPhieuCT("CN");
            PhieuNhapVM.CTPhieu.ThietBiId = thietBi.MaTB;
            PhieuNhapVM.CTPhieu.LapPhieu = user.Username;
            PhieuNhapVM.CTPhieu.NgayTao = DateTime.Now;
            PhieuNhapVM.CTPhieu.NgayNhap = DateTime.Now;
            PhieuNhapVM.CTPhieu.LogFile = "-User tạo(Vừa bơm về): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            await _cTPhieuService.CreateAsync(PhieuNhapVM.CTPhieu); // save ctphieu

            // ghi log
            PhieuNhapVM.PhieuNhap.LogFile = "-User tạo(Vừa bơm về): " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _phieuNhapService.CreateAsync(PhieuNhapVM.PhieuNhap); // save

                SetAlert("Thêm mới thành công.", "success");

                return RedirectToAction(nameof(Index), new { id = PhieuNhapVM.PhieuNhap.SoPhieu, page = page });
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(PhieuNhapVM);
            }
        }

        #endregion Create_VuaBomVe

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            PhieuNhapVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            PhieuNhapVM.PhieuNhap = await _phieuNhapService.GetById(id);

            if (PhieuNhapVM.PhieuNhap == null)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var trangThai = await _trangThaiService.GetById(PhieuNhapVM.PhieuNhap.TrangThaiId);
            ViewBag.tenTrangThai = trangThai.Name;

            return View(PhieuNhapVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            string temp = "", log = "";

            if (id != PhieuNhapVM.PhieuNhap.SoPhieu)
            {
                ViewBag.ErrorMessage = "Phiếu này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                PhieuNhapVM.PhieuNhap.NgaySua = DateTime.Now;
                PhieuNhapVM.PhieuNhap.NguoiSua = user.Username;

                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _phieuNhapService.GetByIdAsNoTracking(id);

                if (t.DonVi != PhieuNhapVM.PhieuNhap.DonVi)
                {
                    temp += String.Format("- DonVi thay đổi: {0}->{1}", t.DonVi, PhieuNhapVM.PhieuNhap.DonVi);
                }

                if (t.TrangThaiId != PhieuNhapVM.PhieuNhap.TrangThaiId)
                {
                    temp += String.Format("- TrangThaiId thay đổi: {0}->{1}", t.TrangThaiId, PhieuNhapVM.PhieuNhap.TrangThaiId);
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
                    PhieuNhapVM.PhieuNhap.LogFile = t.LogFile;
                }

                try
                {
                    await _phieuNhapService.UpdateAsync(PhieuNhapVM.PhieuNhap);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(PhieuNhapVM);
                }
            }
            // not valid

            return View(PhieuNhapVM);
        }

        public IActionResult DetailsRedirect(string strUrl/*, string tabActive*/)
        {
            //if (!string.IsNullOrEmpty(tabActive))
            //{
            //    strUrl = strUrl + "&tabActive=" + tabActive; // for redirect tab
            //}
            return Redirect(strUrl);
        }
    }
}