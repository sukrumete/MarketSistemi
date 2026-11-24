using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;

string baglanti = "Server=localhost;Database=MarketDb;Trusted_Connection=True;TrustServerCertificate=True;";
SqlConnection hat = new SqlConnection(baglanti);
try
{
    hat.Open();
}
catch
{
    Console.WriteLine("Veritabanına bağlanılamadı. Lütfen bağlantı ayarlarını kontrol edin.");
    return;
}
bool devam = true;
while (devam)
{
    Console.WriteLine("1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Sipariş Kalemi Ekle 6-)Çıkış");
    string secim = Console.ReadLine();

    if (secim == "1")
    {
        UrunleriListele(hat);
    }

    if (secim == "2")
    {
        SiparisEkle(hat);
    }

    if (secim == "3")
    {
        SiparisleriListele(hat);
    }

    if (secim == "4")
    {
        SiparisKalemleriniListele(hat);
    }

    if (secim == "5")
    {
        SiparisKalemiEkle(hat);
    }

    if (secim == "6")
    {
        Console.WriteLine("Çıkış yapılıyor.");
        devam = false;
    }
}
hat.Close();



static void UrunleriListele(SqlConnection baglanti)

{
    try
    {
        string sorgu = "SELECT * FROM Urunler";
        SqlCommand komut = new SqlCommand(sorgu, baglanti);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"]);
        }
        oku.Close();
    }
    catch (SqlException hata)
    {
        Console.WriteLine("Ürün Listelenemedi");
    }
}

static void SiparisleriListele(SqlConnection baglanti)
{
    try
    {
        string sorgu = "SELECT Isim, Soyad, Fiyat FROM Siparisler INNER JOIN Musteriler ON Musteriler.Id = Siparisler.MusteriId";
        SqlCommand komut = new SqlCommand(sorgu, baglanti);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"]);
        }
        oku.Close();
    }
    catch (SqlException hata)
    {
        Console.WriteLine("Sipariş Listelenemedi");
    }
}


static void SiparisKalemleriniListele(SqlConnection baglanti)
{
    try
    {
        string sorgu = "SELECT Isim, Fiyat, Adet From SiparisKalemleri Inner JOIN Urunler ON Urunler.Id = SiparisKalemleri.UrunId";
        SqlCommand komut = new SqlCommand(sorgu, baglanti);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"] + " - " + oku["Adet"]);
        }
        oku.Close();
    }
    catch (SqlException hata)
    {
        Console.WriteLine("Sipariş Kalemleri Listelenemedi");
    }
}

static void SiparisEkle(SqlConnection baglanti)
{

    try
    {
        Console.WriteLine("Lütfen Müşteri Id'sini Giriniz");
        string musteriId = Console.ReadLine();
        bool musteriIdGecerli = int.TryParse(musteriId, out int musteriIdSayisi);
        if (musteriIdGecerli && 0 < musteriIdSayisi)
        {
            Console.WriteLine("Lütfen Fiyatını Giriniz");
            string fiyat = Console.ReadLine();
            fiyat = fiyat.Replace('.', ',');
            bool fiyatGecerli = decimal.TryParse(fiyat, out decimal fiyatSayisi);
            if (fiyatGecerli && 0 < fiyatSayisi)
            {
                string sorgu = "Insert into Siparisler (MusteriId, Fiyat ,Tarih) values (@musteriId, @fiyat, GETDATE())";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@musteriId", musteriIdSayisi);
                komut.Parameters.AddWithValue("@fiyat", fiyatSayisi);
                komut.ExecuteNonQuery();
                Console.WriteLine("Sipariş başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Fiyat Sıfırdan Büyük Olmalıdır. Lütfen tekrar deneyin.");
            }
        }
        else
        {
            Console.WriteLine("Müşteri Id Sıfırdan Büyük Olmalıdır. Lütfen tekrar deneyin.");
        }

    }
    catch (SqlException hata)
    {
        Console.WriteLine("Bu Müşteri bulunamadı. Lütfen tekrar deneyin.");
    }
    catch (Exception hata)
    {
        Console.WriteLine("Geçersiz bir değer girdiniz. Lütfen tekrar deneyin.");
        Console.WriteLine(hata.Message);
    }
}

static void SiparisKalemiEkle(SqlConnection baglanti)
{
    try
    {
        Console.WriteLine("Lütfen Sipariş Id'sini Giriniz");
        string siparisId = Console.ReadLine();
        bool siparisIdGecerli = int.TryParse(siparisId, out int siparisIdSayisi);
        Console.WriteLine("Lütfen Ürün Id'sini Giriniz");
        string urunId = Console.ReadLine();
        bool urunIdGecerli = int.TryParse(urunId, out int urunIdSayisi);
        Console.WriteLine("Lütfen Ürün Adetini Giriniz");
        string adet = Console.ReadLine();
        bool adetGecerli = int.TryParse(adet, out int adetSayisi);
        if (adetGecerli && 0 < adetSayisi && siparisIdGecerli && 0 < siparisIdSayisi && urunIdGecerli && 0 < urunIdSayisi)
        {
            string sorgu = "Insert into SiparisKalemleri (SiparisId, UrunId, Adet) values (@siparisId, @urunId, @adet)";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@siparisId", siparisIdSayisi);
            komut.Parameters.AddWithValue("@urunId", urunIdSayisi);
            komut.Parameters.AddWithValue("@adet", adetSayisi);
            komut.ExecuteNonQuery();
            string guncelleSorgu = "UPDATE Siparisler SET Fiyat = (SELECT SUM(Fiyat * Adet) FROM SiparisKalemleri INNER JOIN Urunler ON Urunler.Id = SiparisKalemleri.UrunId WHERE SiparisId = @siparisId) WHERE Id = @siparisId";
            SqlCommand guncelleKomut = new SqlCommand(guncelleSorgu, baglanti);
            guncelleKomut.Parameters.AddWithValue("@siparisId", siparisIdSayisi);
            guncelleKomut.ExecuteNonQuery();
            Console.WriteLine("Sipariş kalemi başarıyla eklendi.");
        }
        else
        {
            Console.WriteLine("Lütfen geçerli bir Sipariş Id, Ürün Id ve Adet giriniz. Hepsi sıfırdan büyük olmalıdır. Lütfen tekrar deneyin.");
        }

    }
    catch (SqlException hata)
    {
        Console.WriteLine("Bu Sipariş veya Ürün bulunamadı. Lütfen tekrar deneyin.");
    }
    catch (Exception)
    {
        Console.WriteLine("Geçersiz bir değer girdiniz. Lütfen tekrar deneyin.");
    }
}
