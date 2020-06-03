using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TestWebMVC.Models;

namespace TestWebMVC.Controllers
{
    public class AccountController : Controller
    {

        // GET: Account
        [HttpGet]
        public ActionResult register(int id = 0)
        {
            user userModel = new user();
            ViewBag.role = new List<SelectListItem>(){
            new SelectListItem(){Value="user",Text="User"},
            new SelectListItem(){Value="owner",Text="Owner"} };
            return View(userModel);
        }
        [HttpPost]
        
        public ActionResult register(TestWebMVC.Models.user userModel)
        {
            if (ModelState.IsValid)
            {
                using (ticketingEntities8 dbModels = new ticketingEntities8())
                {
                    dbModels.users.Add(userModel);
                    dbModels.SaveChanges();
                }
                ModelState.Clear();
                return View("view_login");
            }
            else
            {
                return RedirectToAction("register");
            }
        }
        public ActionResult view_login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult verify(TestWebMVC.Models.user userModel)
        {
            using(ticketingEntities8 db = new ticketingEntities8())
            {
                var userDetails = db.users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                if(userDetails==null)
                {
                    return View("view_login");
                }
                else
                {
                    Session["userID"] = userDetails.UserID;
                    Session["Name"] = userDetails.UserName;
                    Session["Role"] = userDetails.RoleName;
                    return RedirectToAction("Index", "User");
                }
            }
        }

        public ActionResult logout()
        {
            int userId = (int) Session["userID"];
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        
    }
}