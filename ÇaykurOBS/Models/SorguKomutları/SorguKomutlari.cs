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
        public static int DersEkle(Ders YeniDers)
        {
            SqlConnection baglanti = BaglantiAcma();
            string DersKodu = YeniDers.DersKodu;
            string kontrolSorgusu = "SELECT COUNT(*) FROM Dersler WHERE DersKodu = @pDersKodu";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);
            kontrol.Parameters.AddWithValue("@pDersKodu", DersKodu);
            int kayitSayisi = (int)kontrol.ExecuteScalar();
            if (kayitSayisi > 0)
            {
                baglanti.Close();
                return 0;
            }
            else
            {
                string kayit = "INSERT INTO Dersler (OgretmenID, Kredisi, DersAdi, DersKodu) VALUES (@OgretmenID, @Kredisi, @DersAdi, @DersKodu)";
                SqlCommand kayitkomutu = new SqlCommand(kayit, baglanti);
                kayitkomutu.Parameters.AddWithValue("@OgretmenID", YeniDers.OgretmenID);
                kayitkomutu.Parameters.AddWithValue("@Kredisi", YeniDers.DersKredisi);
                kayitkomutu.Parameters.AddWithValue("@DersAdi", YeniDers.DersAdi);
                kayitkomutu.Parameters.AddWithValue("@DersKodu", YeniDers.DersKodu);
                kayitkomutu.ExecuteNonQuery();
                baglanti.Close();
                return 1;
            }
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
        public static int DersDuzenle(int DersID, Ders YeniDers)
        {
            SqlConnection baglanti = BaglantiAcma();
            string DersKodu = YeniDers.DersKodu;
            string kontrolSorgusu = "SELECT COUNT(*) FROM Dersler WHERE DersKodu = @pDersKodu AND DersID != @pDersID";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);
            kontrol.Parameters.AddWithValue("@pDersKodu", DersKodu);
            kontrol.Parameters.AddWithValue("@pDersID", DersID);
            int kayitSayisi = (int)kontrol.ExecuteScalar();
            if (kayitSayisi > 0)
            {
                baglanti.Close();
                return 0;
            }
            else
            {
                string sorgu = "UPDATE Dersler SET DersAdi=@DersAdi, Kredisi=@DersKredisi, OgretmenID=@OgretmenID, DersKodu=@DersKodu WHERE DersID=@DersID";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@DersID", DersID);
                komut.Parameters.AddWithValue("@DersAdi", YeniDers.DersAdi);
                komut.Parameters.AddWithValue("@DersKredisi", YeniDers.DersKredisi);
                komut.Parameters.AddWithValue("@OgretmenID", YeniDers.OgretmenID);
                komut.Parameters.AddWithValue("@DersKodu", YeniDers.DersKodu);
                komut.ExecuteNonQuery();
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
                komut.Parameters.AddWithValue("@ButunlemeNotu", YeniNot.ButunlemeNotu );
                int etkilenen = komut.ExecuteNonQuery(); 
                baglanti.Close();
                return etkilenen;
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
        public static List<Ders> DersGetir()
        {
            List<Ders> liste = new List<Ders>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT Dersler.DersID, Dersler.DersAdi, Dersler.Kredisi, Dersler.DersKodu , Ogretmenler.OgretmenAdi, Ogretmenler.OgretmenSoyadi FROM Dersler " +
                            "JOIN Ogretmenler ON Dersler.OgretmenID=Ogretmenler.OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                Ders drs = new Ders();
                drs.DersAdi = veriler["DersAdi"].ToString();
                drs.DersKodu = veriler["DersKodu"].ToString();
                drs.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);  
                drs.DersID = Convert.ToInt32(veriler["DersID"]);
                drs.OgretmenAdiSoyadi = veriler["OgretmenAdi"].ToString() + " " + veriler["OgretmenSoyadi"].ToString();
                liste.Add(drs);
            }
            baglanti.Close();
            return liste;
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
                notogr.ButunlemeNotu = Convert.ToInt32(veriler["ButunlemeNotu"]== DBNull.Value ? (int?)null:Convert.ToInt32(veriler["ButunlemeNotu"]));
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
        public static int DersSilme(int DersID)
        {
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "DELETE FROM Dersler WHERE DersID=@DersID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@DersID", DersID);
            komut.ExecuteNonQuery();
            baglanti.Close();

            return 0;
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
        public static int AkademisyenSilme(int AkademisyenID)
        {
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "DELETE FROM Ogretmenler WHERE OgretmenID=@OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@OgretmenId", AkademisyenID);
            komut.ExecuteNonQuery();
            baglanti.Close();

            return 0;
        }
    }
}

