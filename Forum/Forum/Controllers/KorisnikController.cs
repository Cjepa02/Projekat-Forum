using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models.Dal;
using Forum.Models;
using Forum.Models.Dto;
using System.Web.UI;

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
            korisnik.Tip_korisnika = "korisnik";
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
                    (korisnik => korisnik.KorisnickoIme == model.KorisnickoIme &&
                        korisnik.Lozinka == model.Lozinka);

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
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (korisnik != null)
            {
                if (korisnik.Tip_korisnika == "admin")
                {
                    ViewBag.Korisniks = dbContext.korisniks.ToList();
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            return null;
        }
        [HttpPost]
        public ActionResult Promovisi(Podforum model)
        {         
            dbContext.podforums.Add(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Home", "");
        }
        public ActionResult Izmena(int id)
        {
            Korisnik model = dbContext.korisniks
                .FirstOrDefault(red => red.Id == id);
            ViewBag.Korisniks = model.Lozinka;
            return View(model);
        }

        [HttpPost]
        public ActionResult Izmena(Korisnik paketic)
        {
            Korisnik model = dbContext.korisniks
            .FirstOrDefault(red => 1 == paketic.Id);
            string staralozinka = model.Lozinka;
            if (staralozinka == paketic.Lozinka)
            {
                model.Lozinka = paketic.Lozinka;
                Response.Write("<script>alert('Lozinka uspešno promenjena!')</script>");
                dbContext.SaveChanges();
            }
            else if (staralozinka != paketic.Lozinka)
            {
                Response.Write("<script>alert('Stara lozinka netačna!')</script>");
            }
            return RedirectToAction("Index", "Home", "");
        }
    }
}