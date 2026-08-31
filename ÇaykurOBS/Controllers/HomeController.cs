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
        public IActionResult AdminPanel()
        {
            return View();
        }
        public IActionResult OgrenciPanel()
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
        public IActionResult DersEkleme()
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            return View();
        }
        public IActionResult DersDuzenleme()
        {
            ViewBag.Akademisyenler = SorguKomutlari.AkademisyenGetir();
            return View();
        }
        public IActionResult VarsayilanSifreOgrenci(int ID)
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetir();
            SorguKomutlari.VarsayilanSifre(ID);
            return View("OgrenciTablosu", ogrenciler);
        }
        public IActionResult VarsayilanSifreAkademisyen(int ID)
        {
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetir();
            SorguKomutlari.VarsayilanSifreAkademisyen(ID);
            return View("AkademisyenTablosu", akademisyenler);
        }
        public IActionResult DersSilme(int ID)
        {
            int sorgu = SorguKomutlari.DersSilme(ID);
            List<Ders> dersler = SorguKomutlari.DersGetir();
            if (sorgu == 0)
                return View("DersTablosu", dersler);
            else
                return View("DersTablosu", dersler);
        }
        public IActionResult OgrenciSilme(int ID)
        {
            int sorgu = SorguKomutlari.OgrenciSilme(ID);
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetir();
            if (sorgu == 0)
                return View("OgrenciTablosu", ogrenciler);
            else
                return View("OgrenciTablosu", ogrenciler);

        }
        public IActionResult AkademisyenSilme(int ID)
        {
            int sorgu = SorguKomutlari.AkademisyenSilme(ID);
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetir();
            if (sorgu == 0)
                return View("AkademisyenTablosu", akademisyenler);
            else
                return View("AkademisyenTablosu", akademisyenler);

        }
        public IActionResult DersTablosu()
        {
            List<Ders> Dersler = SorguKomutlari.DersGetir();
            return View(Dersler);
        }
        public IActionResult OgrenciTablosu()
        {
            List<Ogrenci> ogrenciler = SorguKomutlari.OgrenciGetir();
            return View(ogrenciler);
        }
        public IActionResult AkademisyenTablosu()
        {
            List<Akademisyen> akademisyenler = SorguKomutlari.AkademisyenGetir();
            return View(akademisyenler);
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

                List<Ogrenci> tumOgrenciler = SorguKomutlari.OgrenciGetir();

                foreach (var ogrenci in tumOgrenciler)
                {
                    if (ogrenci.OgrenciID == sonuc)
                    {
                        string adSoyad = $"{ogrenci.Isim} {ogrenci.Soyisim}";
                        HttpContext.Session.SetString("KullaniciAdSoyad", adSoyad);
                        break;
                    }
                }
                if (sifre == "123456")
                {
                    return View("EpostaNumaraDogrulama");
                }
                return View("OgrenciPanel");
            }
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
    }
}
