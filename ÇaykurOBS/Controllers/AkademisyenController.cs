using ÇaykurOBS.Models;
using Microsoft.AspNetCore.Mvc;


namespace ÇaykurOBS.Controllers
{
    public class AkademisyenController : Controller
    {
        private readonly ILogger<AkademisyenController> _logger;

        public AkademisyenController(ILogger<AkademisyenController> logger)
        {
            _logger = logger;
        }
        public IActionResult OgretmenPanel()
        {
            return View();
        }

        public IActionResult EpostaNumaraDogrulamaAkademisyen()
        {
            return View();
        }
        public IActionResult OgretmenGiris()
        {
            return View();
        }
        public IActionResult AkademisyenOgrenciGoruntuleme()
        {
            int akademisyenID = HttpContext.Session.GetInt32("AkademisyenId") ?? 0;
            if (akademisyenID == 0)
            {
                return View("OgretmenGiris");
            }
            ViewBag.Dersler = AkademisyenSorguKomutlari.AkademisyenDersGetir(akademisyenID);
            return View();
        }
        public IActionResult AkademisyenOgrenciveNotGoruntuleme()
        {
            int akademisyenID = HttpContext.Session.GetInt32("AkademisyenId") ?? 0;
            if (akademisyenID == 0)
            {
                return View("OgretmenGiris");
            }
            ViewBag.Dersler = AkademisyenSorguKomutlari.AkademisyenDersGetir(akademisyenID);
            return View();
        }
        public IActionResult AkademisyenOgrenciGoruntulemeNot()
        {
            int akademisyenID = HttpContext.Session.GetInt32("AkademisyenId") ?? 0;
            if (akademisyenID == 0)
            {
                return View("OgretmenGiris");
            }
            ViewBag.Dersler = AkademisyenSorguKomutlari.AkademisyenDersGetir(akademisyenID);
            return View();
        }
        public IActionResult AkademisyenDersTablosu()
        {
            int AkademisyenID = HttpContext.Session.GetInt32("AkademisyenId") ?? 0;
            List<Ders> Dersler = AkademisyenSorguKomutlari.AkademisyenDersGetir(AkademisyenID);
            return View(Dersler);
        }
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            return View("OgretmenGiris");
        }
        [HttpGet]
        public IActionResult NotDuzenleme(int DersID, int OgrenciID)
        {
            List<DersveNotlar> notListesi = AkademisyenSorguKomutlari.DersiAlanOgrencileriveNotlariGetir(DersID);
            DersveNotlar duzenlenecekNot = null;

            foreach (var not in notListesi)
            {
                if (not.DersID == DersID && not.OgrenciID == OgrenciID)
                {
                    duzenlenecekNot = not;
                    break;
                }
            }
            return View(duzenlenecekNot);
        }
        [HttpPost]
        public IActionResult NotDuzenleme(DersveNotlar YeniNot)
        {
            if (YeniNot.VizeNotu == null || YeniNot.FinalNotu == null)
            {
                ViewBag.HataMesaji = "Vize veya final notu boş bırakılamaz";
                return View("NotDuzenleme", YeniNot);
            }
            else if (YeniNot.VizeNotu > 100 || YeniNot.FinalNotu > 100 || YeniNot.FinalNotu < 0 || YeniNot.VizeNotu < 0 || (YeniNot.ButunlemeNotu.HasValue && (YeniNot.ButunlemeNotu < 0 || YeniNot.ButunlemeNotu > 100)))
            {
                ViewBag.HataMesaji = "Vize, Final ve Bütünleme notu 0 ile 100 arasında olmalıdır";
                return View("NotDuzenleme", YeniNot);
            }
            else
            {
                int sonuc = AkademisyenSorguKomutlari.NotDuzenle(YeniNot);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Not düzenlenemedi";
                    return View("NotDuzenleme", YeniNot);
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    List<DersveNotlar> derslerveNotlar = AkademisyenSorguKomutlari.DersiAlanOgrencileriveNotlariGetir(YeniNot.DersID);
                    return View("NotOgrenciTablosu", derslerveNotlar);
                }
            }
        }
        [HttpPost]
        public IActionResult NotGirme(int DersID)
        {
            ViewBag.SecilenDersID = DersID;
            ViewBag.Ogrenciler = AkademisyenSorguKomutlari.DersiAlanOgrencileriGetir(DersID);
            return View();
        }
        [HttpPost]
        public IActionResult NotKaydet(NotGirme YeniNot)
        {
            if (YeniNot.DersID == 0 || YeniNot.OgrenciID == 0)
            {
                ViewBag.HataMesaji = "Lütfen dersi ve öğrenciyi seçiniz.";
                return View("OgretmenPanel");
            }
            else
            {
                int sonuc = AkademisyenSorguKomutlari.NotGirme(YeniNot);
                if (sonuc == 1)
                {
                    ViewBag.BasariliMesaji = "Not girişi başarılı!";
                    return View("OgretmenPanel");
                }
                else
                {
                    ViewBag.HataMesaji = "Seçilen öğrencinin notu zaten girmişsiniz lütfen ilgili menüden düzenleyin.";
                    return View("NotGirme");
                }
            }
        }
        [HttpPost]
        public IActionResult AkademisyenGirisYap(Akademisyen YeniGiris)
        {
            string akademisyenNumarasi = YeniGiris.AkademisyenNumarasi;
            string sifre = YeniGiris.AkademisyenSifre;

            string adminNumarasi = "123456789";
            string adminSifre = "admin123";

            if (akademisyenNumarasi == adminNumarasi && sifre == adminSifre)
            {
                return View("~/Views/Home/AdminPanel.cshtml");
            }
            int sonuc = AkademisyenSorguKomutlari.AkademisyenKayitKontrol(akademisyenNumarasi, sifre);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz akademisyen numarası veya şifre.";
                return View("OgretmenGiris");
            }
            else
            {
                HttpContext.Session.SetInt32("AkademisyenId", sonuc);

                Akademisyen girisYapanHoca = AkademisyenSorguKomutlari.AkademisyenBilgiGetir(sonuc);
                if (girisYapanHoca != null)
                {
                    string adSoyad = $"{girisYapanHoca.AkademisyenIsim} {girisYapanHoca.AkademisyenSoyisim}";
                    HttpContext.Session.SetString("KullaniciAdSoyad", adSoyad);
                }

                if (sifre == "123456")
                {
                    return View("EpostaNumaraDogrulamaAkademisyen");
                }
                return View("OgretmenPanel");
            }
        }
        [HttpPost]
        public IActionResult EpostaNumaraDogrulamaAkademisyen(AkademisyenGiris YeniGiris)
        {
            string akademisyenNumarasi = YeniGiris.AkademisyenNumarasi;
            string email = YeniGiris.AkademisyenEmail;
            int sonuc = AkademisyenSorguKomutlari.EpostaNumaraDogrulamaAkademisyen(akademisyenNumarasi, email);
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
                AkademisyenSorguKomutlari.SifreSifirlamaAkademisyen(AkademisyenId, yeniSifre);
                return View("OgretmenGiris");
            }
        }
        [HttpPost]
        public IActionResult DersOgrenciTablosu(int DersID)
        {
            List<DersSecimleri> dersSecimleri = AkademisyenSorguKomutlari.DersiAlanOgrencileriGetir(DersID);
            return View(dersSecimleri);
        }
        [HttpPost]
        public IActionResult NotOgrenciTablosu(int DersID)
        {
            List<DersveNotlar> dersveNotlar = AkademisyenSorguKomutlari.DersiAlanOgrencileriveNotlariGetir(DersID);
            return View(dersveNotlar);
        }
    }
}
