# 📧 Mailora – E-Posta ve Mesajlaşma Yönetim Sistemi

ASP.NET Core MVC · Entity Framework Core · SQL Server · ASP.NET Core Identity

Kullanıcıların hesap oluşturabildiği, e-posta adreslerini doğrulayabildiği, mesaj gönderip alabildiği ve e-posta kutularını yönetebildiği; kullanıcı bazlı dinamik dashboard ve SMTP e-posta altyapısı içeren web tabanlı bir uygulama.

* * *

## 📸 Ekran Görüntüleri
### 🔐 Giriş & Kayıt Ekranları
<img width="1906" height="945" alt="Register-Signup" src="https://github.com/user-attachments/assets/5b16bc7b-9373-4fb3-bbef-74c0be385c5b" />
<img width="1919" height="946" alt="Login-Signin" src="https://github.com/user-attachments/assets/43d3e196-3eac-441b-b217-137629dcd980" />
<img width="1918" height="944" alt="Login-ForgorPassword" src="https://github.com/user-attachments/assets/62487696-d67d-4ed0-8cd4-26002deec92f" />

### ✉️ E-Posta Doğrulama
<img width="1919" height="947" alt="Login-EmailConfirm" src="https://github.com/user-attachments/assets/2621bdcf-0f25-4459-9a2e-4bbc5fdc03eb" />

### 📊 Kullanıcı Dashboard
<img width="1919" height="946" alt="Dashboard-Index-1" src="https://github.com/user-attachments/assets/ee1057bf-b10d-429e-9efd-59d597c7e0fc" />
<img width="1918" height="944" alt="Dashboard-Index-2" src="https://github.com/user-attachments/assets/dc9eb8fa-b12c-4999-9522-532205f91505" />

### 📥 Gelen Kutusu
<img width="1919" height="943" alt="Message-Inbox" src="https://github.com/user-attachments/assets/f27e8e65-a631-459b-b159-bd234356404c" />

### 📤 Gönderilen Mesajlar
<img width="1919" height="945" alt="Message-Sendbox" src="https://github.com/user-attachments/assets/21d8c972-4ae6-49f8-94cd-a1b5b2e0945e" />

### 📨 Mesaj Detayı & Mesaj Oluşturma
<img width="1918" height="942" alt="Message-CreateMessage" src="https://github.com/user-attachments/assets/2213af68-4959-4dae-859b-774b34bea9e0" />
<img width="1919" height="946" alt="Message-Detail" src="https://github.com/user-attachments/assets/b312ed11-e6ee-4cb3-8aa3-9a4c9a686f0c" />

### ⭐ Favoriler, Taslaklar & Çöp Kutusu
<img width="1919" height="945" alt="Message-FavoriteMessage" src="https://github.com/user-attachments/assets/6d4ecd18-4a1b-4f72-ab0f-f2eaee03d251" />
<img width="1919" height="944" alt="Message-Draft" src="https://github.com/user-attachments/assets/995650ca-4014-4e56-b821-df84acfd8eb7" />
<img width="1919" height="943" alt="Message-Trashes" src="https://github.com/user-attachments/assets/a58757e5-f340-481d-bbe4-aa1e8af120b1" />

### 👤 Profil Yönetimi
<img width="1661" height="1397" alt="Profile-MyProfile" src="https://github.com/user-attachments/assets/9a5e942d-1c41-4631-bbd1-60785ca4da37" />

* * *

## 📖 Proje Hakkında

Mailora, MY Academy eğitimi kapsamında geliştirilen ASP.NET Core MVC tabanlı bir e-posta ve mesajlaşma yönetim uygulamasıdır.

Uygulama; kullanıcı kimlik doğrulama süreçlerini, e-posta gönderim işlemlerini ve mesaj yönetimini tek bir platformda bir araya getirir.

Uygulama iki temel bölümden oluşur:

1. **Kullanıcı Paneli:** Kullanıcıların gelen ve gönderilen mesajlarını görüntüleyebildiği, yeni mesaj oluşturabildiği, mesajlarını kategorilere ayırabildiği ve profil bilgilerini yönetebildiği alan.
2. **Dashboard:** Oturum açmış kullanıcıya ait mesaj istatistiklerinin, grafiklerin ve son mesajların dinamik olarak görüntülendiği alan.

Uygulamadaki mesaj ve dashboard verileri aktif kullanıcıya göre filtrelenir. E-posta gönderme işlemleri merkezi bir servis üzerinden yürütülür; SMTP ayarları yapılandırma sistemi ve Dependency Injection aracılığıyla yönetilir.

* * *

## ✨ Özellikler

### 👤 Kullanıcı & Kimlik Yönetimi

