# Market Sistemi

SQL Server ve C# (konsol uygulaması) ile yazdığım küçük bir market sipariş sistemi. C# ve SQL öğrenme sürecimdeki projelerimden biri. SQL'in ikinci yarısını (JOIN, gruplama, ilişkili tablolar) öğrenmek ve bunu C# ile bağlamak için yaptım.

## Neler yapıyor?

Konsolda bir menü açılır, çıkış seçilene kadar tekrar eder:

1. Ürünleri listele
2. Sipariş ekle (müşteri Id'si ve fiyat girilir, tarihi SQL kendisi koyar)
3. Siparişleri listele (müşteri adıyla birlikte, JOIN ile)
4. Sipariş kalemlerini listele (ürün adı, fiyat ve adet, JOIN ile)
5. Çıkış

```
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Çıkış
3
Mete - 35,40
Ahmet - 50,40
Mete - 785,40
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Çıkış
4
Süt - 45,00 - 2
Ekmek - 15,00 - 1
```

## Veritabanı

Dört tablo var: Musteriler, Urunler, Siparisler, SiparisKalemleri.
Siparisler müşteriye, SiparisKalemleri ise hem siparişe hem ürüne FOREIGN KEY ile bağlı.

## Nasıl çalıştırılır?

1. SQL Server Management Studio'da `MarketKurulum.sql` dosyasını çalıştırın. Bu, MarketDb veritabanını, tabloları ve örnek verileri kurar. (MarketDb zaten varsa "already exists" hatası verir.)
2. `MarketSistemi.slnx`'i Visual Studio'da açıp çalıştırın (F5).
3. Bağlantı `localhost` üzerindeki SQL Server'a Windows kimlik doğrulamasıyla yapılır. Şifre yoktur. Sunucunuz farklıysa `Program.cs` içindeki bağlantı cümlesini değiştirin.

Gereksinimler: .NET 10, SQL Server, Microsoft.Data.SqlClient paketi.

## Bu projede kullandığım konular

- SQL: CREATE TABLE, PRIMARY KEY / FOREIGN KEY, INSERT, INNER JOIN (birden fazla tabloyla), COUNT, SUM, GROUP BY, HAVING, TOP, BETWEEN, LIKE, DISTINCT
- C#: SqlConnection, SqlCommand, SqlDataReader, ExecuteReader / ExecuteNonQuery, parametreli sorgu (@yer tutucu + AddWithValue), menü için while döngüsü ve if

En çok zorlandığım konular: SQL'de tablo adı ile kutu adını karıştırmak ve JOIN'deki ON eşleşmesi. Bunları tekrar ederek oturtmaya devam ediyorum.

## Yapay zekâ yardımı

Bu projede Claude (yapay zekâ) ile birlikte çalıştım. Dökümü şöyle:

**Yapay zekânın yaptığı:**
- Konuları benzetmelerle, adım adım anlattı ve projenin sürüm planını çıkardı.
- Kalıpları gösterdi: INSERT ... VALUES yapısı, @ yer tutucu ve AddWithValue, ExecuteReader ile ExecuteNonQuery farkı, JOIN'deki ON eşleşmesi.
- Dört tablolu yapının (SiparisKalemleri ara tablosu) fikrini gösterdi. Menü için "devam" değişkenli while döngüsünü ve tarih için GETDATE() kullanmayı önerdi.
- Hatalarımı bulmam için ipucu verdi (örneğin okuyucunun kapatılmaması, `=` ile `==` karışıklığı).
- Notlarımın, bu README'nin ve GELISIM.md'nin ilk taslağının yazılmasına yardım etti.

**Benim yaptığım:**
- `Program.cs` içindeki kodu, ipuçlarıyla, satır satır kendim yazdım.
- SQL sorgularını SSMS'te kendim yazıp denedim.
- `MarketKurulum.sql` içindeki tablo ve INSERT komutlarını kendim yazdım. Urunler'in hangi bilgileri tutacağını (isim, marka, fiyat) kendim düşündüm.
- Test için dosyayı sıfırdan, başka bir veritabanı adıyla çalıştırdım.

Yani kavramları ve kalıpları yapay zekâ gösterdi, komutları ve programı ben yazdım. Birçok yerde ipucu aldım, bunu saklamıyorum.

👉 Sürüm sürüm gelişim notu: [GELISIM.md](https://github.com/sukrumete/MarketSistemi/blob/master/GELISIM.md)

## Bilinen eksikler

- Girilen değerler kontrol edilmiyor: olmayan bir müşteri Id'si ya da harf yazılırsa program hata verip kapanır (try-catch yok).
- Fiyat ondalık için nokta ile girilmeli (24.50).
- Sipariş eklerken sadece Siparisler tablosuna yazılıyor, sipariş kalemleri C#'tan eklenmiyor. Sipariş fiyatı kalemlerden hesaplanmıyor, elle giriliyor.
- Silme ve güncelleme yok.

## Sonraki adımlar

- try-catch ile hata yönetimi
- Sipariş kalemi ekleme ve toplam fiyatı otomatik hesaplama
- Kodu metotlara ve sınıflara bölmek
