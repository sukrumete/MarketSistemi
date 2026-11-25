# Market Sistemi

MarketSistemi'ni yaparken nasıl ilerlediğimi, nerede takıldığımı ve yapay zekâdan nerede yardım aldığımı anlatan not.

## Amaç

SQL'in ikinci yarısını (JOIN, gruplama, ilişkili tablolar) öğrenmek ve bunu C# ile bağlamak. Projeyi önce 6 küçük sürümde (V1-V6) yaptım, sonra C# tarafını V7, V8, V9 ve V10 ile geliştirmeye devam ettim. Her sürümde bir konu öğrendim, SQL komutlarını önce SSMS'te yazıp denedim. V10 ile proje tamamlandı.

## Sürümler

- **V1:** Tablolar ve anahtar bağlantısı. İki tablo (Musteriler, Siparisler) kurdum, PRIMARY KEY ve FOREIGN KEY'in ne işe yaradığını öğrendim.
- **V2:** INNER JOIN ve LEFT JOIN. İki tabloyu yan yana getirmeyi, hiç siparişi olmayan müşteriyi LEFT JOIN ile bulmayı öğrendim.
- **V3:** COUNT, SUM, GROUP BY, HAVING. Saymak, toplamak, müşteriye göre gruplamak ve grupları HAVING ile ayıklamak.
- **V4:** Urunler ve SiparisKalemleri. Tablo sayısı dörde çıktı. Bir siparişin içinde birden fazla ürün olabildiği için ara tablo (SiparisKalemleri) gerektiğini öğrendim. Çoklu JOIN yazdım.
- **V5:** TOP, BETWEEN, LIKE, DISTINCT. Sonuçları sınırlamak, aralık seçmek, yazıda arama yapmak, tekrarları atmak.
- **V6:** C# ile bağlamak. SqlConnection, SqlCommand, SqlDataReader; konsol menüsü; `while` döngüsü; parametreli INSERT; JOIN'li sorguları C#'tan çalıştırmak. Sonra `MarketKurulum.sql` ile veritabanını sıfırdan kurulabilir hale getirdim ve GitHub'a yükledim.
- **V7:** Hata yönetimi. Sipariş eklerken harf ya da olmayan bir Id girince program çöküyordu, `try-catch` ile korudum. Sonra veritabanı bağlantısını açan `hat.Open()` satırını da `try-catch` içine aldım. Sunucu adını bilerek yanlış yazarak denedim: mesaj çıkıyor ama menü yine geliyordu ve ilk sorguda program çöküyordu. `catch` içine `return;` ekleyince program mesajdan sonra kapandı.
- **V8:** Sipariş kalemi ekleme ve toplam fiyat. Menüye 5. seçenek olarak "Sipariş Kalemi Ekle" ekledim (INSERT, parametreli, `try-catch` korumalı). Sonra siparişin fiyatını kalemlerden otomatik hesaplamayı yaptım. Önce SSMS'te adım adım denedim: `Fiyat * Adet` ile satır tutarı, `SUM` ile toplam, test için eklediğim hatalı satırı `DELETE` ile silmek, `UPDATE` ile fiyatı yazmak, sonra `UPDATE`'in içine `SELECT SUM(...)` koymak (iç içe sorgu). Çalıştığını fiyatı bilerek 1'e düşürüp tekrar hesaplatarak ispatladım. Sonra aynı `UPDATE`'i C#'ta menü 5'in içine ikinci bir komut olarak taşıdım: kalem eklenince siparişin Fiyat'ı kendiliğinden güncelleniyor.
- **V9:** Girdi kontrolü ve daha anlaşılır hata mesajları. Menü 5'te adet için `int.TryParse` ile kontrol ekledim (sayı olmalı ve sıfırdan büyük olmalı, `if/else` ile). Sonra menü 2'de fiyatı `decimal.TryParse` ile kontrol ettim ve eksi fiyatı engelledim. Testte `100.50` yazınca tabloya 10050 girdiğini gördüm: Türkçe bilgisayar noktayı binlik ayracı sayıyordu. `TryParse`'tan önce `Replace` ile noktayı virgüle çevirdim ve SQL'e yazı yerine çevrilmiş sayıyı gönderdim. Sonra `catch` bloğunu `catch (Exception hata)` yapıp `hata.Message` ile gerçek nedeni gördüm (FOREIGN KEY hatası). Bu hatayı Türkçe ve anlaşılır göstermek için `catch (SqlException)` ekledim. Ama `abc` yazınca "müşteri bulunamadı" demesi yanlıştı, çünkü `SqlException` her veritabanı hatasını yakalıyor. Bunun üzerine müşteri Id'sini de `TryParse` ile kontrol ettim ve iç içe `if` ile ayrı bir uyarı verdim. Menü 5'te sipariş Id, ürün Id ve adedi tek bir `if` içinde `&&` ile birleştirdim. En sonda denemelerden kalan test siparişlerini ve kalemlerini `DELETE` ile (önce aynı `WHERE` ile `SELECT` yapıp neyin silineceğine bakarak) temizledim ve sipariş 1'in fiyatını iç içe `UPDATE` ile yeniden hesaplattım.
- **V10:** Hata yakalamayı tamamlamak ve metotlara bölmek. Menü 5'in `catch`'ini menü 2'ye bakarak `SqlException` ve `Exception` diye ayırdım. Menü 5'te sipariş Id, ürün Id ve adedi SQL'e yazı yerine çevrilmiş sayı olarak göndermeye başladım, menü 2'de de müşteri Id'sini sayı olarak gönderdim. Listeleme menülerine (1, 3, 4) `try/catch` ekledim. Sonra bütün menüleri metotlara taşıdım: `UrunleriListele`, `SiparisleriListele`, `SiparisKalemleriniListele`, `SiparisEkle`, `SiparisKalemiEkle`. Bağlantı `Main`'de bir kez açılıyor ve her metoda parametre olarak veriliyor. Hepsini F5 ile baştan test ettim. Testte bir siparişe kalem eklediğim için fiyat bozulmuştu, test satırlarını `SELECT` ile bakıp `DELETE` ile sildim ve fiyatı `UPDATE` ile geri düzelttim. Proje bununla tamamlandı.

