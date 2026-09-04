using ÇaykurOBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace ÇaykurOBS.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly ILogger<OgrenciController> _logger;
     
        public OgrenciController(ILogger<OgrenciController> logger)
        {
            _logger = logger;
        }
        public IActionResult OgrenciDersTablosu()
        {
            int OgrenciID = HttpContext.Session.GetInt32("OgrenciId") ?? 0;
            List<DersSecimleri> DerslerSecimleri = OgrenciSorguKomutlari.OgrenciDersGetir(OgrenciID);
            return View(DerslerSecimleri);
        }
        public IActionResult OgrenciPanel()
        {
            int ogrenciID = HttpContext.Session.GetInt32("OgrenciId") ?? 0;
            if (ogrenciID == 0)
            {
                return View("Login");
            }
            return View();
        }
        public IActionResult DersTablosu()
        {
            int OgrenciID = HttpContext.Session.GetInt32("OgrenciId") ?? 0;
            ViewBag.MevcutKredi = OgrenciSorguKomutlari.OgrenciMevcutKrediGetir(OgrenciID);
            List<Ders> dersler = OgrenciSorguKomutlari.DersleriGetir();
            return View(dersler);
        }
        public IActionResult EpostaNumaraDogrulama()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SifremiUnuttumPaneli()
        {
            return View();
        }
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [HttpGet]
        public IActionResult NotOgrenciTablosu(int OgrenciID)
        {
            OgrenciID = HttpContext.Session.GetInt32("OgrenciId") ?? 0;
            List<DersveNotlar> dersveNotlar = OgrenciSorguKomutlari.OgrenciNotGetir(OgrenciID);
            return View(dersveNotlar);
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
                return View("~/Views/Home/AdminPanel.cshtml");
            }
            int sonuc = OgrenciSorguKomutlari.OgrenciKayitKontrol(ogrenciNumarasi, sifre);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz öğrenci numarası veya şifre.";
                return View("Login");
            }
            else
            {
                HttpContext.Session.SetInt32("OgrenciId", sonuc);

                List<Ogrenci> tumOgrenciler = OgrenciSorguKomutlari.OgrenciGetir();

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
            int sonuc = OgrenciSorguKomutlari.EpostaNumaraDogrulama(ogrenciNumarasi, email);
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
               OgrenciSorguKomutlari.SifreSifirlama(OgrenciId, yeniSifre);
               return View("Login");
            }               
        }
        [HttpPost]
        public IActionResult DersSeciminiKaydet(List<int> secilenDersler)
        {
            int OgrenciID = HttpContext.Session.GetInt32("OgrenciId") ?? 0;

            if (secilenDersler != null && secilenDersler.Count > 0)
            {
             OgrenciSorguKomutlari.TopluDersSecimiKaydet(OgrenciID, secilenDersler);
            }
            else
            {
                ViewBag.HataMesaji = "Lütfen en az bir ders seçin.";
            }
            ViewBag.MevcutKredi = OgrenciSorguKomutlari.OgrenciMevcutKrediGetir(OgrenciID);
            List<DersSecimleri> DerslerSecimleri = OgrenciSorguKomutlari.OgrenciDersGetir(OgrenciID);
            return View("OgrenciDersTablosu",DerslerSecimleri);
        }
    }
}