- ASP.NET Core Identity ile kullanıcı kayıt ve giriş işlemleri
- Kayıt sırasında e-posta doğrulama kodu gönderimi
- E-posta doğrulama süreci
- Şifremi unuttum ve e-posta üzerinden şifre yenileme bağlantısı gönderimi
- Kullanıcı profil bilgilerinin görüntülenmesi ve güncellenmesi
- Profil görseli ve kişisel bilgilerin yönetimi
- Kullanıcı şifresini değiştirme

### ✉️ Mesaj Yönetimi

- Gelen kutusu: Kullanıcıya gelen mesajları listeleme
- Gönderilen kutusu: Kullanıcının gönderdiği mesajları listeleme
- Mesaj detaylarını görüntüleme
- Yeni mesaj oluşturma ve gönderme
- Okundu / okunmadı durumunu yönetme
- Mesajları favorilere ekleme ve favorilerden çıkarma
- Taslak mesajları yönetme
- Silinen mesajları çöp kutusunda görüntüleme
- Mesajları kategorilere göre düzenleme

### 📊 Dashboard & İstatistikler

- Gelen mesaj sayısı
- Gönderilen mesaj sayısı
- Okunmamış mesaj sayısı
- Favori mesaj sayısı
- Taslak mesaj sayısı
- Son 7 güne ait gelen ve gönderilen mesaj aktivite grafiği
- Kategori bazlı mesaj dağılımı
- Kategorilere göre mesaj sayıları
- Okunma / okunmama durumu grafiği
- Son mesajların listelenmesi
- Dashboard bileşenlerinin ViewComponent mimarisiyle ayrıştırılması

### 📬 E-Posta Altyapısı

- MailKit ve MimeKit ile SMTP üzerinden e-posta gönderimi
- Düz metin e-posta gönderimi
- HTML formatında e-posta gönderimi
- Merkezi `IEmailService` / `EmailService` yapısı
- `IOptions<EmailSettings>` ile e-posta ayarlarının yönetimi
- SMTP parolasını User Secrets üzerinden saklayabilme

* * *

## 🧰 Kullanılan Teknolojiler

| Katman | Teknoloji |
|---|---|
| Framework | ASP.NET Core MVC |
| Dil | C# |
| ORM | Entity Framework Core (Code First) |
| Veritabanı | Microsoft SQL Server |
| Kimlik Yönetimi | ASP.NET Core Identity |
| E-Posta | MailKit, MimeKit, SMTP |
| Ön yüz | Razor Views, HTML5, CSS3, JavaScript |
| UI | Bootstrap, Bootstrap Icons |
| Grafikler | Chart.js |
| Mimari | MVC, ViewComponents, Service |
| Bağımlılık Yönetimi | Dependency Injection, Options Pattern |
| Gizli Bilgiler | .NET User Secrets |

* * *

## 🏗️ Proje Mimarisi

Proje, ASP.NET Core MVC yaklaşımıyla geliştirilmiştir. Controller sınıfları istekleri yönetirken, ViewModel yapıları ekranlara taşınan verileri düzenler. E-posta gönderme işlemleri servis katmanında merkezileştirilmiş; dashboard üzerindeki istatistik alanları ViewComponent yapısıyla birbirinden ayrılmıştır.