## Takıldığım yerler

- **Tablo ile kutuyu karıştırmak:** `FROM`'dan sonra tablo, `SELECT`/`WHERE`/`GROUP BY`'dan sonra kutu adı gelir. Bunu çok karıştırdım.
- **SQL yazım hataları:** SELECT'te kutular arasına virgül unutmak, `REFERENCES Tablo(Id)` biçimini yanlış yazmak, `Fiyat.DESC` (boşluk olmalı), GROUP BY ile SELECT'teki kutuların uyuşmaması.
- **Türkçe klavye:** `IDENTITY`, `JOIN`, `Fiyat`, `Id` gibi kelimelerde noktalı İ yazılıyordu, SQL kabul etmiyordu. Aynısı C#'ta da oldu: `siparisıdSayisi` yazmışım, büyük/küçük harfe duyarlı olduğu için başka bir isim sayıldı ve kırmızı hata verdi (doğrusu `siparisIdSayisi`).
- **C# tarafı:** `oku.Read` yazıp parantez koymamak, `=` ile `==` karışıklığı, `else`'e şart yazmaya çalışmak, süslü parantezleri doğru iç içe koymak, değişken adında Türkçe karakter ve boşluk kullanmak.
- **DataReader kapatmamak:** Menü ikinci kez çalışınca "açık DataReader var" hatası aldım. `oku.Close()` yazınca çözüldü.
- **`GO` komutu:** `MarketKurulum.sql`'i sıfırdan denerken `USE` komutu hata verdi, `GO` ile çözdük.
- **UPDATE ve DELETE'te WHERE:** `WHERE`'i yanlış yere yazdım (tırnağın dışına), yanlış kutu adı yazdım (`SiparisId` yerine `Id`) ve ilk denemede `WHERE 1005 = 999` gibi anlamsız bir şey yazdım. `WHERE` olmazsa bütün satırların değiştiğini ya da silindiğini öğrendim. Test verisini silerken bir keresinde asıl siparişi silecek bir koşul yazmıştım (`WHERE Id = 2`), çalıştırmadan önce yapay zekâ fark ettirdi. O günden sonra silmeden önce aynı `WHERE` ile `SELECT` yapıyorum.
- **İç içe SELECT:** Bu projenin en zor yeriydi. `UPDATE ... SET Fiyat = ( SELECT ... )` kalıbını anlamakta çok zorlandım, parantezi ve iki `WHERE`'i doğru yere koymak uzun sürdü.
- **Catch içinde sadece mesaj yazmak:** Bağlantı hatasında program çökmekten kurtuldu ama devam edip sonra başka bir yerde çöktü. `return;` ile bitirmek gerekiyormuş.
- **Girdi kontrolü kutularını karıştırmak:** `TryParse` iki kutu veriyor, "geçerli mi" (bool) ve "sayı". `if` içinde bool kutusunu sayı gibi, sayı kutusunu tek başına şart gibi kullandım, `<` işaretini ters yazdım. "Geçerli ve sayı" şeklinde bir çift olarak düşününce oturdu.
- **Nokta ve virgül tuzağı:** `100.50` yazınca tabloya 10050 girdi ve hiç hata çıkmadı. Sessizce yanlış sonuç vermesi en tehlikeli kısımdı.
- **Hata mesajı her zaman doğru değil:** `SqlException` her veritabanı hatasını yakaladığı için `abc` yazınca da "müşteri bulunamadı" çıktı. Doğru mesaj için girdiyi veritabanına gitmeden önce kontrol etmem gerektiğini öğrendim.
- **SQL'e yazı yerine sayı göndermek:** Menü 5'te `TryParse` ile sayıya çevirmiştim ama `AddWithValue`'ya hâlâ eski yazı kutusunu veriyordum. "Çuvalı kaba boşalttım ama hâlâ çuvalı kullanıyorum" benzetmesiyle, `bool` kutusu ile `Sayisi` ile biten sayı kutusunu ayırt edince oturdu.
- **Metotta bağlantıyı görememek:** Menü 1'i metoda taşıyınca `hat` kırmızı oldu, çünkü metot `Main`'deki bağlantıyı göremiyor. İlk denemede metodun içinde yeni bir bağlantı açmaya çalıştım, bağlantı satırını metodun başlığı ile `{` arasına yazdım ve `Main`'in adını değiştirmeye kalktım. Çözüm bağlantıyı parametre olarak vermekmiş (`SqlConnection baglanti`). Metodun içinde parantezde yazdığım ismi kullanmam gerektiğini ve bu ismin `Main`'dekiyle aynı olmak zorunda olmadığını da öğrendim.
- **Yanlış menünün kodunu taşımak:** `UrunleriListele` metoduna ilk önce menü 3'ün kodunu koymuştum.
- **Boş `catch` ve yanlış mesaj:** Listeleme menülerinde ilk `catch`'i boş bırakmıştım, bu yüzden hata olunca hiçbir şey yazmayacaktı. Mesajı da yanlış menüden almıştım ("Sipariş Eklenemedi" yazmıştım).
- **Test verisi dersi:** Menü 5'i test ederken Ahmet'in gerçek siparişine kalem ekledim, fiyatı 100'den 180'e çıktı. Kalemi silince fiyat kendiliğinden geri dönmüyor. Kalemi olmayan siparişte toplamı hesaplayan `UPDATE` boş (NULL) döneceği için fiyatı doğrudan 100 yazarak geri düzelttim.

