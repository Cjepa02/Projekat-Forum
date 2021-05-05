using Forum.Models;
using Forum.Models.Dal;
using Forum.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Forum.Controllers
{
    public class KorisnikController : Controller
    {
        private ForumContext dbContext = new ForumContext();

        public ActionResult Index()
        {
            //Nizovi su mi nepotrebni, mogao sam samo uz liste da napravim ovo (možda)
            List<Podforum> podforums = dbContext.podforums.ToList();
            List<Korisnik> korisniks = dbContext.korisniks.ToList();
            int[] korisnikid = new int[korisniks.Count];
            int[] podforumkorisnikid = new int[podforums.Count];
            List<int> korisnikidlista = new List<int>();
            for(int i = 0; i < korisniks.Count; i++)
            {
                korisnikid[i] = korisniks[i].Id;
            }
            korisnikidlista.AddRange(korisnikid);
            for (int i = 0; i < podforums.Count; i++) 
            {
                podforumkorisnikid[i] = podforums[i].KorisnikId;
            }
            for(int i = 0; i < podforums.Count; i++)
            {
                if (!korisnikidlista.Contains(podforumkorisnikid[i]))
                {
                    int korisnikidpodforum = podforumkorisnikid[i];
                    Podforum podforum = dbContext.podforums.FirstOrDefault(red => red.KorisnikId == korisnikidpodforum);
                    podforum.KorisnikId = -1;
                    podforum.KorisnikKorisnickoIme = null;
                    dbContext.SaveChanges();
                }
            }
            ViewBag.Podforums = podforums;
            return View();
        }

        public ActionResult Podforum(int id)
        {
            Podforum podforum = dbContext.podforums.FirstOrDefault(red => red.Id == id);
            List<Tema> tema = dbContext.temas.ToList();
            List<Tema> temapodforum = new List<Tema>();
            List<Tema> temapodforumskracentekst = new List<Tema>();
            for (int i = 0; i < tema.Count; i++)
            {
                if (tema[i].PodforumId == id)
                {
                    temapodforum.Add(tema[i]);
                }
            }
            temapodforumskracentekst = temapodforum;
            for (int i = 0; i < temapodforum.Count; i++)
            {
                string tekst = temapodforum[i].Tekst;
                int brojkaraktera = tekst.Count();
                if (brojkaraktera > 115)
                {
                    temapodforumskracentekst[i].Tekst = tekst.Substring(0, 115) + "...";
                }
            }
            ViewBag.Tema = temapodforumskracentekst;
            ViewBag.PodforumId = podforum.Id;
            ViewBag.PodforumNaslov = podforum.Naslov;
            ViewBag.PodforumModerator = podforum.KorisnikId;
            return View(podforum);
        }
        public ActionResult TemaIzmena(int id)
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                goto ErorGoto;
            }
            else if (korisnik != null)
            {
                Tema tema = dbContext.temas.FirstOrDefault(red => red.Id == id);
                int podforumid = tema.PodforumId;
                Podforum podforum = dbContext.podforums.FirstOrDefault(red => red.Id == podforumid);
                int moderator = podforum.KorisnikId;
                if (tema.KorisnikId == korisnik.Id || tema.KorisnikId == moderator)
                {
                    ViewBag.Tema = tema;
                    return View();
                }
                else
                    goto ErorGoto;
            }
        ErorGoto:;
            return RedirectToAction("Index", "Korisnik", "");
        }
        [HttpPost]
        public ActionResult TemaIzmena(Tema model)
        {
            Tema tema = dbContext.temas.FirstOrDefault(red => red.Id == model.Id);
            tema.Naslov = model.Naslov;
            tema.Tekst = model.Tekst;
            tema.Izmenjen = true;
            int podforumid = tema.PodforumId;
            dbContext.SaveChanges();
            return RedirectToAction("Podforum", "Korisnik", new { id = podforumid });
        }
        public ActionResult Tema()
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            if (korisnik == null)
            {
                return RedirectToAction("Login", "Korisnik");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Tema(Tema tema, int id)
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            int korisnikid = korisnik.Id;
            tema.KorisnikId = korisnikid;
            tema.KorisnikKorisnickoIme = korisnik.KorisnickoIme;
            tema.PodforumId = id;
            DateTime datum = DateTime.Now;
            tema.DatumVreme = datum;
            tema.Izmenjen = false;
            dbContext.temas.Add(tema);
            dbContext.SaveChanges();
            return RedirectToAction("Podforum", "Korisnik", new { id = id });
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
            if (korisnik.Ime == null || korisnik.Prezime == null || korisnik.KorisnickoIme == null || korisnik.Email == null || korisnik.Lozinka == null)
            {
                ViewBag.Error = "Molimo Vas da unesete sve potrebe podatke!";
                goto ErorGoto;
            }
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
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Korisnik");
            }
            else if (korisnik != null)
            {
                Session.Remove("korisnik");
                return RedirectToAction("Index", "Korisnik", "");
            }
            return View();
        }

        public ActionResult Promovisi()
        {
            Forum.Models.Korisnik korisnik = Session["korisnik"] as Forum.Models.Korisnik;
            List<Podforum> podforum = dbContext.podforums.ToList();
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Korisnik");
            }
            else if (korisnik != null)
            {
                if (korisnik.Tip_korisnika == "admin")
                {
                    ViewBag.Korisniks = dbContext.korisniks.ToList();
                    ViewBag.Podforums = dbContext.podforums.ToList();
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
            Podforum podforum = dbContext.podforums.FirstOrDefault
                     (red => red.Naslov == model.Naslov);
            podforum.KorisnikId = korisnik.Id;
            podforum.KorisnikKorisnickoIme = korisnik.KorisnickoIme;
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Korisnik", "");
        }
        public ActionResult NapraviPodforum()
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
                    ViewBag.Podforums = dbContext.podforums.ToList();
                    return View();
                }
                return RedirectToAction("Index", "Korisnik");
            }
            return null;
        }
        [HttpPost]
        public ActionResult NapraviPodforum(Podforum model)
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
            if (paketic.Lozinka == null || paketic.TrenutnaLozinka == null)
            {
                ViewBag.Error = "Molimo Vas da unesete sve potrebe podatke!";
                goto ErorGoto;
            }
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
            else if (model.TrenutnaLozinka != paketic.TrenutnaLozinka)
            {
                ViewBag.Error = "Trenutna lozinka netačna!";
            }
        ErorGoto:;
            return View();
        }
    }
}