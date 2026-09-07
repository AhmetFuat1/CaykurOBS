using ÇaykurOBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Diagnostics;

namespace ÇaykurOBS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
     
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View("~/Views/Home/AdminPanel.cshtml");
        }
        public IActionResult Kayit()
        {
            return View();
        }
        public IActionResult AkademisyenKayit()
        {
            return View();
        }
        public IActionResult DersEkleme()
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            return View();
        }
        public IActionResult VarsayilanSifreOgrenci(int ID)
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetirHepsi();
            SorguKomutlari.VarsayilanSifre(ID);
            return View("OgrenciTablosu", ogrenciler);
        }
        public IActionResult VarsayilanSifreAkademisyen(int ID)
        {
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetirHepsi();
            SorguKomutlari.VarsayilanSifreAkademisyen(ID);
            return View("AkademisyenTablosu", akademisyenler);
        }
        public IActionResult DersSilme(int ID)
        {
            SorguKomutlari.DersSilme(ID);
            List<Ders> dersler = SorguKomutlari.DersGetir();
            return View("DersTablosu", dersler);
        }
        public IActionResult OgrenciSilme(int ID)
        {
            SorguKomutlari.OgrenciSilme(ID);
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetirHepsi();
            return View("OgrenciTablosu", ogrenciler);
        }
        public IActionResult AkademisyenSilme(int ID)
        {
            SorguKomutlari.AkademisyenSilme(ID);
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetirHepsi();
            return View("AkademisyenTablosu", akademisyenler);
        }
        public IActionResult OgrenciYenidenEkle(int ID)
        {
            SorguKomutlari.OgrenciYenidenEkle(ID);
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetirHepsi();
            return View("OgrenciTablosu", ogrenciler);
        }
        public IActionResult AkademisyenYenidenEkle(int ID)
        {
            SorguKomutlari.AkademisyenYenidenEkle(ID);
            List<Akademisyen> akademisyen = SorguKomutlari.AkademisyenGetirHepsi();
            return View("AkademisyenTablosu", akademisyen);
        }
        public IActionResult DersSecimSilme(int DersID, int OgrenciID)
        {
            SorguKomutlari.DersSecimSilme(DersID, OgrenciID);
            List<DersSecimleri> onayBekleyenler = SorguKomutlari.DersOnayTablosu();
            return View("DersOnayTablosu", onayBekleyenler);
        }
        public IActionResult DersTablosu()
        {
            List<Ders> Dersler = SorguKomutlari.DersGetir();
            return View(Dersler);
        }
        public IActionResult OgrenciTablosu()
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetirHepsi();
            return View(ogrenciler);
        }
        public IActionResult AkademisyenTablosu()
        {
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetirHepsi();
            return View(akademisyenler);
        }
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Ogrenci/Login.cshtml");
        }
        public IActionResult DersOnayTablosu()
        {
            List<DersSecimleri> onayBekleyenler = SorguKomutlari.DersOnayTablosu();
            return View(onayBekleyenler);
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
        public IActionResult DersSecimOnaylama(int DersID, int OgrenciID)
        {
            SorguKomutlari.DersSecimOnaylama(DersID, OgrenciID);
            List<DersSecimleri> onayBekleyenler = SorguKomutlari.DersOnayTablosu();
            return View("DersOnayTablosu", onayBekleyenler);
        }
        public IActionResult OnayliDersSilme(int DersID,int OgrenciID)
        {
            SorguKomutlari.DersSecimSilme(DersID, OgrenciID);
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            List<Ders> dersler = SorguKomutlari.DersGetir();
            Ders duzenlenecekDers = null;

            foreach (var ders in dersler)
            {
                if (ders.DersID == DersID)
                {
                    duzenlenecekDers = ders;
                    break;
                }
            }
            ViewBag.DersiAlanOgrenciler = SorguKomutlari.DersiAlanOgrencileriGetir(DersID);
            return View("DersDuzenleme", duzenlenecekDers);
        }
        [HttpGet]
        public IActionResult DersDuzenleme(int ID)
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            List<Ders> dersler = SorguKomutlari.DersGetir();
            Ders duzenlenecekDers = null;

            foreach (var ders in dersler)
            {
                if (ders.DersID == ID)
                {
                    duzenlenecekDers = ders;
                    break;
                }
            }
            ViewBag.DersiAlanOgrenciler = SorguKomutlari.DersiAlanOgrencileriGetir(ID);
            return View(duzenlenecekDers);
        }
        [HttpPost]
        public IActionResult DersEkleme(Ders YeniDers)
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            if (string.IsNullOrEmpty(YeniDers.DersAdi) || string.IsNullOrEmpty(YeniDers.DersKodu) || YeniDers.DersKredisi == null || YeniDers.OgretmenID == 0)
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("DersEkleme");
            }
            else if (YeniDers.DersKodu.Length != 5)
            {
                ViewBag.HataMesaji = "Ders kodu 5 haneli olmalıdır.";
                return View("DersEkleme");
            }
            else if (YeniDers.DersKredisi < 1 || YeniDers.DersKredisi > 10)
            {
                ViewBag.HataMesaji = "Ders kredisi 1 ile 10 arasında olmalıdır.";
                return View("DersEkleme");
            }
            else
            {
                int sonuc = SorguKomutlari.DersEkle(YeniDers);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Bu Ders zaten kayıtlı.";
                    return View("DersEkleme");
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    return View("AdminPanel");
                }
            }
        }
        [HttpPost]
        public IActionResult DersDuzenleme(Ders YeniDers)
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            if (string.IsNullOrEmpty(YeniDers.DersAdi) || string.IsNullOrEmpty(YeniDers.DersKodu) || YeniDers.DersKredisi == null || YeniDers.OgretmenID == 0)
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("DersDuzenleme", YeniDers);
            }
            else if (YeniDers.DersKodu.Length != 5)
            {
                ViewBag.HataMesaji = "Ders kodu 5 haneli olmalıdır.";
                return View("DersDuzenleme", YeniDers);
            }
            else if (YeniDers.DersKredisi < 1 || YeniDers.DersKredisi > 10)
            {
                ViewBag.HataMesaji = "Ders kredisi 1 ile 10 arasında olmalıdır.";
                return View("DersDuzenleme",YeniDers);
            }
            else
            {
                int sonuc = SorguKomutlari.DersDuzenle(YeniDers.DersID, YeniDers);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Bu Ders zaten kayıtlı.";
                    return View("DersDuzenleme", YeniDers);
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    List<Ders> dersler = SorguKomutlari.DersGetir();
                    return View("DersTablosu", dersler);
                }
            }

        }
        [HttpPost]
        public IActionResult KayitOl(Ogrenci YeniOgrenci)
        {
            if (string.IsNullOrEmpty(YeniOgrenci.OgrenciNumarasi) || string.IsNullOrEmpty(YeniOgrenci.Isim) || string.IsNullOrEmpty(YeniOgrenci.Soyisim))
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("Kayit");
            }
            else if (YeniOgrenci.OgrenciNumarasi.Length != 9)
            {
                ViewBag.HataMesaji = "Öğrenci numarası 9 haneli olmalıdır.";
                return View("Kayit");
            }

            else if(!YeniOgrenci.Soyisim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) || !YeniOgrenci.Isim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ViewBag.HataMesaji = "İsim ve soyisim sadece harflerden oluşmalıdır.";
                return View("Kayit");
            }
            else
            {
                int sonuc = SorguKomutlari.OgrenciKayitEkle(YeniOgrenci);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Bu öğrenci numarası veya e-posta zaten kayıtlı.";
                    return View("Kayit");
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    return View("AdminPanel");
                }
            }
        }
        [HttpPost]
        public IActionResult AkademisyenKayitOl(Akademisyen YeniAkademisyen)
        {
            if (string.IsNullOrEmpty(YeniAkademisyen.AkademisyenNumarasi) || string.IsNullOrEmpty(YeniAkademisyen.AkademisyenIsim) || string.IsNullOrEmpty(YeniAkademisyen.AkademisyenSoyisim))
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("AkademisyenKayit");
            }
            else if (YeniAkademisyen.AkademisyenNumarasi.Length != 9)
            {
                ViewBag.HataMesaji = "Akademisyen numarası 9 haneli olmalıdır.";
                return View("AkademisyenKayit");
            }
            else if (!long.TryParse(YeniAkademisyen.AkademisyenNumarasi, out _))
            {
                ViewBag.HataMesaji = "Akademisyen numarası sadece rakamlardan oluşmalıdır.";
                return View("AkademisyenKayit");
            }

            else if (!YeniAkademisyen.AkademisyenSoyisim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) || !YeniAkademisyen.AkademisyenIsim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ViewBag.HataMesaji = "İsim ve soyisim sadece harflerden oluşmalıdır.";
                return View("AkademisyenKayit");
            }
            else
            {
                int sonuc = SorguKomutlari.AkademisyenKayitEkle(YeniAkademisyen);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Bu akademisyen numarası veya e-posta zaten kayıtlı.";
                    return View("AkademisyenKayit");
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    return View("AdminPanel");
                }
            }
        } 
    }
}
