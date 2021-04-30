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
        public ActionResult Index()
        {
            ViewBag.Podforums = dbContext.podforums.ToList();
            return View();
        }
        public ActionResult Podforum(int id)
        {
            Podforum podforum = dbContext.podforums.FirstOrDefault(red => red.Id == id);
            List<Tema> tema = dbContext.temas.ToList();
            for (int i = 0; i < tema.Count; i++)
            {
                if (tema[i].PodforumId == id)
                {
                    ViewBag.Tema += tema[i];
                }
            }
            return View(podforum);
        }
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
            if (Korisnik.Count == 0)
            {
                if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com") && !korisnik.Email.Contains("@smart.edu.rs"))
                {
                    ViewBag.Error = "Molimo Vas unesite ispravnu i-mejl adresu!";
                    goto ErorGoto;
                }
            }
            else if (Korisnik.Count != 0)
            {
                for (int i = 0; i < Korisnik.Count; i++)
                {
                    if (username == Korisnik[i].KorisnickoIme.ToString() && email == Korisnik[i].Email.ToString())
                    {
                        ViewBag.Error = "Korisnik sa korisničkim imenom " + username + " " + "i unetom i-mejl adresom već postoji!";
                        if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com") && !korisnik.Email.Contains("@smart.edu.rs"))
                        {
                            ViewBag.Error2 = "Molimo Vas unesite ispravnu i-mejl adresu!";
                        }
                        goto ErorGoto;
                    }
                    if (username != Korisnik[i].KorisnickoIme.ToString() && email == Korisnik[i].Email.ToString())
                    {
                        ViewBag.Error = "Korisnik sa unetom i-mejl adresom već postoji!";
                        if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com") && !korisnik.Email.Contains("@smart.edu.rs"))
                        {
                            ViewBag.Error2 = "Molimo Vas unesite ispravnu i-mejl adresu!";
                        }
                        goto ErorGoto;
                    }
                    if (username == Korisnik[i].KorisnickoIme.ToString() && email != Korisnik[i].Email.ToString())
                    {
                        ViewBag.Error = "Korisnik sa korisničkim imenom " + username + " " + "već postoji!";
                        if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com") && !korisnik.Email.Contains("@smart.edu.rs"))
                        {
                            ViewBag.Error2 = "Molimo Vas unesite ispravnu i-mejl adresu!";
                        }
                        goto ErorGoto;
                    }
                    if (!korisnik.Email.Contains("@gmail.com") && !korisnik.Email.Contains("@yahoo.com") && !korisnik.Email.Contains("@smart.edu.rs"))
                    {
                        ViewBag.Error2 = "Molimo Vas unesite ispravnu i-mejl adresu!";
                        goto ErorGoto;
                    }
                }
            }
            dbContext.korisniks.Add(korisnik);
            korisnik.Tip_korisnika = "korisnik";
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Korisnik", "");
        ErorGoto:;
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
                    return RedirectToAction("Index", "Korisnik", "");
                }
            }
            return View();
        }
        public ActionResult IzlogujSe()
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if(korisnik == null)
            {
                return RedirectToAction("Index", "Korisnik");
            }
            else if(korisnik != null)
            {
                Session.Remove("korisnik");
                return RedirectToAction("Index", "Korisnik", "");
            }
            return View();
        }
        public ActionResult Promovisi()
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Korisnik");
            }
            else if (korisnik != null)
            {
                if (korisnik.Tip_korisnika == "admin")
                {
                    ViewBag.Korisniks = dbContext.korisniks.ToList();
                    return View();
                }
                return RedirectToAction("Index", "Korisnik");
            }
            return null;
        }
        [HttpPost]
        public ActionResult Promovisi(Podforum model)
        {
            Korisnik korisnik = dbContext.korisniks.FirstOrDefault
                    (red => red.KorisnickoIme == model.KorisnikKorisnickoIme);
            model.KorisnikId = korisnik.Id;
            dbContext.podforums.Add(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Korisnik", "");
        }
        public ActionResult Izmena(int id)
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Korisnik");
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
                    return RedirectToAction("Index", "Korisnik", "");
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