## Yapay zekâ yardımı

Çoğunlukla Claude ile çalıştım. Küçük adımlarla, önce kendim denedim, takılınca ipucu aldım.

**Yapay zekânın yaptığı:**

- Konuları benzetmelerle ve sırayla anlattı, V1-V6 planını çıkardı.
- Kalıpları gösterdi: INSERT ... VALUES, @ yer tutucu ve AddWithValue, ExecuteReader/ExecuteNonQuery farkı, JOIN'deki ON eşleşmesi, try-catch yapısı. Bazı JOIN iskeletlerini o gösterdi.
- SiparisKalemleri ara tablosu fikrini gösterdi, menü için `devam` değişkenli döngüyü ve `GETDATE()`'i önerdi.
- Bağlantı hatasında programı `return` ile bitirmem gerektiğini gösterdi.
- SELECT içinde hesap yapmayı (`Fiyat * Adet`) ve `UPDATE`'in içine `SELECT` koymayı (iç içe sorgu) o gösterdi. Bunu yapay zekâ olmadan yapamazdım.
- Toplam fiyatı C#'a taşırken gereken üç satırın (SqlCommand, AddWithValue, ExecuteNonQuery) kalıbını listeledi.
- Girdi kontrolünün kalıbını gösterdi: `TryParse` sonucunu bool kutusuna koyup `if` içinde `&&` ile kullanmak, iç içe `if` yapısı, `catch (SqlException)` ile `catch (Exception)` ayrımı. Boşluklu ipuçlarıyla yönlendirdi.
- `100.50` → 10050 sorununun nedenini açıkladı, `Replace` ile çözme fikrini ve SQL'e yazı yerine çevrilmiş sayıyı göndermeyi gösterdi.
- `catch (Exception hata)` ve `hata.Message` ile hatanın nedenini görmeyi gösterdi.
- Metot kavramını benzetmeyle anlattı, metodun iskeletini (`static void ___()`) ve bağlantıyı parametre olarak vermek gerektiğini gösterdi.
- Listeleme menülerinde `try/catch` gerektiğini ve `SqlException`'ın ne işe yaradığını anlattı. Menü 2'de müşteri Id'sinin hâlâ yazı olarak gittiğini fark ettirdi.
- Hatalarımda ipucu verdi (ters `<` işareti, yanlış DELETE koşulu, kutu adı karışıklıkları, `siparisıdSayisi` yazımı, boş `catch`, yanlış hata mesajı), düzeltmeyi çoğu zaman bana bıraktı. Test verisini silerken önce `SELECT` yapma kuralını hatırlattı.
- Notlarımın, README'nin ve bu dosyanın ilk taslağının hazırlanmasına yardım etti.

