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
    public class KorisnikController : Controller
    {
        private ForumContext dbContext = new ForumContext();
        public ActionResult Registracija()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {
            dbContext.korisniks.Add(korisnik);
            korisnik.tip_korisnika = "korisnik";
            dbContext.SaveChanges();
            return RedirectToAction("Index","Home","");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {        
                ViewBag.Error = null;

                Korisnik k = dbContext.korisniks.FirstOrDefault
                    (korisnik => korisnik.korisnickoime == model.korisnickoime &&
                        korisnik.lozinka == model.lozinka);

                if (k == null)
                {
                    ViewBag.Error = "Pogresni kredencijali";
                    return View();
                }
                else
                {
                    Session.Add("korisnik", k);
                    return RedirectToAction("Index", "Home", "");
                }
            }

            return View();
        }
        public ActionResult IzlogujSe()
        {
            Session.Remove("korisnik");
            return RedirectToAction("Index", "Home", "");
        }
        public ActionResult Promovisi()
        {
            //if admin
            ViewBag.Korisniks = dbContext.korisniks.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Promovisi(Podforum model)
        {
            //if admin
            dbContext.podforums.Add(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Home", "");
        }
        public ActionResult Izmena(int id)
        {
            Korisnik model = dbContext.korisniks
                .FirstOrDefault(red => red.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Izmena(Korisnik paketic)
        {
            Korisnik model = dbContext.korisniks
            .FirstOrDefault(red => red.Id == paketic.Id);
            model.Lozinka = paketic.Lozinka;
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Home", "");
        }
    }
}