```text
MYAcademy_IdentityEmailProject/
│
├── Controllers/
│   ├── RegisterController.cs
│   ├── MessageController.cs
│   └── ...
│
├── Models/
│   ├── AppUser.cs
│   ├── Message.cs
│   ├── Category.cs
│   └── EmailSettings.cs
│
├── ViewModels/
│
├── Services/
│   ├── IEmailService.cs
│   └── EmailService.cs
│
├── ViewComponents/
│   └── Dashboard bileşenleri
│
├── Views/
│   ├── Account / Register / Message görünümleri
│   ├── MessageLayout/
│   └── Shared/
│
├── Data/
│   └── EmailContext.cs
│
├── Migrations/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

> Klasör ağacı, projede kullanılan temel sınıf ve sorumlulukları göstermek amacıyla hazırlanmıştır. GitHub'a eklemeden önce klasör adlarını mevcut çözümündeki gerçek yapıyla eşleştir.

### 🗃️ Veri Modeli

| Varlık | Açıklama |
|---|---|
| `AppUser` | ASP.NET Core Identity kullanıcı modelinden türetilir. Ad, soyad, açıklama, profil görseli ve doğrulama kodu gibi ek kullanıcı alanlarını içerir. |
| `Message` | Mesaj konusu, içeriği, gönderen e-posta adresi, alıcı e-posta adresi, gönderim tarihi, okunma ve favori durumlarını tutar. Taslak, çöp kutusu ve kategori bilgilerini de yönetir. |
| `Category` | Mesajların kategorilere ayrılmasını sağlar. |

İlişkiler: `EmailContext`, `IdentityDbContext<AppUser>` üzerinden Identity tablolarını ve uygulamanın `Message` ile `Category` tablolarını yönetir. Mesaj-kategori ilişkisi `CategoryId` alanı üzerinden kurulur.

* * *

## ⚙️ Kurulum ve Çalıştırma

### Gereksinimler

- .NET SDK (projenin hedeflediği sürüm)
- Microsoft SQL Server / SQL Server Express
- Visual Studio 2022 veya .NET destekleyen bir IDE
- E-posta gönderimi için SMTP hesabı (Gmail kullanılıyorsa uygulama parolası)

### Adımlar

**1. Repoyu klonlayın**

```bash
git clone https://github.com/yavuzsevimdev/IdentityEmailProject.git
cd IdentityEmailProject/MYAcademy_IdentityEmailProject
```

**2. Veritabanı bağlantısını yapılandırın**

`appsettings.json` dosyasındaki `ConnectionStrings` bölümünü kendi SQL Server ortamınıza göre düzenleyin.

Örnek bağlantı dizesi:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=IdentityEmailDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Bağlantı dizesinin anahtarını (`DefaultConnection` gibi) `Program.cs` veya `EmailContext` yapılandırmanda kullanılan isimle aynı tutmalısın.

**3. E-posta ayarlarını yapılandırın**

`appsettings.json` içerisinde SMTP parolası olmadan aşağıdaki ayarları tanımlayın:

```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "SenderName": "Mailora Yönetim Paneli",
    "SenderEmail": "YOUR_EMAIL_ADDRESS",
    "Username": "YOUR_EMAIL_ADDRESS"
  }
}
```

**4. SMTP parolasını User Secrets ile ekleyin**

Proje klasöründe terminal veya Package Manager Console üzerinden:

```bash
dotnet user-secrets init
dotnet user-secrets set "EmailSettings:Password" "YOUR_GMAIL_APP_PASSWORD"
```

Gmail kullanıyorsanız normal Google hesap parolanız yerine Google hesabınızdan oluşturduğunuz uygulama parolasını kullanın.

**5. Paketleri geri yükleyin ve projeyi derleyin**

```bash
dotnet restore
dotnet build
```

**6. Veritabanını oluşturun / güncelleyin**

Projede migration dosyaları mevcutsa:

```bash
dotnet ef database update
```

Migration oluşturmanız gerekiyorsa:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Çözümde DbContext ve başlangıç projesi farklı projelerdeyse komutlara uygun `--project` ve `--startup-project` parametrelerini ekleyin. Visual Studio Package Manager Console üzerinden de `Update-Database` kullanılabilir.

**7. Uygulamayı çalıştırın**

```bash
dotnet run
```

Uygulama, terminalde belirtilen localhost adresinde açılacaktır.

* * *

## 🔐 Güvenlik Notları

- SMTP parolası ve diğer hassas bilgiler kaynak koda yazılmamalıdır.
- Gerçek SMTP parolanızı veya gizli bağlantı bilgilerinizi GitHub deposuna yüklemeyin.
- Geliştirme ortamında hassas yapılandırmalar için .NET User Secrets kullanın.
- Şifre sıfırlama bağlantıları Identity tarafından üretilen güvenli ve süreli token mekanizmasıyla kullanılmalıdır.
- Üretim ortamında gizli bilgiler güvenli ortam değişkenleri veya secret yönetim servisleri üzerinden sağlanmalıdır.

* * *

## 🛣️ Geliştirme Planı (Roadmap)

- [x] ASP.NET Core Identity ile kullanıcı yönetimi
- [x] E-posta doğrulama ve şifre sıfırlama e-postaları
- [x] SMTP e-posta servisinin merkezi hale getirilmesi
- [x] Gelen / gönderilen mesaj yönetimi
- [x] Favoriler, taslaklar ve çöp kutusu
- [x] Kategori bazlı mesaj yönetimi
- [x] Kullanıcıya özel dashboard ve istatistikler
- [ ] Mesajlar için gelişmiş arama ve filtreleme
- [ ] E-posta ekleri ve dosya yönetimi
- [ ] Gerçek zamanlı bildirim altyapısı
- [ ] Rol ve yetki yönetiminin genişletilmesi

* * *

## 👨‍💻 Geliştirici

**Yavuz Bahadır Sevim**

- GitHub: https://github.com/yavuzsevimdev/
- LinkedIn: https://www.linkedin.com/in/yavuz-bahadir-sevim-dev/

* * *

⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın!
