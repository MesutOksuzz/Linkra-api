> Kaynak: `Linkra_Gereksinim_Dokumani_v1.0.docx` dosyasından pandoc ile Markdown'a çevrildi. Asıl belge .docx dosyasıdır.

# Jira Benzeri Proje Yönetim Sistemi

## Yazılım Sistem Gereksinimleri Raporu

**Doküman sürümü:** 1.0  
**Tarih:** 3 Kasım 2026  
**Proje türü:** Yazılım geliştirme ve eğitim projesi  
**Hedef süre:** 3–4 hafta  
**Geliştirme modeli:** Tam zamanlı, haftada 35–40 saat

## 1. Projenin Amacı

Bu projenin amacı, başlangıç seviyesindeki bir yazılım geliştiricinin C#
ve .NET becerilerini ilerletmesini, Clean Architecture yaklaşımını
uygulamasını ve Angular ile uçtan uca çalışan bir web uygulaması
geliştirmesini sağlamaktır.

Geliştirilecek uygulama Jira'dan esinlenen, yazılım ekiplerinin
projelerini ve görevlerini yönetebileceği bir sistem olacaktır.

Proje ticari amaç taşımamaktadır. Öncelik, kapsamlı bir ürün ortaya
çıkarmaktan çok geliştiricinin modern yazılım geliştirme süreçlerinde
deneyim kazanmasıdır.

## 2. Teknoloji Gereksinimleri

| Bileşen              | Teknoloji                 |
|----------------------|---------------------------|
| Backend              | C# / ASP.NET Core .NET 10 |
| Frontend             | Angular                   |
| Mimari               | Clean Architecture        |
| Veritabanı           | PostgreSQL                |
| ORM                  | Entity Framework Core     |
| Veritabanı yaklaşımı | Code First                |
| Migration            | EF Core Migrations        |
| Veritabanı ortamı    | Docker Compose            |
| Versiyon kontrolü    | Git                       |

Uygulama backend, frontend ve veritabanı bileşenlerinden oluşmalıdır.

## 3. Fonksiyonel Gereksinimler

### 3.1. Kullanıcı Yönetimi

**FR-001:** Kullanıcı e-posta ve şifreyle kayıt olabilmelidir.

**FR-002:** Kullanıcı e-posta ve şifreyle giriş yapabilmelidir.

**FR-003:** Kullanıcı mevcut oturumunu sonlandırabilmelidir.

**FR-004:** Kullanıcı kendi temel profil bilgilerini
görüntüleyebilmelidir.

**FR-005:** Yetkisiz kullanıcılar korunan uygulama ekranlarına
erişememelidir.

### 3.2. Workspace Yönetimi

**FR-006:** Kullanıcı Workspace oluşturabilmelidir.

**FR-007:** Workspace adı ve açıklaması tanımlanabilmelidir.

**FR-008:** Workspace oluşturan kullanıcı Owner olmalıdır.

**FR-009:** Workspace'e mevcut kullanıcılar eklenebilmelidir.

**FR-010:** Workspace üyeleri listelenebilmelidir.

**FR-011:** Workspace Owner üyeleri çıkarabilmelidir.

İlk sürümde sabit Owner ve Member rolleri yeterlidir.

### 3.3. Proje Yönetimi

**FR-012:** Yetkili kullanıcı proje oluşturabilmelidir.

**FR-013:** Her proje bir Workspace'e bağlı olmalıdır.

**FR-014:** Proje adı zorunlu olmalıdır.

**FR-015:** Proje için benzersiz bir kod oluşturulmalıdır.

**FR-016:** Proje açıklaması düzenlenebilmelidir.

**FR-017:** Projeler listelenebilmelidir.

**FR-018:** Yetkili kullanıcı mevcut projeleri düzenleyebilmelidir.

### 3.4. Görev Yönetimi

Sistem aşağıdaki görev türlerini desteklemelidir:

- Epic

- Story

- Task

- Bug

- Subtask

