using Microsoft.Data.SqlClient;
namespace ÇaykurOBS.Models
{
    public class SorguKomutlari
    {
        private readonly string baglantiAdresi = "Server=localhost\\SQLEXPRESS;Database=CaykurOBSDB;Trusted_Connection=True;TrustServerCertificate=True;";
       
        public int OgrenciKayitEkle(Ogrenci YeniOgrenci)
        {
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();

            string kontrolSorgusu = "SELECT COUNT(*) FROM Ogrenciler WHERE Numara = @pNumara OR Eposta = @pEposta";
            SqlCommand kontrol = new SqlCommand(kontrolSorgusu, baglanti);

            kontrol.Parameters.AddWithValue("@pNumara", YeniOgrenci.OgrenciNumarasi);
            kontrol.Parameters.AddWithValue("@pEposta", YeniOgrenci.Email);

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
                kayitkomutu.Parameters.AddWithValue("@Eposta", YeniOgrenci.Email);
                kayitkomutu.Parameters.AddWithValue("@Sifre", YeniOgrenci.Sifre);
                kayitkomutu.ExecuteNonQuery();
                baglanti.Close();
                return 1;
            }
        }

        public int OgrenciKayitKontrol(string OgrenciNumarasi, string Sifre)
        {
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
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
        public int SifreSifirlama(int OgrenciID, string YeniSifre)
        {
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
            string sorgu = "UPDATE Ogrenciler SET sifre=@sifre WHERE OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", YeniSifre);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            komut.ExecuteScalar();
            baglanti.Close();
            return 0;
        }

        public int VarsayılanSifre(int OgrenciID)
        {
            SqlConnection baglanti = new SqlConnection(baglantiAdresi);
            baglanti.Open();
            string yeniSifre = "123456";
            string sorgu = "UPDATE Ogrenciler SET sifre=@sifre WHERE OgrenciID=@OgrenciID";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@sifre", yeniSifre);
            komut.Parameters.AddWithValue("@OgrenciID", OgrenciID);
            komut.ExecuteScalar();
            baglanti.Close();
            return 0;
        }
    }
}

