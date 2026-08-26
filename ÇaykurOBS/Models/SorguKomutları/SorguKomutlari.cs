using Microsoft.Data.SqlClient;
using System;
namespace ÇaykurOBS.Models
{
    public class SorguKomutlari
    {
        public static SqlConnection BaglantiAcma()
        {
            string baglantiAdresi = "Server=localhost\\SQLEXPRESS;Database=CaykurOBSDB;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
            return baglanti;
        }

        public static int OgrenciKayitEkle(Ogrenci YeniOgrenci)
        {
            SqlConnection baglanti = BaglantiAcma();

            string eposta = $"{YeniOgrenci.OgrenciNumarasi}@caykur.gov.tr";
            string sifre = "123456";

            string kontrolSorgusu = "SELECT COUNT(*) FROM Ogrenciler WHERE Numara = @pNumara OR Eposta = @pEposta";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);

            kontrol.Parameters.AddWithValue("@pNumara", YeniOgrenci.OgrenciNumarasi);
            kontrol.Parameters.AddWithValue("@pEposta", eposta);

            int kayitSayisi = (int)kontrol.ExecuteScalar();

            if (kayitSayisi > 0)
            {
                baglanti.Close();
                return 0;
            }
            else
            {
                string kayit = "INSERT INTO Ogrenciler (Ad, Soyad, Numara, Eposta, Sifre) VALUES (@OAd, @Soyad, @Numara, @Eposta, @Sifre)";
                SqlCommand kayitkomutu = new SqlCommand(kayit, baglanti);
                kayitkomutu.Parameters.AddWithValue("@OAd", YeniOgrenci.Isim);
                kayitkomutu.Parameters.AddWithValue("@Soyad", YeniOgrenci.Soyisim);
                kayitkomutu.Parameters.AddWithValue("@Numara", YeniOgrenci.OgrenciNumarasi);
                kayitkomutu.Parameters.AddWithValue("@Eposta", eposta);
                kayitkomutu.Parameters.AddWithValue("@Sifre", sifre);
                kayitkomutu.ExecuteNonQuery();
                baglanti.Close();
                return 1;
            }
        }

        public static int AkademisyenKayitEkle(Akademisyen YeniAkademisyen)
        {
            SqlConnection baglanti = BaglantiAcma();

            string eposta = $"{YeniAkademisyen.AkademisyenNumarasi}@caykur.gov.tr";
            string sifre = "123456";

            string kontrolSorgusu = "SELECT COUNT(*) FROM Ogretmenler WHERE OgretmenNumarasi = @pOgretmenNumarasi OR OgretmenEpostasi = @pOgretmenEpostasi";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);

            kontrol.Parameters.AddWithValue("@pOgretmenNumarasi", YeniAkademisyen.AkademisyenNumarasi);
            kontrol.Parameters.AddWithValue("@pOgretmenEpostasi", eposta);

            int kayitSayisi = (int)kontrol.ExecuteScalar();

