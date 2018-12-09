using PostOffice.App_Data;
using PostOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace PostOffice.Controllers
{
    public class MainController : Controller
    {
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                string pass = loginModel.UserPassword;
                string login = loginModel.UserName;

                var Users = PostOperations.GetUsersEf();
                USERS user = null;

                string PasswordHash;
                try
                {
                    PasswordHash = PostOperations.GetUsersEf().Where(p => p.IDUSER == login).Select(p => p.PASSWORD).First();
                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (!HashPass.VerifyMd5Hash(md5Hash, pass, PasswordHash))
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    user = Users.Select(p => p).Where(p => p.IDUSER == login).First();
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("UserPassword", "Пароль или логин введен неправильно!\nПовторите ввод или зарегистрируйтесь.");
                    return View("Login");
                }
                if (user.ACCESSRIGHTS == "user")
                {
                    PostOperations.UserName = user.IDUSER;
                    Session["IsAuthorizedUser"] = "yes";
                    Session["IsAuthorized"] = "no";
                    return RedirectToAction("IndexAddress", "User", new SendPackageModel());
                }
                else
                {
                    PostOperations.UserName = user.IDUSER;
                    Session["IsAuthorizedUser"] = "no";
                    Session["IsAuthorized"] = "yes";
                    return RedirectToAction("SendPackage", "Admin", new SendPackageModel());
                }
            }
            return this.RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        protected string ADMIN_CODE = "0cb1is82dif9s3cm0eas";
        //protected string ADMIN_CODE = "y";
        public ActionResult UserRegistration()
        {
            ViewBag.address = new SelectList(PostOperations.GetIndexEf().OrderBy(p => p.ADDRES).Select(p => p.INDEX1 + ", " + p.ADDRES ));
            return View(new UserRegModel());
        }
        [HttpPost]
        public ActionResult UserRegistration(UserRegModel userRegModel)
        {
            string FNL = userRegModel.Surname + " " + userRegModel.Name + " " + userRegModel.MiddleName;
            string login = userRegModel.Login;
            string pass = userRegModel.UserPassword;

            USERS user = null;
            Clients client = null;
            int index = 0;
            var context = new POSTContext();
            if (userRegModel.AdminCode != null && userRegModel.AdminCode != ADMIN_CODE)
            {
                ModelState.AddModelError("Address", "Неверный код администратора!");
                ViewBag.address = new SelectList(PostOperations.GetIndexEf().OrderBy(p => p.ADDRES).Select(p => p.INDEX1 + ", " + p.ADDRES));
                return View(new UserRegModel());
            }
            //хеширование пароля
            string Password_Hash;
            using (MD5 md5Hash = MD5.Create())
            {
                Password_Hash = HashPass.GetMd5Hash(md5Hash, pass);
            }
            if (userRegModel.AdminCode == ADMIN_CODE)
            {
                user = new USERS
                {
                    IDUSER = login,
                    USER = FNL,
                    PASSWORD = Password_Hash.Substring(0, 20),
                    ACCESSRIGHTS = "admin"
                };
            }
            else
            {
                user = new USERS
                {
                    IDUSER = login,
                    USER = FNL,
                    PASSWORD = Password_Hash.Substring(0, 20),
                    ACCESSRIGHTS = "user"
                };
                client = new Clients
                {
                    IDCLIENT = login,
                    CLIENT = FNL,
                    ADDRES = userRegModel.Address.Substring(8)
                };
            }
            if (userRegModel.AdminCode == null)
            {
                var Indexes = PostOperations.GetIndexEf();
                try
                {
                    index = Indexes.Where(p => (p.ADDRES) == (userRegModel.Address.Substring(8))).Select(p => p.INDEX1).First();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Address", "Address Error!");
                    ViewBag.address = new SelectList(PostOperations.GetIndexEf().OrderBy(p => p.ADDRES).Select(p => p.INDEX1 + ", " + p.ADDRES));
                    return View(new UserRegModel());
                }
                client.INDEX = index;
                context.Clients.Add(client);
                try
                {
                    context.SaveChanges();
                    context = new POSTContext();
                }
                catch
                {
                    context = new POSTContext();
                }
                var context1 = new POSTContext();
                context1.USERS.Add(user);
                try
                {
                    context1.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("Login", "Логин занят!");
                    ViewBag.address = new SelectList(PostOperations.GetIndexEf().OrderBy(p => p.ADDRES).Select(p => p.INDEX1 + ", " + p.ADDRES));
                    return View(new UserRegModel());
                }
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            context.USERS.Add(user);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("Login", "Логин занят!");
                ViewBag.address = new SelectList(PostOperations.GetIndexEf().OrderBy(p => p.ADDRES).Select(p => p.INDEX1 + ", " + p.ADDRES));
                return View(new UserRegModel());
            }
            return RedirectToAction("Login","Main", new LoginModel());
        }
    }
}