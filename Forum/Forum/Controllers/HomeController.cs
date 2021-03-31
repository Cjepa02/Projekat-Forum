using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models.Dal;
using Forum.Models;
using Forum.Models.Dto;


namespace Forum.Controllers
{

    public class HomeController : Controller
    {
        private ForumContext dbContext = new ForumContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Izmena(int id)
        {
            Korisnik model = dbContext.korisniks
                .FirstOrDefault(red => red.id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Izmena(Korisnik paketic)
        {
            Korisnik model = dbContext.korisniks
            .FirstOrDefault(red => red.id == paketic.id);
            model.lozinka = paketic.lozinka;
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Home", "");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
    