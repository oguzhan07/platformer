# Platformer

Unity 6 ve C# ile geliştirdiğim 2D platform oyunu.

> **Durum: geliştirme devam ediyor.** Oyuncu, düşman ve toplanabilir sistemleri çalışıyor;
> oyun henüz bitmedi.

<!-- Buraya 3-4 ekran görüntüsü ekle. docs/ klasörü açıp PNG'leri koy:
![Oynanış](docs/gameplay.png)
![Düşman durumları](docs/enemy.png)
-->

## Şu an çalışanlar

### Oyuncu

Yatay hareket, yer kontrollü zıplama, blok (guard) ve rastgele seçilen iki farklı saldırı
animasyonu. Saldırı, düşman layer mask'ine karşı `Physics2D.OverlapCircle` ile yapılıyor —
yani sadece o katmandaki nesneler vurulabiliyor, sahnedeki her şey taranmıyor.

Animator parametreleri string yerine `Animator.StringToHash` ile önceden hash'lenip saklanıyor;
her karede string karşılaştırması yapılmıyor.

### Düşmanlar — durum makinesi

Düşman davranışı `EnemyState` enum'ı üzerinden üç durumda yönetiliyor. `Update` içinde bir
`switch` var, iç içe if yığını değil:

| Durum | Ne yapar | Nereye geçer |
|---|---|---|
| `Patrol` | İki nokta arasında devriye gezer | Oyuncu takip mesafesine girince → `Follow` |
| `Follow` | Oyuncuya doğru yürür | Saldırı mesafesine girince → `Attack`<br>Takip mesafesinden çıkınca → `Patrol` |
| `Attack` | Oyuncuya hasar verir, yönünü oyuncuya çevirir | Oyuncu uzaklaşınca → `Patrol` |

Devriye sınırları sahneye yerleştirilen `Point` etiketli tetikleyicilerle belirleniyor; düşman
bir sınıra değince yön çarpanı tersine dönüyor. Yani devriye alanı kodda değil, seviye
tasarımında ayarlanıyor.

### Düşmanlar — veriye dayalı tasarım

Düşman istatistikleri düşman scriptine gömülü değil. Her düşman; adını, hasarını, canını,
hareket hızını, takip ve saldırı mesafelerini ve sprite'ını bir **`EnemyType` ScriptableObject**
asset'inden okuyor.

Bunun pratik sonucu: yeni bir düşman türü eklemek kod yazmayı değil, editörde yeni bir asset
oluşturup değerleri girmeyi gerektiriyor. Şu an iki tür var:

| Tür | Hasar | Can | Hareket hızı |
|---|---|---|---|
| Archer | 2 | 5 | 1.0 |
| Lancer | 1 | 7 | 1.5 |

### Can barı

DOTween ile ölçek ve renk animasyonu — can azaldıkça bar yeşilden kırmızıya kayıyor. Arkada
gecikmeli hareket eden ikinci bir bar var; oyuncunun tek seferde ne kadar hasar aldığını görmesini
sağlayan klasik "beyaz bar" efekti.

### Toplanabilirler ve arayüz

Havada süzülen altınlar (DOTween yoyo döngüsü), TextMeshPro ile altın sayacı, menüde müzik ve
bilgi butonları.

### Ayar kolaylığı

Oyuncunun saldırı yarıçapı ile düşmanın takip ve saldırı mesafeleri `OnDrawGizmos` ile sahne
görünümünde çiziliyor. Değerler kör ayarlanmıyor, sahnede görülerek ayarlanıyor.

## Kod yapısı

```
Assets/Project/Scripts/
├── Player.cs            hareket, zıplama, blok, saldırı, hasar alma
├── HealthBar.cs         can barı animasyonu (DOTween + coroutine)
├── Gold.cs              toplanabilir altın
├── GoldManager.cs       altın sayacı
├── Enemy/
│   ├── Enemy.cs         durum makinesi: devriye, takip, saldırı
│   └── EnemyType.cs     düşman istatistiklerini tutan ScriptableObject
└── Buttons/             müzik ve bilgi butonları
```

## Kullanılan teknolojiler

Unity 6 (6000.3.15f1) · C# · Rigidbody2D ve 2D fizik · URP 2D · DOTween · TextMeshPro

## Henüz yapılmadı

- Bölüm tasarımı ve birden fazla seviye
- Ses ve müzik
- Kazanma / kaybetme döngüsü, kayıt sistemi
- Oyuncu haritadan düştüğünde ölüm animasyonu oynuyor ama karakter sahneden kaldırılmıyor

## Görseller

Pixel art hazır paketlerden geliyor, ikisi de ücretsiz ve ticari kullanıma açık:

- **Tiny Swords** — Pixel Frog
- **Apocalypse Pixel Pack – Black & White Edition**

Oyunun kodu bana ait.
