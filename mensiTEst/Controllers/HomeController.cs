using permisionsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using permisionsApp.Entities;
using mensiTEst;
using System.Text.Json;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Globalization;

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

        public IActionResult Input(string? jmeno, string? heslo)
        {
            if (HttpContext.Session.GetString("UserIsLogged") != null)
            {
                if (HttpContext.Session.GetString("UserIsAdmin") == "true")
                {
                    ViewBag.IsAdmin = true;
                }
                //return View();
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
            if (HttpContext.Session.GetString("UserIsAdmin") == null)
            {
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("admSellectedUser", id.ToString());
            return View(Program.AllSubjects[id]);
        }

        [Route("AdminSubjectRightsAction/{nazev}")]
        public IActionResult AdminSubjectRightsAction(string nazev)
        {
            if (HttpContext.Session.GetString("UserIsAdmin") == null)
            {
                return RedirectToAction("Index");
            }

            int id = Int32.Parse(HttpContext.Session.GetString("admSellectedUser"));
            Program.AllSubjects[id].Permsions.First(p => p.Nazev == nazev).Grant = !Program.AllSubjects[id].Permsions.First(p => p.Nazev == nazev).Grant;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Program.AllSubjects, options);
            System.IO.File.WriteAllText(@"SecretStuff.txt", json);

            return RedirectToAction("AdminSubjectRights", "Home", new { id = id });
        }

        public IActionResult AddPerm(string nazev, string povoleno, string kod)
        {
            if (HttpContext.Session.GetString("UserIsAdmin") == null)
            {
                return RedirectToAction("Index");
            }


            bool x = false;
            if (povoleno == "True")
            {
                x = true;
            } else { x = false; }
            Permsion p = new Permsion(nazev, x);
            int id = Int32.Parse(HttpContext.Session.GetString("admSellectedUser"));

            Program.AllSubjects[id].Permsions.Add(p);

            Console.WriteLine(nazev);
            Console.WriteLine(povoleno);
            Console.WriteLine(kod);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Program.AllSubjects, options);
            System.IO.File.WriteAllText(@"SecretStuff.txt", json);

            return RedirectToAction("AdminSubjectRights", "Home", new { id = id });
        }


        public IActionResult MaOpravneni()
        {
            Subject mojeOpr = Program.AllSubjects.FirstOrDefault(s => s.Jmeno == HttpContext.Session.GetString("UserIsLogged"));

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"http://localhost:5066/home/SeznamOpravneni/{mojeOpr.kodUzivatele}", QRCodeGenerator.ECCLevel.H))
            using (BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData))

            {
                string s = "";
                byte[] qrCodeImage = qrCode.GetGraphic(8);



                /*
                Console.WriteLine(qrCodeImage.LongLength);
                Console.WriteLine("-----------------");
                for(int i = 0; i < qrCodeImage.Length; i++)
                {
                    s += qrCodeImage[i].ToString();
                        if(i < qrCodeImage.Length -1)
                        {
                            s += ",";
                        }
                    //za tento kód bych mìl jít do vìzení
                }
                ViewBag.s = s;
                Console.WriteLine(s);

                Je-li ti život milý prosím nech to být.. jinak nech je bùh milostivý
                */



                MoukaVajckoStrouha rizek = new MoukaVajckoStrouha(mojeOpr, qrCodeImage);
                return View(rizek);
            }
        }
    }
    
}
