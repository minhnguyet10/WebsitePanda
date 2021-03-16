using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;
using Website_Panda.Models.Login;

namespace Website_Panda.Controllers
{
    public class LoginAdminController : Controller
    {
        DB_Optical _db = new DB_Optical();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(FormLogin model)
        {
            if (ModelState.IsValid)
            {
                var md = new MDLogin();
                var result = md.Login(model.UserName, GetMD5(model.Password), true);
                if (result == 1)
                {
                    var user = md.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.IDUser;
                    userSession.GroupID = user.GroupID;
                    Session.Add(Common.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Product");
                }
                else if (result == 4)
                {
                    var user = md.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.IDUser;
                    userSession.GroupID = user.GroupID;
                    Session.Add(Common.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 5) //SHIPPER
                {
                    var user = md.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.IDUser;
                    userSession.GroupID = user.GroupID;
                    Session.Add(Common.USER_SESSION, userSession);
                    return RedirectToAction("Approved_Unpaid", "OrderManagement");
                }
                else if (result == 0)
                {
                    return Content("Tài khoản không tồn tại.");
                }

                else if (result == -2)
                {
                    return Content("Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    return Content("Tài khoản của bạn không có quyền đăng nhập.");
                }
                else
                {
                    return Content("đăng nhập không đúng.");
                }
            }
            return RedirectToAction("Index", "LoginAdmin");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Register(Administrator admin){
            
            if (ModelState.IsValid)
            {
                var check = _db.Administrators.FirstOrDefault(s => s.UserName == admin.UserName);
                if (check == null)
                {
                    admin.Password = GetMD5(admin.Password);
                    admin.GroupID = "MEMBER";
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Administrators.Add(admin);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exist! Use another email!";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "LoginAdmin");
        }

        //ma hoa mk
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}