**Benim yaptığım:**

- C# kodunu (Program.cs) ipuçlarıyla satır satır kendim yazdım (V7, V8, V9 ve V10 dahil).
- Menü 5'in `catch` bloklarını menü 2'ye bakarak kendim ayırdım. `AddWithValue` içinde yazı yerine sayı kutusunu göndermeyi ipucuyla kendim buldum.
- Listeleme menüleri için `try/catch`'leri ve mesajları kendim yazdım. Menü 3 ve 4'ü tek başıma yaptım.
- Metotların içini ve `Main`'deki çağrıları kendim yazdım. Menü 1'i ipuçlarıyla taşıdım, menü 3, 4, 2 ve 5'i örneğe bakarak kendim taşıdım. Parametre adını (`baglanti`) ben seçtim.
- SQL sorgularını SSMS'te kendim yazıp denedim. İç içe sorguda yapay zekânın gösterdiği kalıba kendi SELECT sorgumu koydum, hataları düzelttim, sonucu ispatladım ve C#'a kendim taşıdım.
- Programı farklı girdilerle (harf, 0, eksi sayı, olmayan Id, noktalı ve virgüllü fiyat) kendim test ettim ve sonuçlara bakarak kodu düzelttim. Metotlara taşıdıktan sonra bütün menüleri baştan test ettim.
- Test için eklediğim hatalı satırları ve siparişleri `DELETE` ile kendim sildim, sipariş fiyatlarını `UPDATE` ile düzelttim.
- `MarketKurulum.sql` içindeki tabloları ve INSERT'leri kendim yazdım. Urunler tablosunun hangi bilgileri tutacağını (isim, marka, fiyat) kendim düşündüm.
- Notlarımı (sql-notlarim.txt ve C# notları) kendi dosyamda tuttum ve düzenledim. Bölümlerin taslağını yapay zekâ yazdı, ben dosyama ekleyip kontrol ettim.
- Sıfırdan kurulumu başka bir veritabanı adıyla kendim test ettim. Bağlantı korumasını sunucu adını bilerek yanlış yazarak kendim denedim.

## Öğrendiklerim

- Veriyi tablolara bölüp FOREIGN KEY ile bağlamak, tekrarı azaltır. FOREIGN KEY C#'tan eklerken de çalışır: olmayan Id'yi reddeder.
- JOIN, "numarası aynı olanları yan yana koy" demektir.
- SELECT ExecuteReader ile, INSERT/UPDATE/DELETE ExecuteNonQuery ile çalışır; kullanıcıdan gelen bilgi @ yer tutucuyla SQL'e girer.
- Bir komutu önce SSMS'te deneyip sonra C#'a taşımak hataları azaltır.
- `try-catch` programı çökmekten kurtarır, `return` de "burada dur" der. Başarı mesajı `try`'ın en sonuna yazılır.
- SELECT içinde hesap yapılabilir (`Fiyat * Adet`), `SUM` bunları toplar, `UPDATE`'in içine SELECT konabilir.
- `UPDATE` ve `DELETE`'te `WHERE` unutulursa bütün satırlar etkilenir. Silmeden önce aynı `WHERE` ile `SELECT` yapıp neyin silineceğine bakmak güvenli.
- Girdiyi veritabanına göndermeden önce `TryParse` ile kontrol etmek, hem çökmeyi hem de yanlış hata mesajını önler.
- Türkçe bilgisayarda `decimal.TryParse` noktayı binlik ayracı sayar, `100.50` 10050 olabilir ve hata vermez. Bunu önlemek için noktayı virgüle çevirdim ve SQL'e yazı yerine çevrilmiş sayıyı gönderiyorum.
- `catch (SqlException)` her veritabanı hatasını yakalar, yani "müşteri bulunamadı" mesajı her SQL hatası için doğru olmayabilir. Özel hata önce, genel hata en sonda yazılır.
- Metot, isim verilmiş küçük bir iştir. Bir kez yazılır, `Main`'den ismiyle çağrılır. Kod kısalır ve hata bulmak kolaylaşır.
- Bir metot, başka yerde tanımlı bir değişkeni (örneğin bağlantıyı) göremez. Parametre olarak verilir: `static void Metot(SqlConnection baglanti)`. Metodun içinde parantezde yazılan isim kullanılır, bu isim `Main`'dekiyle aynı olmak zorunda değildir.
- Her menünün `try/catch`'i ayrı olmalı. Veritabanı hatası olunca program çökmesin, kullanıcıya menüye uygun bir mesaj versin.
- Test yaparken gerçek veriye dokunmamak lazım. Dokunduysam neyin değiştiğine bakıp (`SELECT`) geri düzeltmem gerekiyor. Kalemi silmek siparişin fiyatını kendiliğinden geri döndürmüyor.

## İleride öğrenmek istediklerim

Bu proje tamamlandı. Aşağıdakileri ileride, başka projelerde öğrenmeyi düşünüyorum:

- Transaction
- `finally` / `using`
- Kodu sınıflara bölmek
