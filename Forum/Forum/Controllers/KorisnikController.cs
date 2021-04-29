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
            List<Korisnik> Korisnik = dbContext.korisniks.ToList();
            string username = korisnik.KorisnickoIme;
            string email = korisnik.Email;
            for (int i = 0; i < Korisnik.Count; i++)
            {
                if (username == Korisnik[i].KorisnickoIme.ToString())
                {
                    ViewBag.Error = "Korisnik sa korisničkim imenom " + username + " " + "već postoji!";
                    if (email == Korisnik[i].Email.ToString())
                    {
                        ViewBag.Error = "Korisnik sa korisničkim imenom " + username + " " + "i unetom i-mejl adresom već postoji!";
                    }
                    else if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com"))
                    {
                        ViewBag.Error = "Molimo Vas unesite ispravnu i-mejl adresu!";
                    }
                }
                if (email == Korisnik[i].Email.ToString())
                {
                    ViewBag.Error = "Korisnik sa i-mejlom " + email + " " + "već postoji!";
                }
            }
          
            return View();          
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
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if(korisnik == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if(korisnik != null)
            {
                Session.Remove("korisnik");
                return RedirectToAction("Index", "Home", "");
            }
            return View();
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
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (korisnik != null)
            {
                Korisnik model = dbContext.korisniks
                    .FirstOrDefault(red => red.Id == id);
                ViewBag.Korisniks = model.Lozinka;
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Izmena(Korisnik paketic)
        {
            Korisnik model = dbContext.korisniks.FirstOrDefault
                    (korisnik => korisnik.Id == paketic.Id);
            model.TrenutnaLozinka = model.Lozinka;
            if (model.TrenutnaLozinka == paketic.TrenutnaLozinka)
            {
                if (model.Lozinka == paketic.Lozinka)
                {
                    ViewBag.Error = "Nova lozinka mora biti različita od stare!";
                }
                else if (model.Lozinka != paketic.Lozinka)
                {
                    model.Lozinka = paketic.Lozinka;
                    model.TrenutnaLozinka = model.Lozinka;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index", "Home", "");
                }
            }
            else if(model.TrenutnaLozinka != paketic.TrenutnaLozinka)
            {
                ViewBag.Error = "Trenutna lozinka netačna!";
            }          
            return View();
        }
    }
}