            if (kayitSayisi > 0)
            {
                baglanti.Close();
                return 0;
            }
            else
            {
                string kayit = "INSERT INTO Ogretmenler (OgretmenAdi, OgretmenSoyadi, OgretmenNumarasi, OgretmenEpostasi, OgretmenSifresi) VALUES (@OgretmenAdi, @OgretmenSoyadi, @OgretmenNumarasi, @OgretmenEpostasi, @OgretmenSifresi)";
                SqlCommand kayitkomutu = new SqlCommand(kayit, baglanti);
                kayitkomutu.Parameters.AddWithValue("@OgretmenAdi", YeniAkademisyen.AkademisyenIsim);
                kayitkomutu.Parameters.AddWithValue("@OgretmenSoyadi", YeniAkademisyen.AkademisyenSoyisim);
                kayitkomutu.Parameters.AddWithValue("@OgretmenNumarasi", YeniAkademisyen.AkademisyenNumarasi);
                kayitkomutu.Parameters.AddWithValue("@OgretmenEpostasi", eposta);
                kayitkomutu.Parameters.AddWithValue("@OgretmenSifresi", sifre);
                kayitkomutu.ExecuteNonQuery();
                baglanti.Close();
                return 1;
            }
        }

        public static int OgrenciKayitKontrol(string OgrenciNumarasi, string Sifre)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "SELECT OgrenciID FROM Ogrenciler WHERE Numara = @pNumara AND Sifre = @pSifre";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pNumara", OgrenciNumarasi);
            komut.Parameters.AddWithValue("@pSifre", Sifre);
            object Id = komut.ExecuteScalar();
            if(Id != null) 
            { 
                int donusmusId = Convert.ToInt32(Id);
                baglanti.Close();
                return donusmusId;
            }
            else 
            { 
                baglanti.Close();
                return 0;
            }
        }

        public static int AkademisyenKayitKontrol(string AkademisyenNumarasi, string Sifre)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "SELECT OgretmenID FROM Ogretmenler WHERE OgretmenNumarasi = @pNumara AND OgretmenSifresi = @pSifre";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pNumara", AkademisyenNumarasi);
            komut.Parameters.AddWithValue("@pSifre", Sifre);
            object Id = komut.ExecuteScalar();
            if (Id != null)
            {
                int donusmusId = Convert.ToInt32(Id);
                baglanti.Close();
                return donusmusId;
            }
            else
            {
                baglanti.Close();
                return 0;
            }
        }
        public static int SifreSifirlama(int OgrenciID, string YeniSifre)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "UPDATE Ogrenciler SET sifre=@sifre WHERE OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", YeniSifre);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            komut.ExecuteNonQuery();
            baglanti.Close();
            return 0;
        }
        public static int SifreSifirlamaAkademisyen(int AkademisyenID, string YeniSifre)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "UPDATE Ogretmenler SET Ogretmensifresi=@sifre WHERE OgretmenID=@OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", YeniSifre);
            komut.Parameters.AddWithValue("@OgretmenID", AkademisyenID);
            komut.ExecuteNonQuery();
            baglanti.Close();
            return 0;
        }

        public static int VarsayilanSifre(int OgrenciID)
        {
            SqlConnection baglanti = BaglantiAcma();

            string yeniSifre = "123456";
            string sorgu = "UPDATE Ogrenciler SET sifre=@sifre WHERE OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", yeniSifre);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            komut.ExecuteNonQuery();
            baglanti.Close();
            return 0;
        }

        public static int VarsayilanSifreAkademisyen(int AkademisyenID)
        {
            SqlConnection baglanti = BaglantiAcma();

            string yeniSifre = "123456";
            string sorgu = "UPDATE Ogretmenler SET OgretmenSifresi=@sifre WHERE OgretmenID=@OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", yeniSifre);
            komut.Parameters.AddWithValue("@OgretmenID", AkademisyenID);
            komut.ExecuteNonQuery();
            baglanti.Close();
            return 0;
        }

        public static int EpostaNumaraDogrulama(string OgrenciNumarasi, string Email)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "SELECT OgrenciID FROM Ogrenciler WHERE Numara = @pNumara AND Eposta = @pEposta";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pNumara", OgrenciNumarasi);
            komut.Parameters.AddWithValue("@pEposta", Email);
            object Id = komut.ExecuteScalar();
            if (Id != null)
            {
                int donusmusId = Convert.ToInt32(Id);
                baglanti.Close();
                return donusmusId;
            }
            else
            {
                baglanti.Close();
                return 0;
            }
        }

        public static int EpostaNumaraDogrulamaAkademisyen(string AkademisyenNumarasi, string Email)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "SELECT OgretmenID FROM Ogretmenler WHERE OgretmenNumarasi = @pNumara AND OgretmenEpostasi = @pEposta";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pNumara", AkademisyenNumarasi);
            komut.Parameters.AddWithValue("@pEposta", Email);
            object Id = komut.ExecuteScalar();

            if (Id != null)
            {
                int donusmusId = Convert.ToInt32(Id);
                baglanti.Close();
                return donusmusId;
            }
            else
            {
                baglanti.Close();
                return 0;
            }
        }

        public static List<Ogrenci> OgrenciGetir()
        {
            List<Ogrenci> liste = new List<Ogrenci>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT OgrenciID, Ad, Soyad, Eposta, Numara FROM Ogrenciler";
            SqlCommand komut = new SqlCommand(sorgu,baglanti);
            SqlDataReader veriler=komut.ExecuteReader();

            while (veriler.Read())
            {
                Ogrenci ogr=new Ogrenci(); 
                ogr.Isim = veriler["Ad"].ToString();
                ogr.Soyisim = veriler["Soyad"].ToString();        
                ogr.OgrenciNumarasi = veriler["Numara"].ToString();
                ogr.Email = veriler["Eposta"].ToString();
                ogr.OgrenciID= Convert.ToInt32(veriler["OgrenciID"]);
                liste.Add(ogr);
            }
            baglanti.Close() ;
            return liste;
        }

        public static List<Akademisyen> AkademisyenGetir()
        {
            List<Akademisyen> liste = new List<Akademisyen>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT OgretmenID, OgretmenAdi, OgretmenSoyadi, OgretmenEpostasi, OgretmenNumarasi FROM Ogretmenler";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            SqlDataReader veriler = komut.ExecuteReader();
            while (veriler.Read())
            {
                Akademisyen akdmsyn = new Akademisyen();
                akdmsyn.AkademisyenIsim = veriler["OgretmenAdi"].ToString();
                akdmsyn.AkademisyenSoyisim = veriler["OgretmenSoyadi"].ToString();
                akdmsyn.AkademisyenNumarasi = veriler["OgretmenNumarasi"].ToString();
                akdmsyn.AkademisyenEmail = veriler["OgretmenEpostasi"].ToString();
                akdmsyn.AkademisyenID = Convert.ToInt32(veriler["OgretmenID"]);
                liste.Add(akdmsyn);
            }
            baglanti.Close();
            return liste;     
        }

        public static int OgrenciSilme(int OgrenciID)
        {
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "DELETE FROM Ogrenciler WHERE OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti); 
            komut.Parameters.AddWithValue("@OgrenciId",  OgrenciID);
            komut.ExecuteNonQuery();
            baglanti.Close();

            return 0;
        }
    }
}

