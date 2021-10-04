using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Data.Utilities;
using ThietBiYeuThuong.Web.Models;

namespace ThietBiYeuThuong.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = _unitOfWork.userRepository.login(model.Username, "015");

                // login
                var users = _unitOfWork.userRepository.GetAll().ToList();

                var result = users.Where(x => x.Username.ToLower() == model.Username.ToLower())
                                        .Select(x => new LoginModel()
                                        {
                                            Id = x.Id,
                                            Username = x.Username,
                                            Password = x.Password,
                                            Hoten = x.Hoten,
                                            Dienthoai = x.Dienthoai,
                                            Email = x.Email,
                                            Macn = x.Macn,
                                            RoleId = x.RoleId,
                                            Trangthai = x.Trangthai
                                        }).FirstOrDefault();
                // login

                if (result == null)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                }
                else
                {
                    if (!result.Trangthai)
                    {
                        ModelState.AddModelError("", "Tài khoản này đã bị khóa");
                        return View();
                    }
                    string modelPass = MaHoaSHA1.EncodeSHA1(model.Password);
                    if (result.Password != modelPass)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                    if (result.Password == modelPass)
                    {
                        //var user = _userRepository.GetById(model.Username);
                        var user = await _unitOfWork.userRepository.GetByIdAsync(result.Id);
                        HttpContext.Session.Set("loginUser", user);

                        //HttpContext.Session.SetString("username", model.Username);
                        //HttpContext.Session.SetString("password", model.Password);
                        //HttpContext.Session.SetString("hoten", result.Hoten);
                        //HttpContext.Session.SetString("phong", result.Maphong);
                        //HttpContext.Session.SetString("chinhanh", user.Macn);
                        HttpContext.Session.SetString("userId", user.Username);

                        return RedirectToAction("Index", "HoSoBN");
                    }
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "HoSoBN");
        }

        public IActionResult ChangePass(string strUrl)
        {
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            ChangePassModel changpassmodel = new ChangePassModel
            {
                Username = user.Username,
                StrUrl = strUrl
            };
            return View(changpassmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePassModel model, string strUrl)
        {
            if (ModelState.IsValid)
            {
                string oldpass = HttpContext.Session.GetString("password");
                if (MaHoaSHA1.EncodeSHA1(oldpass) != MaHoaSHA1.EncodeSHA1(model.Password))
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }
                else if (model.Newpassword != model.Confirmpassword)
                {
                    ModelState.AddModelError("", "Mật khẩu nhập lại không đúng.");
                }
                else
                {
                    // change pass

                    // for qltk user
                    var user = await _unitOfWork.userRepository.GetByIdAsync(model.Username);
                    user.Password = MaHoaSHA1.EncodeSHA1(model.Newpassword);
                    _unitOfWork.userRepository.Update(user);
                    // for qltk user

                    var result = await _unitOfWork.Complete();
                    // change pass

                    if (result > 0)
                    {
                        SetAlert("Đổi mật khẩu thành công", "success");
                        return LocalRedirect(strUrl); // /Users/Create : effect with local url
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đổi mật khẩu.");
                    }
                }
            }
            return View();
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}