using ÇaykurOBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;


namespace ÇaykurOBS.Controllers
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
        public IActionResult OgretmenGiris()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KayitOl(Ogrenci YeniOgrenci)
        {
            if (string.IsNullOrEmpty(YeniOgrenci.OgrenciNumarasi) || string.IsNullOrEmpty(YeniOgrenci.Isim) || string.IsNullOrEmpty(YeniOgrenci.Soyisim) || string.IsNullOrEmpty(YeniOgrenci.Email) || string.IsNullOrEmpty(YeniOgrenci.Sifre) || string.IsNullOrEmpty(YeniOgrenci.SifreTekrar))
            {
                ViewBag.HataMesaji = "Lütfen tüm alanları doldurun.";
                return View("Kayit");
            }
            else if (YeniOgrenci.OgrenciNumarasi.Length != 9)
            {
                ViewBag.HataMesaji = "Öğrenci numarası 9 haneli olmalıdır.";
                return View("Kayit");
            }
            else if (!long.TryParse(YeniOgrenci.OgrenciNumarasi, out _))
            {
                ViewBag.HataMesaji = "Öğrenci numarası sadece rakamlardan oluşmalıdır.";
                return View("Kayit");
            }
            else if (YeniOgrenci.Sifre != YeniOgrenci.SifreTekrar)
            {
                ViewBag.HataMesaji = "Şifreler eşleşmiyor.";
                return View("Kayit");
            }
            else if (YeniOgrenci.Sifre.Length < 6)
            {
                ViewBag.HataMesaji = "Şifre en az 6 karakter olmalıdır.";
                return View("Kayit");
            }
            else if(!Regex.IsMatch(YeniOgrenci.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                ViewBag.HataMesaji = "Geçerli bir e-posta adresi girin.";
                return View("Kayit");
            }
            else if(!YeniOgrenci.Soyisim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) || !YeniOgrenci.Isim.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ViewBag.HataMesaji = "İsim ve soyisim sadece harflerden oluşmalıdır.";
                return View("Kayit");
            }
            else
            {
                SorguKomutlari sorgu = new SorguKomutlari();
                int sonuc = sorgu.OgrenciKayitEkle(YeniOgrenci);
                if (sonuc == 0)
                {
                    ViewBag.HataMesaji = "Bu öğrenci numarası veya e-posta zaten kayıtlı.";
                    return View("Kayit");
                }
                else
                {
                    ViewBag.BasariliMesaji = "Kayıt başarılı!";
                    return View("Login");
                }
            }
        }
        [HttpPost]
        public IActionResult GirisYap(Giris YeniGiris)
        {
            string ogrenciNumarasi = YeniGiris.OgrenciNumarasi;
            string sifre = YeniGiris.Sifre;

            string adminNumarasi = "123456789";
            string adminSifre = "admin123";

            if (ogrenciNumarasi == adminNumarasi && sifre == adminSifre)
            {
                return View("AdminPanel");
            }
            SorguKomutlari sorgu = new SorguKomutlari();
            int sonuc = sorgu.OgrenciKayitKontrol(ogrenciNumarasi, sifre);
            if (sonuc == 0)
            {
                ViewBag.HataMesaji = "Geçersiz öğrenci numarası veya şifre.";
                return View("Login");
            }
            else
            {
                HttpContext.Session.SetInt32("OgrenciId", sonuc);
                if(sifre== "123456")
                {
                    return View("SifremiUnuttumPaneli");
                }
                return View("OgrenciPanel");       
            }
        }
        public IActionResult EpostaNumaraDogrulama()
        {
            return View();
        }
     /*   [HttpPost]
       public IActionResult EpostaNumaraDogrulama(Giris YeniGiris)
        {
            string ogrenciNumarasi = YeniGiris.OgrenciNumarasi;
            string email = YeniGiris.Email;
            using (SqlConnection baglanti = new SqlConnection(baglantiAdresi))
            {
                baglanti.Open();
                string sqlVeriCekme = "SELECT OgrenciID FROM Ogrenciler WHERE Numara = @pNumara AND Eposta = @pEposta";
                SqlCommand komut = new SqlCommand(sqlVeriCekme, baglanti);
                {
                    komut.Parameters.AddWithValue("@pNumara", ogrenciNumarasi);
                    komut.Parameters.AddWithValue("@pEposta", email);
                    object SıfırlamaId = komut.ExecuteScalar();

                    if (SıfırlamaId != null)
                    {
                        int donusmusId = Convert.ToInt32(SıfırlamaId);
                        HttpContext.Session.SetInt32("SifirlamaId", donusmusId);

                        return View("SifremiUnuttumPaneli");
                    }
                    else
                    {
                        ViewBag.HataMesaji = "Geçersiz öğrenci numarası veya e-postası.";
                        return View("EpostaNumaraDogrulama");
                    }
                }
            }
        }*/

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
               SorguKomutlari sorgu = new SorguKomutlari();
               sorgu.SifreSifirlama(OgrenciId, yeniSifre);
                return View("Login");
            }
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
