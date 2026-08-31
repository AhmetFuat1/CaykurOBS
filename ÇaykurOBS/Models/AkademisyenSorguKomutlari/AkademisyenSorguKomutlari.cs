using Microsoft.Data.SqlClient;
using System;
namespace ÇaykurOBS.Models
{
    public class AkademisyenSorguKomutlari
    {
        public static SqlConnection BaglantiAcma()
        {
            string baglantiAdresi = "Server=localhost\\SQLEXPRESS;Database=CaykurOBSDB;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
            return baglanti;
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
        public static int NotGirme(NotGirme YeniNot)
        {
            SqlConnection baglanti = BaglantiAcma();
            string kontrolSorgusu = "SELECT COUNT(*) FROM Notlar WHERE DersID = @pDersID AND OgrenciID = @pOgrenciID";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);
            kontrol.Parameters.AddWithValue("@pDersID", YeniNot.DersID);
            kontrol.Parameters.AddWithValue("@pOgrenciID", YeniNot.OgrenciID);
            int kayitSayisi = (int)kontrol.ExecuteScalar();
            if (kayitSayisi > 0)
            {
                baglanti.Close();
                return 0;
            }
            else
            {
                string kayit = "INSERT INTO Notlar (DersID, OgrenciID, VizeNotu, FinalNotu, ButunlemeNotu) VALUES (@DersID, @OgrenciID, @VizeNotu, @FinalNotu, @ButunlemeNotu)";
                SqlCommand kayitkomutu = new SqlCommand(kayit, baglanti);
                kayitkomutu.Parameters.AddWithValue("@DersID", YeniNot.DersID);
                kayitkomutu.Parameters.AddWithValue("@OgrenciID", YeniNot.OgrenciID);
                kayitkomutu.Parameters.AddWithValue("@VizeNotu", YeniNot.VizeNotu);
                kayitkomutu.Parameters.AddWithValue("@FinalNotu", YeniNot.FinalNotu);
                kayitkomutu.Parameters.AddWithValue("@ButunlemeNotu", (object)YeniNot.ButunlemeNotu ?? DBNull.Value);
                kayitkomutu.ExecuteNonQuery();
                baglanti.Close();
                return 1;
            }
        }
        public static int NotDuzenle(DersveNotlar YeniNot)
        {
            SqlConnection baglanti = BaglantiAcma();

                string sorgu = "UPDATE Notlar SET VizeNotu=@VizeNotu, FinalNotu=@FinalNotu, ButunlemeNotu=@ButunlemeNotu WHERE DersID=@DersID AND OgrenciID=@OgrenciID";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@DersID", YeniNot.DersID);
                komut.Parameters.AddWithValue("@OgrenciID", YeniNot.OgrenciID);
                komut.Parameters.AddWithValue("@VizeNotu", YeniNot.VizeNotu);
                komut.Parameters.AddWithValue("@FinalNotu", YeniNot.FinalNotu);    
                komut.Parameters.AddWithValue("@ButunlemeNotu", (object)YeniNot.ButunlemeNotu ?? DBNull.Value);
                int etkilenen = komut.ExecuteNonQuery(); 
                baglanti.Close();
                return etkilenen;
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
        public static Akademisyen AkademisyenBilgiGetir(int akademisyenId)
        {
            Akademisyen hoca = null;
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT OgretmenID, OgretmenAdi, OgretmenSoyadi, OgretmenEpostasi, OgretmenNumarasi FROM Ogretmenler WHERE OgretmenID = @pId";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pId", akademisyenId);
            SqlDataReader veriler = komut.ExecuteReader();

            if (veriler.Read())
            {
                hoca = new Akademisyen();
                hoca.AkademisyenID = Convert.ToInt32(veriler["OgretmenID"]);
                hoca.AkademisyenIsim = veriler["OgretmenAdi"].ToString();
                hoca.AkademisyenSoyisim = veriler["OgretmenSoyadi"].ToString();
                hoca.AkademisyenNumarasi = veriler["OgretmenNumarasi"].ToString();
                hoca.AkademisyenEmail = veriler["OgretmenEpostasi"].ToString();
            }
            baglanti.Close();
            return hoca;
        }
        public static List<DersSecimleri> DersiAlanOgrencileriGetir(int DersID)
        {
            List<DersSecimleri> liste = new List<DersSecimleri>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT DersSecimleri.DersID, DersSecimleri.OgrenciID, DersSecimleri.Durum, Ogrenciler.Ad, Ogrenciler.Soyad, Dersler.Dersadi, Dersler.Kredisi, Dersler.DersKodu  FROM DersSecimleri " +
                            "JOIN Dersler ON DersSecimleri.DersID=Dersler.DersID " +
                            "JOIN Ogrenciler ON DersSecimleri.OgrenciID=Ogrenciler.OgrenciID " +
                            "WHERE DersSecimleri.DersID = @pDersID AND DersSecimleri.Durum = 'Onaylandı' ";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pDersID", DersID);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                DersSecimleri drsogr = new DersSecimleri();
                drsogr.DersAdi = veriler["DersAdi"].ToString();
                drsogr.DersKodu = veriler["DersKodu"].ToString();
                drsogr.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                drsogr.DersID = Convert.ToInt32(veriler["DersID"]);
                drsogr.OgrenciID = Convert.ToInt32(veriler["OgrenciID"]);
                drsogr.OgrenciAdiSoyadi = veriler["Ad"].ToString() + " " + veriler["Soyad"].ToString();
                liste.Add(drsogr);
            }
            baglanti.Close();
            return liste;
        }
        public static List<DersveNotlar> DersiAlanOgrencileriveNotlariGetir(int DersID)
        {
            List<DersveNotlar> liste = new List<DersveNotlar>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT DersSecimleri.DersID, DersSecimleri.OgrenciID, DersSecimleri.Durum, Ogrenciler.Ad, Ogrenciler.Soyad, Dersler.Dersadi, Dersler.Kredisi, Dersler.DersKodu, Notlar.VizeNotu, Notlar.FinalNotu, Notlar.ButunlemeNotu, Notlar.Ortalama, Notlar.HarfNotu FROM DersSecimleri " +
                            "JOIN Dersler ON DersSecimleri.DersID=Dersler.DersID " +
                            "JOIN Ogrenciler ON DersSecimleri.OgrenciID=Ogrenciler.OgrenciID " +
                            "JOIN Notlar ON DersSecimleri.DersID=Notlar.DersID AND DersSecimleri.OgrenciID=Notlar.OgrenciID  " +
                            "WHERE DersSecimleri.DersID = @pDersID AND DersSecimleri.Durum = 'Onaylandı' ";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@pDersID", DersID);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                DersveNotlar notogr = new DersveNotlar();
                notogr.DersAdi = veriler["DersAdi"].ToString();
                notogr.DersKodu = veriler["DersKodu"].ToString();
                notogr.OgrenciAdiSoyadi = veriler["Ad"].ToString() + " " + veriler["Soyad"].ToString();
                notogr.DersKodu = veriler["DersKodu"].ToString();
                notogr.HarfNotu = veriler["HarfNotu"].ToString();
                notogr.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                notogr.DersID = Convert.ToInt32(veriler["DersID"]);
                notogr.OgrenciID = Convert.ToInt32(veriler["OgrenciID"]);
                notogr.VizeNotu = Convert.ToInt32(veriler["VizeNotu"]);
                notogr.FinalNotu = Convert.ToInt32(veriler["FinalNotu"]);
                notogr.ButunlemeNotu = veriler["ButunlemeNotu"] == DBNull.Value ? null : Convert.ToInt32(veriler["ButunlemeNotu"]);
                notogr.Ortalama = Convert.ToInt32(veriler["Ortalama"]);    
                liste.Add(notogr);
            }
            baglanti.Close();
            return liste;
        }
        public static List<Ders> AkademisyenDersGetir(int AkademisyenID)
        {
            List<Ders> liste = new List<Ders>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT DersID, DersAdi,Kredisi,DersKodu FROM Dersler WHERE OgretmenID=@OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@OgretmenID", AkademisyenID);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                Ders akdmsyndrs = new Ders();
                akdmsyndrs.DersID = Convert.ToInt32(veriler["DersID"]);
                akdmsyndrs.DersAdi = veriler["DersAdi"].ToString();
                akdmsyndrs.DersKodu = veriler["DersKodu"].ToString();
                akdmsyndrs.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                liste.Add(akdmsyndrs);
            }
            baglanti.Close();
            return liste;
        }
    }
}