**FR-019:** Kullanıcı görev oluşturabilmelidir.

**FR-020:** Görev başlığı zorunlu olmalıdır.

**FR-021:** Görev açıklaması girilebilmelidir.

**FR-022:** Göreve öncelik atanabilmelidir.

**FR-023:** Görev bir kullanıcıya atanabilmelidir.

**FR-024:** Görev bilgileri düzenlenebilmelidir.

**FR-025:** Görevler proje bazında listelenebilmelidir.

**FR-026:** Her görev için benzersiz bir görev kodu üretilmelidir.

**FR-027:** Subtask geçerli bir üst göreve bağlı olmalıdır.

**FR-028:** Epic altında Story, Task ve Bug bulunabilmelidir.

### 3.5. Kanban Board

**FR-029:** Her proje bir Kanban Board'a sahip olmalıdır.

Başlangıçta üç sabit görev durumu kullanılacaktır:

1.  To Do

2.  In Progress

3.  Done

**FR-030:** Görevler durumlarına uygun Kanban sütunlarında
gösterilmelidir.

**FR-031:** Kullanıcı görevleri sürükle-bırak yöntemiyle sütunlar
arasında taşıyabilmelidir.

**FR-032:** Sütun değişikliği görevin durumunu güncellemelidir.

**FR-033:** Kanban üzerinden görev detayları açılabilmelidir.

**FR-034:** Kanban görevleri sorumlu, durum ve öncelik kriterlerine göre
filtrelenebilmelidir.

### 3.6. Sprint ve Backlog

**FR-035:** Her proje Backlog görünümüne sahip olmalıdır.

**FR-036:** Sprint'e atanmamış uygun görevler Backlog içerisinde
görüntülenebilmelidir.

**FR-037:** Yetkili kullanıcı Sprint oluşturabilmelidir.

**FR-038:** Sprint adı, başlangıç ve bitiş tarihi tanımlanabilmelidir.

**FR-039:** Backlog'daki görevler Sprint'e taşınabilmelidir.

**FR-040:** Aynı anda bir projede yalnızca bir Sprint aktif
olabilmelidir.

**FR-041:** Sprint başlatılabilmelidir.

**FR-042:** Aktif Sprint tamamlanabilmelidir.

**FR-043:** Sprint tamamlandığında bitmemiş görevler Backlog'a geri
aktarılabilmelidir.

**FR-044:** Sprint içerisindeki görevler listelenebilmelidir.

İlk sürümde Velocity, Burndown Chart ve gelişmiş Sprint raporları
geliştirilmeyecektir.

### 3.7. Arama ve Filtreleme

**FR-045:** Görevler başlığa göre aranabilmelidir.

**FR-046:** Görevler durumlarına göre filtrelenebilmelidir.

**FR-047:** Görevler sorumlu kullanıcıya göre filtrelenebilmelidir.

**FR-048:** Görevler önceliklerine göre filtrelenebilmelidir.

## 4. Fonksiyonel Olmayan Gereksinimler

### 4.1. Mimari

- Backend Clean Architecture prensiplerine göre düzenlenmelidir.

- İş kuralları uygun katmanlarda yer almalıdır.

- Dependency Injection kullanılmalıdır.

- Veritabanı işlemleri EF Core ile gerçekleştirilmelidir.

### 4.2. Güvenlik

- Kullanıcı parolaları güvenli biçimde saklanmalıdır.

- Korunan API işlemlerinde kimlik doğrulama yapılmalıdır.

- Yetkisiz kullanıcıların başka Workspace ve projelerin verilerine
  > erişmesi engellenmelidir.

- Kullanıcıya hassas sistem bilgileri gösterilmemelidir.

### 4.3. Hata Yönetimi

- API hataları anlaşılır ve tutarlı yanıtlar döndürmelidir.

- Geçersiz isteklerde uygun hata mesajı gösterilmelidir.

- Beklenmeyen hatalar uygulamanın tamamen durmasına neden olmamalıdır.

