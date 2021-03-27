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
            dbContext.SaveChanges();
            return RedirectToAction("Registracija");
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
                // Zelim da proverim da li u bazi postoji red u tabeli korisnik
                // sa korisnickim imenom i lozinkom koji je prosledjen
                // select * from korisnik where korisnikoIme = model.Korisnicko ime and
                // lozinka = model.Lozinka

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
    }
}