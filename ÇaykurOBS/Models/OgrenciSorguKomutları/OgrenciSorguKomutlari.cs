using Microsoft.Data.SqlClient;
namespace ÇaykurOBS.Models
{
    public class OgrenciSorguKomutlari
    {
        public static SqlConnection BaglantiAcma()
        {
            string baglantiAdresi = "Server=localhost\\SQLEXPRESS;Database=CaykurOBSDB;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
            return baglanti;
        }
        public static int OgrenciKayitKontrol(string OgrenciNumarasi, string Sifre)
        {
            SqlConnection baglanti = BaglantiAcma();

            string sorgu = "SELECT OgrenciID FROM Ogrenciler WHERE Numara = @pNumara AND Sifre = @pSifre AND Durum = 'Aktif Öğrenci'";
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
        public static List<Ogrenci> OgrenciGetir()
        {
            List<Ogrenci> liste = new List<Ogrenci>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT OgrenciID, Ad, Soyad, Eposta, Numara FROM Ogrenciler";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                Ogrenci ogr = new Ogrenci();
                ogr.Isim = veriler["Ad"].ToString();
                ogr.Soyisim = veriler["Soyad"].ToString();
                ogr.OgrenciNumarasi = veriler["Numara"].ToString();
                ogr.Email = veriler["Eposta"].ToString();
                ogr.OgrenciID = Convert.ToInt32(veriler["OgrenciID"]);
                liste.Add(ogr);
            }
            baglanti.Close();
            return liste;
        }
        public static List<DersSecimleri> OgrenciDersGetir(int OgrenciID)
        {
            List<DersSecimleri> liste = new List<DersSecimleri>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT DersSecimleri.DersID, DersSecimleri.Durum, Dersler.DersAdi,Dersler.Kredisi,Dersler.DersKodu FROM DersSecimleri " +
                           "JOIN Dersler ON DersSecimleri.DersID = Dersler.DersID " +
                           "WHERE OgrenciID=@OgrenciID ";
                            
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                DersSecimleri ogr = new DersSecimleri();
                ogr.DersID = Convert.ToInt32(veriler["DersID"]);
                ogr.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                ogr.DersAdi = veriler["DersAdi"].ToString();
                ogr.DersKodu = veriler["DersKodu"].ToString();
                ogr.Durum = veriler["Durum"].ToString();

                liste.Add(ogr);
            }
            baglanti.Close();
            return liste;
        }

        public static List<DersveNotlar> OgrenciNotGetir(int OgrenciID)
        {
            List<DersveNotlar> liste = new List<DersveNotlar>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT Notlar.DersID, Notlar.OgrenciID, Notlar.VizeNotu, Notlar.FinalNotu, Notlar.ButunlemeNotu, Notlar.Ortalama, Notlar.HarfNotu, " +
                           "Dersler.DersAdi,Dersler.Kredisi,Dersler.DersKodu, Ogretmenler.OgretmenAdi, Ogretmenler.OgretmenSoyadi FROM Notlar " +
                           "JOIN Dersler ON Notlar.DersID=Dersler.DersID " +
                           "JOIN Ogretmenler ON Dersler.OgretmenID=Ogretmenler.OgretmenID " +
                           "WHERE Notlar.OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                DersveNotlar notogr = new DersveNotlar();
                notogr.DersAdi = veriler["DersAdi"].ToString();
                notogr.DersKodu = veriler["DersKodu"].ToString();
                notogr.OgrenciAdiSoyadi = veriler["OgretmenAdi"].ToString() + " " + veriler["OgretmenSoyadi"].ToString();
                notogr.HarfNotu = veriler["HarfNotu"].ToString();
                notogr.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                notogr.DersID = Convert.ToInt32(veriler["DersID"]);
                notogr.OgrenciID = Convert.ToInt32(veriler["OgrenciID"]);
                notogr.VizeNotu = Convert.ToInt32(veriler["VizeNotu"]);
                notogr.FinalNotu = Convert.ToInt32(veriler["FinalNotu"]);
                notogr.ButunlemeNotu = veriler["ButunlemeNotu"] == DBNull.Value ? 0 : Convert.ToInt32(veriler["ButunlemeNotu"]);
                notogr.Ortalama = Convert.ToInt32(veriler["Ortalama"]);
                liste.Add(notogr);
            }
            baglanti.Close();
            return liste;
        }
        public static List<Ders> DersleriGetir()
        {
            List<Ders> liste = new List<Ders>();
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT Dersler.OgretmenID, Dersler.DersID, Dersler.DersAdi, Dersler.Kredisi, Dersler.DersKodu, Ogretmenler.OgretmenAdi, Ogretmenler.OgretmenSoyadi FROM Dersler "+
                            "JOIN Ogretmenler ON Dersler.OgretmenID=Ogretmenler.OgretmenID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            SqlDataReader veriler = komut.ExecuteReader();

            while (veriler.Read())
            {
                Ders drs = new Ders();
                drs.DersAdi = veriler["DersAdi"].ToString();
                drs.OgretmenAdiSoyadi = veriler["OgretmenAdi"].ToString() + " " + veriler["OgretmenSoyadi"].ToString();
                drs.DersKodu = veriler["DersKodu"].ToString();
                drs.OgretmenID = Convert.ToInt32(veriler["OgretmenID"]);
                drs.DersID = Convert.ToInt32(veriler["DersID"]);
                drs.DersKredisi = Convert.ToInt32(veriler["Kredisi"]);
                liste.Add(drs);
            }
            baglanti.Close();
            return liste;
        }
        public static void TopluDersSecimiKaydet(int OgrenciID, List<int> secilenDersler)
        {
            SqlConnection baglanti = BaglantiAcma();
            foreach (int DersID in secilenDersler)
            {
                string sorgu = "SELECT COUNT(*) FROM DersSecimleri WHERE OgrenciID=@OgrenciID AND DersID=@DersID";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
                komut.Parameters.AddWithValue("@DersID", DersID);
                int varMi = Convert.ToInt32(komut.ExecuteScalar());

                if (varMi == 0)
                {
                    string sorgu2 = "INSERT INTO DersSecimleri (OgrenciID, DersID) VALUES (@OgrenciID, @DersID)";
                    SqlCommand komut2 = new SqlCommand(sorgu2, baglanti);
                    komut2.Parameters.AddWithValue("@OgrenciID", OgrenciID);
                    komut2.Parameters.AddWithValue("@DersID", DersID);
                    komut2.ExecuteNonQuery();
                }
            }
            baglanti.Close();
        }
        public static int OgrenciMevcutKrediGetir(int OgrenciID)
        {
            SqlConnection baglanti = BaglantiAcma();
            string sorgu = "SELECT SUM(Dersler.Kredisi) AS ToplamKredi FROM DersSecimleri " +
                           "JOIN Dersler ON DersSecimleri.DersID = Dersler.DersID " +
                           "WHERE DersSecimleri.OgrenciID = @OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            object sonuc = komut.ExecuteScalar();
            baglanti.Close();
            if (sonuc != DBNull.Value && sonuc != null)
            {
                return Convert.ToInt32(sonuc);
            }
            else
            {
                return 0;
            }
        }
    }
}

