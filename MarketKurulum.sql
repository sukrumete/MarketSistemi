CREATE DATABASE MarketDb
GO
USE MarketDb
GO
CREATE TABLE Musteriler
(
Id int IDENTITY(1,1) PRIMARY KEY,
Isim nvarchar(50),
Soyad nvarchar(50)
)

CREATE TABLE Urunler
(
Id int IDENTITY(1,1) PRIMARY KEY,
Isim nvarchar(50),
Marka nvarchar(50),
Fiyat decimal(10,2)
)

CREATE TABLE Siparisler
(
Id int IDENTITY(1,1) PRIMARY KEY,
MusteriId int,
Tarih date,
Fiyat decimal(10,2),
FOREIGN KEY (MusteriId)  REFERENCES Musteriler(Id)
)

CREATE TABLE SiparisKalemleri
(
Id int IDENTITY(1,1) PRIMARY KEY,
SiparisId int,
UrunId int,
Adet int,
FOREIGN KEY (SiparisId) REFERENCES Siparisler(Id),
FOREIGN KEY (UrunId) REFERENCES Urunler(Id)
)

INSERT INTO Musteriler (Isim, Soyad)
VALUES ('Mete', 'Han')

INSERT INTO Musteriler (Isim, Soyad)
VALUES ('Ahmet',  N'Neşer')

INSERT INTO Musteriler (Isim, Soyad)
VALUES ('Berke',  N'Naşet')

INSERT INTO Urunler (Isim, Marka, Fiyat)
VALUES (N'Süt', 'Kuzey Mera', 45)

INSERT INTO Urunler (Isim, Marka, Fiyat)
VALUES ('Peynir', N'Güney Mera', 100)

INSERT INTO Urunler (Isim, Marka,Fiyat)
VALUES ('Ekmek', N'Fırın', 15)

INSERT INTO Siparisler (MusteriId, Tarih, Fiyat)
VALUES (1, '2026-11-13', 35.40)

INSERT INTO Siparisler (MusteriId, Tarih, Fiyat)
VALUES (2, '2027-05-22', 50.40)

INSERT INTO Siparisler (MusteriId, Tarih, Fiyat)
VALUES (1, '2026-11-07', 785.40)

INSERT INTO SiparisKalemleri (SiparisId, UrunId, Adet)
VALUES (1, 1, 2)

INSERT INTO SiparisKalemleri (SiparisId, UrunId, Adet)
VALUES (1, 3, 1)