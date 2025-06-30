using permisionsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using permisionsApp.Entities;
using mensiTEst;

namespace permisionsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserIsAdmin") == "true")
            {
                return RedirectToAction("Input");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Input(string jmeno, string heslo)
        {
            if (HttpContext.Session.GetString("UserIsLogged") != null)
            {
                if(HttpContext.Session.GetString("UserIsAdmin") == "true")
                {
                    ViewBag.IsAdmin = true;
                }
                return View();
            }

            for (int i = 0; i < Program.AllSubjects.Count; i++)
            {
                if (jmeno == Program.AllSubjects[i].Jmeno && heslo == Program.AllSubjects[i].Heslo)
                {
                    HttpContext.Session.SetString("UserIsLogged", jmeno);
                    if (Program.AllSubjects[i].IsAdmin)
                    {
                        HttpContext.Session.SetString("UserIsAdmin", "true");
                        ViewBag.IsAdmin = true;
                        return View(Program.AllSubjects);
                    }

                    //Console.WriteLine("nevim");
                    //Console.WriteLine(HttpContext.Session.GetString("UserIsAdmin"));

                    return View();
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult SeznamOpravneni(string id)
        {
            if (HttpContext.Session.GetString("UserIsLogged") == null)
            {
                return RedirectToAction("Index");
            }
            Console.WriteLine($"Tvé id je:{id}");
            Subject subjektNaStranku = new Subject();
            bool x = false;
            foreach (Subject s in Program.AllSubjects)
            {
                if (s.kodUzivatele == id)
                {
                    subjektNaStranku = s;
                    x = true;
                    break;
                }
            }
            if (x!)
            { RedirectToAction("Input"); }
            return View(subjektNaStranku);
        }

        public IActionResult AdminSubjectRights(int id)
        {
            HttpContext.Session.SetString("admSellectedUser", id.ToString());
            return View(Program.AllSubjects[id]);
        }

        [Route("AdminSubjectRightsAction/{nazev}")]
        public IActionResult AdminSubjectRightsAction(string nazev)
        {
            int id = Int32.Parse(HttpContext.Session.GetString("admSellectedUser"));
            Program.AllSubjects[id].Permsions.First(p => p.Nazev == nazev).Grant = !Program.AllSubjects[id].Permsions.First(p => p.Nazev == nazev).Grant;
            return RedirectToAction("AdminSubjectRights", "Home", new { id = id });
        }

    }
    
}
