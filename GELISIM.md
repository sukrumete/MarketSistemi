# Market Sistemi 

MarketSistemi'ni yaparken nasıl ilerlediğimi, nerede takıldığımı ve yapay zekâdan nerede yardım aldığımı anlatan not.

## Amaç

SQL'in ikinci yarısını (JOIN, gruplama, ilişkili tablolar) öğrenmek ve bunu C# ile bağlamak. Projeyi 6 küçük sürümde (V1-V6) yaptım. Her sürümde bir konu öğrendim, SSMS'te komutları yazıp denedim.

## Sürümler

- **V1: Tablolar ve anahtar bağlantısı.** İki tablo (Musteriler, Siparisler) kurdum, PRIMARY KEY ve FOREIGN KEY'in ne işe yaradığını öğrendim.
- **V2: INNER JOIN ve LEFT JOIN.** İki tabloyu yan yana getirmeyi, hiç siparişi olmayan müşteriyi LEFT JOIN ile bulmayı öğrendim.
- **V3: COUNT, SUM, GROUP BY, HAVING.** Saymak, toplamak, müşteriye göre gruplamak ve grupları HAVING ile ayıklamak.
- **V4: Urunler ve SiparisKalemleri.** Tablo sayısı dörde çıktı. Bir siparişin içinde birden fazla ürün olabildiği için ara tablo (SiparisKalemleri) gerektiğini öğrendim. Çoklu JOIN yazdım.
- **V5: TOP, BETWEEN, LIKE, DISTINCT.** Sonuçları sınırlamak, aralık seçmek, yazıda arama yapmak, tekrarları atmak.
- **V6: C# ile bağlamak.** SqlConnection, SqlCommand, SqlDataReader; konsol menüsü; `while` döngüsü; parametreli INSERT; JOIN'li sorguları C#'tan çalıştırmak. Sonra `MarketKurulum.sql` ile veritabanını sıfırdan kurulabilir hale getirdim ve GitHub'a yükledim.

## Takıldığım yerler

- **Tablo ile kutuyu karıştırmak:** `FROM`'dan sonra tablo, `SELECT`/`WHERE`/`GROUP BY`'dan sonra kutu adı gelir. Bunu çok karıştırdım.
- **SQL yazım hataları:** SELECT'te kutular arasına virgül unutmak, `REFERENCES Tablo(Id)` biçimini yanlış yazmak, `Fiyat.DESC` (boşluk olmalı), GROUP BY ile SELECT'teki kutuların uyuşmaması.
- **Türkçe klavye:** `IDENTITY`, `JOIN`, `Fiyat` gibi kelimelerde noktalı İ yazılıyordu, SQL kabul etmiyordu.
- **C# tarafı:** `oku.Read` yazıp parantez koymamak, `=` ile `==` karışıklığı, `else`'e şart yazmaya çalışmak, süslü parantezleri doğru iç içe koymak.
- **DataReader kapatmamak:** Menü ikinci kez çalışınca "açık DataReader var" hatası aldım. `oku.Close()` yazınca çözüldü.
- **`GO` komutu:** `MarketKurulum.sql`'i sıfırdan denerken `USE` komutu hata verdi, `GO` ile çözdük.

## Yapay zekâ yardımı 

Çoğunlukla Claude ile çalıştım. Küçük adımlarla, önce kendim denedim, takılınca ipucu aldım.

**Yapay zekânın yaptığı:**
- Konuları benzetmelerle ve sırayla anlattı, V1-V6 planını çıkardı.
- Kalıpları gösterdi: INSERT ... VALUES, @ yer tutucu ve AddWithValue, ExecuteReader/ExecuteNonQuery farkı, JOIN'deki ON eşleşmesi. Bazı JOIN iskeletlerini o gösterdi.
- SiparisKalemleri ara tablosu fikrini gösterdi, menü için `devam` değişkenli döngüyü ve `GETDATE()`'i önerdi.
- Hatalarımda ipucu verdi, düzeltmeyi çoğu zaman bana bıraktı.
- Notlarımın ve README ile bu dosyanın ilk taslağının hazırlanmasına yardım etti.

**Benim yaptığım:**
- C# kodunu (Program.cs) ipuçlarıyla satır satır kendim yazdım.
- SQL sorgularını SSMS'te kendim yazıp denedim.
- `MarketKurulum.sql` içindeki tabloları ve INSERT'leri kendim yazdım. Urunler tablosunun hangi bilgileri tutacağını (isim, marka, fiyat) kendim düşündüm.
- Notlarımı (sql-notlarim.txt) kendi dosyamda tuttum ve yazdım.
- Sıfırdan kurulumu başka bir veritabanı adıyla kendim test ettim.

## Öğrendiklerim

- Veriyi tablolara bölüp FOREIGN KEY ile bağlamak, tekrarı azaltır.
- JOIN, "numarası aynı olanları yan yana koy" demektir.
- SELECT ExecuteReader ile, INSERT ExecuteNonQuery ile çalışır; kullanıcıdan gelen bilgi @ yer tutucuyla SQL'e girer.
- Bir komutu önce SSMS'te deneyip sonra C#'a taşımak hataları azaltır.

## Sonraki adımlar

- try-catch ile hata yönetimi
- Sipariş kalemi ekleme ve toplam fiyatı otomatik hesaplama
- Kodu metotlara ve sınıflara bölmek
