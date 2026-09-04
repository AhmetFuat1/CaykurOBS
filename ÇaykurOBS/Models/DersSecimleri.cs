namespace ÇaykurOBS.Models
{       public class DersSecimleri
    {
        public int SecimID { get; set; }
        public int DersID { get; set; }
        public int OgrenciID { get; set; }
        public int DersKredisi { get; set; }
        public string Durum { get; set; }
        public string DersAdi { get; set; }
        public string DersKodu { get; set; }
        public string OgrenciAdiSoyadi { get; set; }
        public string OgretmenAdiSoyadi { get; set; }
    }
}
