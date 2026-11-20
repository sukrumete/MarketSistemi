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
        string sorgu = "SELECT * FROM Urunler";
        SqlCommand komut = new SqlCommand(sorgu, hat);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"]);
        }
        oku.Close();
    }
    if (secim == "2")
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
                    SqlCommand komut = new SqlCommand(sorgu, hat);
                    komut.Parameters.AddWithValue("@musteriId", musteriId);
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

    if (secim == "3")
    {
        string sorgu = "SELECT Isim, Soyad, Fiyat FROM Siparisler INNER JOIN Musteriler ON Musteriler.Id = Siparisler.MusteriId";
        SqlCommand komut = new SqlCommand(sorgu, hat);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"]);
        }
        oku.Close();
    }
    if (secim == "4")
    {
        string sorgu = "SELECT Isim, Fiyat, Adet From SiparisKalemleri Inner JOIN Urunler ON Urunler.Id = SiparisKalemleri.UrunId";
        SqlCommand komut = new SqlCommand(sorgu, hat);
        SqlDataReader oku = komut.ExecuteReader();
        while (oku.Read())
        {
            Console.WriteLine(oku["Isim"] + " - " + oku["Fiyat"] + " - " + oku["Adet"]);
        }
        oku.Close();
    }
    if (secim == "5")
    {
        try
        {
            Console.WriteLine("Lütfen Sipariş Id'sini Giriniz");
            string siparisId = Console.ReadLine();
            Console.WriteLine("Lütfen Ürün Id'sini Giriniz");
            string urunId = Console.ReadLine();
            Console.WriteLine("Lütfen Ürün Adetini Giriniz");
            string adet = Console.ReadLine();
            bool adetGecerli = int.TryParse(adet, out int adetSayisi);
            if (adetGecerli && 0 < adetSayisi)
            {
                string sorgu = "Insert into SiparisKalemleri (SiparisId, UrunId, Adet) values (@siparisId, @urunId, @adet)";
                SqlCommand komut = new SqlCommand(sorgu, hat);
                komut.Parameters.AddWithValue("@siparisId", siparisId);
                komut.Parameters.AddWithValue("@urunId", urunId);
                komut.Parameters.AddWithValue("@adet", adet);
                komut.ExecuteNonQuery();
                string guncelleSorgu = "UPDATE Siparisler SET Fiyat = (SELECT SUM(Fiyat * Adet) FROM SiparisKalemleri INNER JOIN Urunler ON Urunler.Id = SiparisKalemleri.UrunId WHERE SiparisId = @siparisId) WHERE Id = @siparisId";
                SqlCommand guncelleKomut = new SqlCommand(guncelleSorgu, hat);
                guncelleKomut.Parameters.AddWithValue("@siparisId", siparisId);
                guncelleKomut.ExecuteNonQuery();
                Console.WriteLine("Sipariş kalemi başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Adet Sıfırdan Büyük Olmalıdır. Lütfen tekrar deneyin.");
            }

        }
        catch
        {
            Console.WriteLine("Geçersiz bir değer girdiniz. Lütfen tekrar deneyin.");
        }
    }
    if (secim == "6")
    {
        Console.WriteLine("Çıkış yapılıyor.");
        devam = false;
    }
}
hat.Close();