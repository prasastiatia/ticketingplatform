using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebMVC.Models;

namespace TestWebMVC.Controllers
{
    public class UserController : Controller
    {
        public static List<ticket> lst;
        private ticketingEntities8 db1 = new ticketingEntities8();

        // GET: User
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                if(Session["Role"].ToString() == "user")
                {
                    ViewBag.Title = "Dashboard User";
                }
                else
                {
                    ViewBag.Title = "Dashboard Owner";
                }
                return View("dashboard_user");
            }
            else
            {
                return RedirectToAction("view_login", "Account");
            }
        }

        [HttpGet]
        public ActionResult AddTicket(int id=0)
        {
            if (Session["UserID"] != null)
            {
                ticket ticketDb = new ticket();
                ViewBag.ticket = new List<SelectListItem>(){
            new SelectListItem(){Value="Open",Text="Open"},
            new SelectListItem(){Value="In Progress",Text="In Progress"},
            new SelectListItem(){Value="Done",Text="Done"},
            new SelectListItem(){Value="Close",Text="Close"}
            };
                ticketDb.ticket_assignee = "Tiketku";
                ticketDb.ticket_owner = Session["Name"].ToString();
                return View(ticketDb);
            } else
            {
                return RedirectToAction("view_login", "Account");
            }
        }
       
        public ActionResult AddTicket(ticket ticket_model)
        {
            using (ticketingEntities8 dbModels = new ticketingEntities8())
            {
                dbModels.tickets.Add(ticket_model);
                dbModels.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("AllTicket");
        }
        public ActionResult DetailTicket(int id)
        {
            using (ticketingEntities8 dbModels = new ticketingEntities8())
            {
                return View(dbModels.tickets.Where(x => x.ticket_id == id).FirstOrDefault());
            }
        }
        public ActionResult AllTicket(string option, string search)
        {
            using(ticketingEntities8 dbModels = new ticketingEntities8())
            {
                if (option == "owner")
                {
                    //Index action method will return a view with a student records based on what a user specify the value in textbox  
                    return View(dbModels.tickets.Where(x => x.ticket_owner.StartsWith(search) || search == null).ToList());
                }
                else if (option == "status")
                {
                    return View(dbModels.tickets.Where(x => x.ticket_status == search || search == null).ToList());
                }
                else
                {
                    return View(dbModels.tickets.Where(x => x.ticket_category == search || search == null).ToList());
                }
                return View(dbModels.tickets.ToList());
            }
           
        }
        [HttpGet]
        public ActionResult EditTicket(int? id )
        {
            
            ticket tickets = db1.tickets.Where(x => x.ticket_id == id).FirstOrDefault();
           
            return View(tickets);
        }
        [HttpPost]
        public ActionResult EditTicket( ticket ticketing)
        {

            try
            {
                using (ticketingEntities8 dbModels = new ticketingEntities8())
                {
                    dbModels.Entry(ticketing).State = EntityState.Modified;
                    dbModels.SaveChanges();
                }

                return RedirectToAction("AllTicket");
            }
            catch
            {
                return View();
            }

            //return RedirectToAction("AllTicket");
        }
        [HttpGet]
        public ActionResult DeleteTicket(int id)
        {
            using (ticketingEntities8 dbModels = new ticketingEntities8())
            {
                return View(dbModels.tickets.Where(x => x.ticket_id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult DeleteTicket(int id, ticket ticket_model)
        {
            try
            {
                using (ticketingEntities8 dbModels = new ticketingEntities8())
                {
                   ticket ticketing =  dbModels.tickets.Where(x => x.ticket_id == id).FirstOrDefault();
                    dbModels.tickets.Remove(ticketing);
                    dbModels.SaveChanges();
                }
                return RedirectToAction("AllTicket");
            }
            catch
            {
                return View();
            }
        }
    }
}