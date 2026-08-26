using ÇaykurOBS.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult EpostaNumaraDogrulamaAkademisyen()
        {
            return View();
        }
        public IActionResult EpostaNumaraDogrulama()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Kayit()
        {
            return View();
        }
        public IActionResult AkademisyenKayit()
        {
            return View();
        }
        public IActionResult OgretmenGiris()
        {
            return View();
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
        [HttpPost]
        public IActionResult GirisYap(OgrenciGiris YeniGiris)
        {
            string ogrenciNumarasi = YeniGiris.OgrenciNumarasi;
            string sifre = YeniGiris.Sifre;

            string adminNumarasi = "123456789";
            string adminSifre = "admin123";

            if (ogrenciNumarasi == adminNumarasi && sifre == adminSifre)
            {
                return View("AdminPanel");
            }
            int sonuc = SorguKomutlari.OgrenciKayitKontrol(ogrenciNumarasi, sifre);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz öğrenci numarası veya şifre.";
                return View("Login");
            }
            else
            {
                HttpContext.Session.SetInt32("OgrenciId", sonuc);
                if (sifre == "123456")
                {
                    return View("EpostaNumaraDogrulama");
                }
                return View("OgrenciPanel");
            }
        }

            [HttpPost]
            public IActionResult AkademisyenGirisYap(AkademisyenGiris YeniGiris)
            {
                string akademisyenNumarasi = YeniGiris.AkademisyenNumarasi;
                string sifre = YeniGiris.AkademisyenSifre;

                string adminNumarasi = "123456789";
                string adminSifre = "admin123";

                if (akademisyenNumarasi == adminNumarasi && sifre == adminSifre)
                {
                    return View("AdminPanel");
                }
                int sonuc = SorguKomutlari.AkademisyenKayitKontrol(akademisyenNumarasi, sifre);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Geçersiz akademisyen numarası veya şifre.";
                    return View("OgretmenGiris");
                }
                else
                {
                    HttpContext.Session.SetInt32("AkademisyenId", sonuc);
                    if (sifre == "123456")
                    {
                        return View("EpostaNumaraDogrulamaAkademisyen");
                    }
                    return View("OgretmenPanel");
                }
            }

        public IActionResult VarsayilanSifreOgrenci(int ID)
        {
            List <Ogrenci> ogrenciler= SorguKomutlari.OgrenciGetir();
            SorguKomutlari.VarsayilanSifre(ID);
            return View("OgrenciTablosu", ogrenciler);
        }
  
        public IActionResult VarsayilanSifreAkademisyen(int ID)
        {
            List <Akademisyen> akademisyenler= SorguKomutlari.AkademisyenGetir();
            SorguKomutlari.VarsayilanSifreAkademisyen(ID);
            return View("AkademisyenTablosu", akademisyenler);
        }
      
        public IActionResult OgrenciSilme(int ID)
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetir();
            int sorgu= SorguKomutlari.OgrenciSilme(ID);
            if (sorgu == 0)
                return View("OgrenciTablosu", ogrenciler);
            else
                return View("OgrenciTablosu", ogrenciler);

        }
       
        [HttpPost]
       public IActionResult EpostaNumaraDogrulama(OgrenciGiris YeniGiris)
        {
            string ogrenciNumarasi = YeniGiris.OgrenciNumarasi;
            string email = YeniGiris.Email;
            int sonuc = SorguKomutlari.EpostaNumaraDogrulama(ogrenciNumarasi, email);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz öğrenci numarası veya e-posta.";
                return View("EpostaNumaraDogrulama");
            }
            else
            {
                HttpContext.Session.SetInt32("OgrenciId", sonuc);
                return View("SifremiUnuttumPaneli");
            }
        }
       
        [HttpPost]
        public IActionResult EpostaNumaraDogrulamaAkademisyen(AkademisyenGiris YeniGiris)
        {
            string akademisyenNumarasi = YeniGiris.AkademisyenNumarasi;
            string email = YeniGiris.AkademisyenEmail;
            int sonuc = SorguKomutlari.EpostaNumaraDogrulamaAkademisyen(akademisyenNumarasi, email);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz akademisyen numarası veya e-posta.";
                return View("EpostaNumaraDogrulamaAkademisyen");
            }
            else
            {
                HttpContext.Session.SetInt32("AkademisyenId", sonuc);
                return View("SifremiUnuttumPaneliAkademisyen");
            }
        }

        [HttpPost]
        public IActionResult SifremiUnuttumPaneli(Ogrenci YeniOgrenci)
        {
            if (YeniOgrenci.Sifre == null || YeniOgrenci.SifreTekrar == null)
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("SifremiUnuttumPaneli");
            }
            else if (YeniOgrenci.Sifre != YeniOgrenci.SifreTekrar)
            {
                ViewBag.HataMesaji = "Şifreler eşleşmiyor.";
                return View("SifremiUnuttumPaneli");
            }
            else if (YeniOgrenci.Sifre.Length < 6)
            {
                ViewBag.HataMesaji = "Şifre en az 6 karakter olmalıdır.";
                return View("SifremiUnuttumPaneli");
            }
            else
            {
               string yeniSifre = YeniOgrenci.Sifre;
               int OgrenciId = HttpContext.Session.GetInt32("OgrenciId")??0;
               SorguKomutlari.SifreSifirlama(OgrenciId, yeniSifre);
               return View("Login");
            }
        }
        [HttpPost]
        public IActionResult SifremiUnuttumPaneliAkademisyen(Akademisyen YeniAkademisyen)
        {
            if (YeniAkademisyen.AkademisyenSifre == null || YeniAkademisyen.AkademisyenSifreTekrar == null)
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("SifremiUnuttumPaneliAkademisyen");
            }
            else if (YeniAkademisyen.AkademisyenSifre != YeniAkademisyen.AkademisyenSifreTekrar)
            {
                ViewBag.HataMesaji = "Şifreler eşleşmiyor.";
                return View("SifremiUnuttumPaneliAkademisyen");
            }
            else if (YeniAkademisyen.AkademisyenSifre.Length < 6)
            {
                ViewBag.HataMesaji = "Şifre en az 6 karakter olmalıdır.";
                return View("SifremiUnuttumPaneliAkademisyen");
            }
            else
            {
                string yeniSifre = YeniAkademisyen.AkademisyenSifre;
                int AkademisyenId = HttpContext.Session.GetInt32("AkademisyenId") ?? 0;
                SorguKomutlari.SifreSifirlamaAkademisyen(AkademisyenId, yeniSifre);
                return View("OgretmenGiris");
            }
        }

        public IActionResult OgrenciTablosu()
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetir();
            return View(ogrenciler);
        }

        public IActionResult AkademisyenTablosu()
        {
            List<Akademisyen> akedemisyenler = SorguKomutlari.AkademisyenGetir();
            return View(akedemisyenler);
        }
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home"); 
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
    }
}
