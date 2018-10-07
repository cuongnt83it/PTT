using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using PTT.Common;
using Model;
using System.Web.Security;

namespace PTT.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
           
            if (ControllerContext.HttpContext.Request.Cookies["login"] != null)
            {
                HttpCookie aCookie = Request.Cookies["login"];
                var dao = new Model.DAO.UserDao();
                var result = dao.Login(aCookie.Values["UserName"], Hepper.MD5Hash(aCookie.Values["UserName"] + aCookie.Values["Password"]));
                if (result == 1)
                {
                    var us = dao.GetUserByUserName(aCookie.Values["UserName"]);
                    UserLogin uslog = new UserLogin();
                    uslog.UserID = us.LoginID;
                    uslog.UserName = us.UserName;
                    uslog.FullName = us.FullName;
                    uslog.Image = us.Image;
                    uslog.LastLogIn = us.LastLogIn;
                    Session[CommonConstant.USER_SESSION] = uslog;
                    dao.LastLogin(us.LoginID, Hepper.GetDateServer());
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
        // GET: Login
        public ActionResult Logout()
        {
            Session[CommonConstant.USER_SESSION] = null;
            
            if (ControllerContext.HttpContext.Request.Cookies["login"] != null)
            {
                var user = new HttpCookie("user")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                Response.Cookies.Add(user);
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.LoginModel mode)
        {
            if (ModelState.IsValid)
            {
                var dao = new Model.DAO.UserDao();
                var result = dao.Login(mode.UserName, Hepper.MD5Hash(mode.UserName + mode.Password));
                if (result == 1)
                {
                    var us = dao.GetUserByUserName(mode.UserName);
                    UserLogin uslog = new UserLogin();
                    uslog.UserID = us.LoginID;
                    uslog.UserName = us.UserName;
                    uslog.FullName = us.FullName;
                    uslog.Image = us.Image;
                    uslog.LastLogIn = us.LastLogIn;
                    Session[CommonConstant.USER_SESSION] = uslog;
                    dao.LastLogin(us.LoginID, Hepper.GetDateServer());
                    if (mode.Remember)
                    {

                        HttpCookie aCookie = new HttpCookie("login");
                        aCookie.Values["UserName"] = mode.UserName;
                        aCookie.Values["Password"] = mode.Password;
                        aCookie.Expires = DateTime.Now.AddDays(365);
                        aCookie.Secure = true;
                        ControllerContext.HttpContext.Response.Cookies.Add(aCookie);

                    }
                    return RedirectToAction("Index", "Home");

                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
            }
            return View();
        }
       
    }
}