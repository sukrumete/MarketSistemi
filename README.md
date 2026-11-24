# Market Sistemi

SQL Server ve C# (konsol uygulaması) ile yazdığım küçük bir market sipariş sistemi. C# ve SQL öğrenme sürecimdeki projelerimden biri. SQL'in ikinci yarısını (JOIN, gruplama, ilişkili tablolar) öğrenmek ve bunu C# ile bağlamak için yaptım.

## Neler yapıyor?

Konsolda bir menü açılır, çıkış seçilene kadar tekrar eder:

1. Ürünleri listele
2. Sipariş ekle (müşteri Id'si ve fiyat girilir, tarihi SQL kendisi koyar). Müşteri Id'si sayı değilse ya da sıfırdan büyük değilse, fiyat sıfırdan büyük bir sayı değilse uyarı verir. Fiyat nokta ya da virgülle yazılabilir (24.50 veya 24,50). Olmayan bir müşteri Id'si girilirse ayrı bir uyarı verir.
3. Siparişleri listele (müşteri adıyla birlikte, JOIN ile)
4. Sipariş kalemlerini listele (ürün adı, fiyat ve adet, JOIN ile)
5. Sipariş kalemi ekle (sipariş Id'si, ürün Id'si ve adet girilir). Üçü de sıfırdan büyük bir sayı değilse uyarı verir. Kalem eklenince siparişin fiyatı, o siparişin kalemlerinin toplamından (fiyat × adet) otomatik hesaplanıp güncellenir.
6. Çıkış

Örnek çıktı (kurulum dosyasındaki örnek verilerle):

```
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Sipariş Kalemi Ekle 6-)Çıkış
5
Lütfen Sipariş Id'sini Giriniz
2
Lütfen Ürün Id'sini Giriniz
2
Lütfen Ürün Adetini Giriniz
1
Sipariş kalemi başarıyla eklendi.
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Sipariş Kalemi Ekle 6-)Çıkış
3
Mete - 35,40
Ahmet - 100,00
Mete - 785,40
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Sipariş Kalemi Ekle 6-)Çıkış
4
Süt - 45,00 - 2
Ekmek - 15,00 - 1
Peynir - 100,00 - 1
```

Ahmet'in 2 numaralı siparişi 50,40'tan 100,00'a döndü, çünkü fiyat artık kalemlerin toplamından geliyor (1 adet Peynir × 100).

Hatalı girişte örnek çıktı:

```
2
Lütfen Müşteri Id'sini Giriniz
abc
Müşteri Id Sıfırdan Büyük Olmalıdır. Lütfen tekrar deneyin.
1-)Ürünleri Listele 2-)Sipariş Ekle 3-)Siparişleri Listele 4-)Siparis Kalemleri 5-)Sipariş Kalemi Ekle 6-)Çıkış
2
Lütfen Müşteri Id'sini Giriniz
999
Lütfen Fiyatını Giriniz
300
Bu Müşteri bulunamadı. Lütfen tekrar deneyin.
```

## Veritabanı

Dört tablo var: Musteriler, Urunler, Siparisler, SiparisKalemleri. Siparisler müşteriye, SiparisKalemleri ise hem siparişe hem ürüne FOREIGN KEY ile bağlı. FOREIGN KEY sayesinde olmayan bir müşteri, sipariş ya da ürün numarasıyla kayıt eklenemez.

## Nasıl çalıştırılır?

1. SQL Server Management Studio'da `MarketKurulum.sql` dosyasını çalıştırın. Bu, MarketDb veritabanını, tabloları ve örnek verileri kurar. (MarketDb zaten varsa "already exists" hatası verir.)
2. `MarketSistemi.slnx`'i Visual Studio'da açıp çalıştırın (F5).

Bağlantı localhost üzerindeki SQL Server'a Windows kimlik doğrulamasıyla yapılır. Şifre yoktur. Sunucunuz farklıysa `Program.cs` içindeki bağlantı cümlesini değiştirin.

Gereksinimler: .NET 10, SQL Server, Microsoft.Data.SqlClient paketi.

## Bu projede kullandığım konular

- **SQL:** CREATE TABLE, PRIMARY KEY / FOREIGN KEY, INSERT, INNER JOIN (birden fazla tabloyla), WHERE, hesaplanan sütun (Fiyat * Adet), COUNT, SUM, GROUP BY, HAVING, TOP, BETWEEN, LIKE, DISTINCT, UPDATE (içinde SELECT ile, yani iç içe sorgu), DELETE (SSMS'te test verisini silmek için, önce aynı WHERE ile SELECT yapıp neyin silineceğine bakarak)
- **C#:** SqlConnection, SqlCommand, SqlDataReader, ExecuteReader / ExecuteNonQuery, parametreli sorgu (@yer tutucu + AddWithValue), menü için while döngüsü ve if, `int.TryParse` / `decimal.TryParse` ile girdi kontrolü, `&&` ile birden fazla şartı bağlama, iç içe if, `Replace` ile noktayı virgüle çevirme, try-catch (sipariş ekleme, sipariş kalemi ekleme ve bağlantı açma), `catch (SqlException)` ile veritabanı hatasını genel hatadan ayırma, `hata.Message` ile hatanın nedenini görme, `return` ile programı erken bitirme
- **En çok zorlandığım konular:** SQL'de tablo adı ile kutu adını karıştırmak, JOIN'deki ON eşleşmesi ve UPDATE'in içine SELECT koymak (iç içe sorgu). Hangi `WHERE`'de `Id`, hangisinde `SiparisId` yazılacağını karıştırdım. Girdi kontrolünde "geçerli mi" kutusu (bool) ile "sayı" kutusunu karıştırdım, `<` ve `>` yönünü ters yazdım. Bir DELETE'te koşulu yanlış yazmıştım (asıl siparişi silecekti), çalıştırmadan önce yapay zekâ fark ettirdi.
- **Öğrendiğim bir tuzak:** Türkçe bilgisayar `100.50` yazısını 10050 olarak okuyor (nokta binlik ayracı sayılıyor). Fiyat kontrolü "sıfırdan büyük mü" diye baktığı için bunu geçirdi, tabloya 100 TL yerine 10050 TL yazıldı ve hata mesajı çıkmadı. Testte kendim gördüm, nedenini yapay zekâ açıkladı. `TryParse`'tan önce noktayı virgüle çevirerek çözdüm ve SQL'e yazıyı değil çevrilmiş sayıyı gönderiyorum.

## Yapay zekâ yardımı

Bu projede Claude (yapay zekâ) ile birlikte çalıştım. Dökümü şöyle:

**Yapay zekânın yaptığı:**

- Konuları benzetmelerle, adım adım anlattı ve projenin sürüm planını çıkardı.
- Kalıpları gösterdi: INSERT ... VALUES yapısı, @ yer tutucu ve AddWithValue, ExecuteReader ile ExecuteNonQuery farkı, JOIN'deki ON eşleşmesi, try-catch yapısı.
- Dört tablolu yapının (SiparisKalemleri ara tablosu) fikrini gösterdi. Menü için "devam" değişkenli while döngüsünü ve tarih için GETDATE() kullanmayı önerdi.
- Bağlantı hatasında programı `return` ile bitirmenin gerektiğini gösterdi.
- SELECT içinde hesap yapmayı (`Fiyat * Adet`) ve UPDATE'in içine SELECT koymayı (iç içe sorgu) o gösterdi. Bunları yapay zekâ olmadan yazamazdım.
- Toplam fiyatı C#'a taşırken gerekli üç satırın (SqlCommand, AddWithValue, ExecuteNonQuery) kalıbını listeledi.
- Girdi kontrolünün kalıbını gösterdi: `TryParse` sonucunu bool kutusuna koyup `if` içinde `&&` ile kullanmak, iç içe if yapısı ve `catch (SqlException)` ile `catch (Exception)` ayrımı. Her adımda boşluklu ipuçları verdi.
- `100.50` → 10050 sorununun nedenini açıkladı, `Replace` ile çözme fikrini ve SQL'e yazı yerine çevrilmiş sayıyı göndermeyi gösterdi.
- Hatalarımı bulmam için ipucu verdi (örneğin okuyucunun kapatılmaması, = ile == karışıklığı, WHERE'in tırnağın dışında kalması, yanlış kutu adı, ters `<` işareti, yanlış DELETE koşulu).
- Notlarımın, bu README'nin ve GELISIM.md'nin ilk taslağının yazılmasına yardım etti.

**Benim yaptığım:**

- `Program.cs` içindeki kodu, ipuçlarıyla, satır satır kendim yazdım (sipariş kalemi ekleme, bağlantı try-catch'i, fiyat güncelleme, adet, fiyat ve Id kontrolleri ve ayrı catch blokları dahil).
- SQL sorgularını SSMS'te kendim yazıp denedim. Yapay zekânın gösterdiği iç içe sorgu kalıbına kendi SELECT sorgumu koydum, hataları düzelttim, çalıştığını fiyatı bilerek bozup geri hesaplatarak ispatladım ve C#'a taşıdım.
- Programı farklı girdilerle (harf, 0, eksi sayı, olmayan Id, noktalı ve virgüllü fiyat) kendim test ettim.
- Testlerden kalan hatalı siparişleri ve kalemleri DELETE ile kendim sildim, sipariş fiyatını UPDATE ile yeniden hesaplattım.
- `MarketKurulum.sql` içindeki tablo ve INSERT komutlarını kendim yazdım. Urunler'in hangi bilgileri tutacağını (isim, marka, fiyat) kendim düşündüm.
- Test için kurulum dosyasını sıfırdan, başka bir veritabanı adıyla çalıştırdım. Bağlantı korumasını da sunucu adını bilerek yanlış yazarak denedim.

Yani kavramları ve kalıpları yapay zekâ gösterdi, komutları ve programı ben yazdım. Birçok yerde ipucu aldım, bunu saklamıyorum.

👉 Sürüm sürüm gelişim notu: [GELISIM.md](GELISIM.md)

## Bilinen eksikler

- `catch (SqlException)` menü 2'de var ama her veritabanı hatasını yakalıyor ve mesajı hep "Bu Müşteri bulunamadı" diyor. Yani bu mesaj her durumda doğru değil (örneğin fiyat veritabanının sınırını aşarsa da aynı mesaj çıkar). Menü 5'te ise olmayan bir sipariş ya da ürün Id'si hâlâ genel mesajı veriyor ("Geçersiz bir değer girdiniz"), nedenini ayırt etmiyor.
- Listeleme seçeneklerinde (1, 3, 4) try-catch yok.
- Veritabanına bağlanma hatası (SQL Server kapalıysa) için try-catch var, program mesaj yazıp kapanıyor. Program çalışırken bağlantı sonradan koparsa korunmuyor.
- Siparişin fiyatı, ona kalem eklenince kalemlerin toplamından hesaplanıyor. Menü 2'de fiyat hâlâ elle girilir ve o siparişe kalem eklenince elle girilen fiyatın yerini kalemlerin toplamı alır. Hiç kalemi olmayan siparişler elle girilen fiyatta kalır.
- Menü 5'te kalem ekleme ve fiyat güncelleme iki ayrı komut. İkincisi hata verirse ilki geri alınmıyor (bunun için "transaction" denen bir şey var, henüz öğrenmedim).
- Menü 5'te Id'ler ve adet, kontrol edildikten sonra veritabanına yine yazı olarak gönderiliyor (menü 2'de fiyat sayı olarak gidiyor). Menü 2'de müşteri Id'si de yazı olarak gidiyor. Adet ve Id için üst sınır kontrolü de yok.
- Silme ve güncelleme C# menüsünde yok (UPDATE ve DELETE'i sadece SSMS'te kullandım).

## Sonraki adımlar

- Menü 5'e de ayrı hata mesajları (`SqlException`) eklemek
- Id ve adedi veritabanına çevrilmiş sayı olarak göndermek
- Kodu metotlara ve sınıflara bölmek
