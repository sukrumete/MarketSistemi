using Microsoft.Data.SqlClient;

string baglanti = "Server=localhost;Database=MarketDb;Trusted_Connection=True;TrustServerCertificate=True;";
SqlConnection hat = new SqlConnection(baglanti);
hat.Open();
bool devam = true;
while (devam)
{
    Console.WriteLine("1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Çıkış");
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
        Console.WriteLine("Lütfen Müşteri Id'sini Giriniz");
        string musteriId = Console.ReadLine();
        Console.WriteLine("Lütfen Fiyatını Giriniz");
        string fiyat = Console.ReadLine();
        string sorgu = "Insert into Siparisler (MusteriId, Fiyat ,Tarih) values (@musteriId, @fiyat, GETDATE())";
        SqlCommand komut = new SqlCommand(sorgu, hat);
        komut.Parameters.AddWithValue("@musteriId", musteriId);
        komut.Parameters.AddWithValue("@fiyat", fiyat);
        komut.ExecuteNonQuery();

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
        Console.WriteLine("Çıkış yapılıyor.");
        devam = false;
    }
}
hat.Close();