### 4.4. Kullanılabilirlik

- Angular arayüzü masaüstü kullanımına uygun olmalıdır.

- Kullanıcı temel proje ve görev işlemlerini kolayca
  > gerçekleştirebilmelidir.

- Form doğrulama hataları kullanıcıya gösterilmelidir.

- Yükleme ve işlem durumları anlaşılır biçimde sunulmalıdır.

## 5. Geliştirme Planı

| Hafta | Çalışmalar                                                                         | Beklenen teslim                                           |
|-------|------------------------------------------------------------------------------------|-----------------------------------------------------------|
| 1     | Proje kurulumu, Clean Architecture, PostgreSQL, EF Core, Angular, kimlik doğrulama | Çalışan backend ve frontend iskeleti, temel giriş sistemi |
| 2     | Workspace, proje ve görev yönetimi                                                 | Görev oluşturulabilen ve listelenebilen uygulama          |
| 3     | Kanban, görev durumları, atama ve filtreleme                                       | Çalışan Kanban arayüzü                                    |
| 4     | Sprint, Backlog, entegrasyon, hata düzeltmeleri, dokümantasyon                     | Çalışan ve sunuma hazır uygulama                          |

## 6. Teslim Edilecek Çıktılar

Geliştirici proje sonunda aşağıdaki çıktıları sağlamalıdır:

1.  Backend kaynak kodu

2.  Angular kaynak kodu

3.  PostgreSQL veritabanı migration dosyaları

4.  Docker Compose yapılandırması

5.  API dokümantasyonu

6.  README ve kurulum yönergeleri

7.  Çalışan uygulamanın gösterimi

Ayrı otomatik test geliştirme teslimi beklenmemektedir.

## 7. Tamamlanma ve Kabul Kriterleri

Proje aşağıdaki işlemler gerçekleştirilebildiğinde fonksiyonel açıdan
teslim edilebilir kabul edilecektir:

- Kullanıcı kayıt olup giriş yapabiliyor.

- Yeni Workspace oluşturulabiliyor.

- Workspace içerisinde proje oluşturulabiliyor.

- Projeye görev eklenebiliyor.

- Görevler güncellenebiliyor.

- Görev türleri kullanılabiliyor.

- Görev bir kullanıcıya atanabiliyor.

- Görevler Kanban sütunlarında görüntülenebiliyor.

- Görevler sütunlar arasında taşınabiliyor.

- Görevler filtrelenebiliyor.

- Sprint oluşturulup başlatılabiliyor.

- Backlog'dan Sprint'e görev taşınabiliyor.

- Sprint tamamlanabiliyor.

- Uygulama kurulum yönergeleri izlenerek yerel ortamda
  > çalıştırılabiliyor.

## 8. Kapsam Dışı Özellikler

Aşağıdaki özellikler 3–4 haftalık ilk teslimat kapsamında değildir:

- Dinamik rol ve izin yönetimi

- Özelleştirilebilir Workflow Engine

- Custom Fields

- Gelişmiş davet sistemi

- Dosya ekleri

- Yorum ve aktivite geçmişi

- Gelişmiş raporlama

- Velocity ve Burndown raporları

- Bildirim sistemi

- Otomasyon

- Gelişmiş audit kayıtları

- Otomatik Unit, Integration ve E2E test geliştirme

- Yüksek ölçekli performans optimizasyonları

- Kurumsal SSO ve iki aşamalı doğrulama

Bu özellikler ileride yapılabilecek geliştirmeler olarak
değerlendirilebilir.

## 9. Projenin Başarı Ölçütü

Projenin başarı ölçütü, Jira'nın bütün özelliklerini yeniden geliştirmek
değildir.

Başarı; geliştiricinin C#/.NET 10, Clean Architecture, EF Core,
PostgreSQL ve Angular kullanarak temel proje yönetimi işlevlerini yerine
getiren, çalışır durumda bir uygulamayı bağımsız olarak açıklayabilmesi
ve teslim edebilmesidir.
