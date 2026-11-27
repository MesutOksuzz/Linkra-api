# Mesut — Öğrenme profili ve çalışma notları

Bu dosya, Papara .NET bootcamp'i (OTS projesi, Kasım 2026, eğitim tamamlandı) boyunca Claude ile yapılan çalışmanın özetidir.
Referans proje: `MesutOksuzz/pk-ots` (dal `dev`). Çözüm üç projeden oluşur: `Ots.Api` (controller, domain, DbContext,
CQRS handler'ları, repository/UoW, middleware), `Ots.Base` (BaseEntity, ApiResponse, şifreleme, ortak servisler),
`Ots.Schema` (Request/Response DTO'ları).
Yeni bir projede Claude'un nereden devam edeceğini bilmesi için yazıldı. Yeni projenin repo köküne
`CLAUDE.md` adıyla konursa her oturumda otomatik okunur.

## Nasıl çalışmayı tercih ediyorum

- **Dil:** Türkçe. Teknik terimler İngilizce kalabilir, ilk geçtiğinde kısaca açıklanmalı.
- **Kodu ben yazarım.** Claude adım adım nereye ne açacağımı, hangi sırayla ve **neden** o sırayla
  yapıldığını anlatır. Hazır kodu verirken neyin neden öyle olduğunu da söyler.
- **Önce test, sonra öneri.** Claude kodu/komutu bana vermeden önce kendi ortamında derleyip mümkünse
  çalıştırarak doğrular. Tahmin ettiği yerleri açıkça "tahmin" diye işaretler.
- **Kısa ve net.** Uzun anlatım yerine tablo, adım listesi, kopyalanabilir kod blokları. Her komut
  ayrı blokta.
- **Benden habersiz değişiklik yok.** Bir şey kurmadan/değiştirmeden önce ne yapacağını ve nedenini
  söyler. Commit, push, force push benim onayımla.
- Her adımdan sonra ben push ederim, Claude GitHub'dan çekip dosya dosya kontrol eder.
- Takıldığımda ekran görüntüsü atarım; Claude hatanın kök sebebini açıklar, sadece belirtiyi değil.

## Ortamım

- **İşletim sistemi:** Windows (iş bilgisayarı + ev bilgisayarı; ikisi arasında GitHub ile geçiş).
  İş bilgisayarı şirket bilgisayarı: kurulumlar için bazen IT gerekiyor (BIOS sanallaştırma IT'den açıldı).
- **IDE:** JetBrains Rider (ana editör). Tuş haritası **Visual Studio**: yeniden adlandırma F2 /
  Ctrl+R,R; bul-değiştir Ctrl+H; dosyalarda bul-değiştir Ctrl+Shift+H; commit Ctrl+K; push Ctrl+Shift+K;
  pull Ctrl+T.
- **Yapay zeka:** VS Code'da GitHub Copilot (Agent modu) kullandım. Rider ve VS Code aynı projede
  **aynı anda açık olmamalı** (bir kez iç içe `Ots.Api\Ots.Api` klasörü oluştu).
- **Docker Desktop** (hesapsız). Konteynerler: `ots-mssql` (SQL Server 2022, 1433), `ots-redis`
  (Redis 7, 6379). Postgres projeden kaldırıldı.
- **DBeaver** (SQL Server bağlantısı, "Show all databases" ve "Trust Server Certificate" açık).
- **.NET 10 SDK**, `dotnet-ef` 10.0.14.

## Öğrendiklerim (OTS projesinde uygulayarak)

Seviye ölçeği: **Temel** = gördüm, yardımla yaparım · **Orta** = kendim yaparım, takıldığım yerler olur ·
**İyi** = kendim yaparım, hatayı da kendim bulurum.

### Özet tablo

| Konu | Seviye |
|---|---|
| Web API temelleri ve Swagger | İyi |
| EF Core code-first, DbContext, configuration | İyi |
| Migration yönetimi | İyi |
| İlişkiler, silme davranışları, Include | İyi |
| CQRS + MediatR | İyi |
| DTO (Request/Response) + AutoMapper | İyi |
| FluentValidation | Orta-İyi |
| ApiResponse, soft delete, BaseEntity | İyi |
| JWT kimlik doğrulama ve yetkilendirme | Orta |
| Şifre hash'leme | Temel |
| Middleware | Orta |
| DI ve servis ömürleri | Orta-İyi |
| ChangeTracker ile audit, IAppSession | Orta |
| Generic Repository + Unit of Work, transaction | Orta |
| Filtreler | Orta |
| Serilog | Orta |
| Cache (Response / Memory / Redis) | Orta |
| LINQ, IEnumerable/IQueryable, LinqKit | Orta |
| Git ve GitHub | Orta |
| Docker | Orta |
| Hata ayıklama (Rider debugger, log okuma) | Orta |
| Yapay zeka ile kod üretme (Copilot Agent) | Orta |

### Ayrıntılar

**1. Web API temelleri ve Swagger** — *İyi*
- Yaptıklarım: `ControllerBase`, `[ApiController]`, `[Route("api/[controller]")]`, `[HttpGet/Post/Put/Delete]`;
  `[FromRoute]`, `[FromQuery]`, `[FromBody]` ile parametre alma; Startup tabanlı yapı (`Program` + `Startup`).
- Bildiklerim: `[ApiController]`'ın otomatik model doğrulaması (400) yaptığını; Swagger örnek gövdesinin
  gerçek veriyle değiştirilmesi gerektiğini; `launchSettings.json` ile port ayarını.
- Dikkat: Nullable açıkken `[ApiController]` boş olmayan alanları zorunlu sayıyor.

**2. EF Core code-first** — *İyi*
- Yaptıklarım: Entity'ler (`BaseEntity`'den türeyen), `IEntityTypeConfiguration<T>` ile Fluent API
  (`HasKey`, `UseIdentityColumn`, `IsRequired`, `HasMaxLength`, `HasPrecision`, `HasDefaultValue`, `HasIndex(...).IsUnique()`),
  `[Table("X", Schema = "dbo")]`, `ApplyConfigurationsFromAssembly`, `AddDbContext` ile DI kaydı, connection string.
- Bildiklerim: `DbSet` yazmadan `context.Set<T>()` ile çalışılabildiğini; configuration'ın sınıftaki nullable
  işaretinden üstün geldiğini; iki farklı DB sağlayıcısı (SQL Server, PostgreSQL) için ayrı context'ler kurmayı ve sonra birini kaldırmayı.
- Dikkat: Klasör adı `DbContext` olunca base sınıf tam adla yazılmalı (`Microsoft.EntityFrameworkCore.DbContext`).
  Npgsql'de `DateTime` UTC olmalı (ya da legacy timestamp anahtarı).

**3. Migration yönetimi** — *İyi*
- Yaptıklarım: `migrations add`, `database update`, `migrations list` (Pending kontrolü), `migrations remove`,
  belirli bir migration'a geri dönme (`database update <Ad>`, `Down()` çalışır), `migrations script` ile SQL'i görme,
  "Kontrol" migration'ı alıp boş çıkmasıyla model-DB uyumunu doğrulama, kolon adlarını veri kaybetmeden değiştirme.
- Bildiklerim: Snapshot'ın rolü; NOT NULL kolon eklerken `defaultValue`; EF'nin yeniden adlandırmayı "sil + ekle" sanabileceği.
- Dikkat: Yeniden adlandırma içeren migration'ın `Up()` metodu mutlaka okunmalı (`RenameColumn` elle yazıldı).
  `dotnet ef` `.csproj`'un olduğu klasörde ya da `--project` ile çalışır. Uygulanmış migration düzenlenmez.

**4. İlişkiler ve Include** — *İyi*
- Yaptıklarım: Bire-çok (`HasMany/WithOne/HasForeignKey`), aynı tabloya iki FK (`FromAccount`/`ToAccount`),
  `OnDelete(Cascade/Restrict)`, navigation property'ler, `Include`/`ThenInclude` (lambda ve string: `"Account.Customer"`).
- Bildiklerim: SQL Server'ın "multiple cascade paths" kısıtı; finansal kayıtlarda Restrict; navigation property'nin DB'de kolon olmadığı.
- Dikkat: Include unutulursa hata vermez, response alanları sessizce `null` gelir.

**5. CQRS + MediatR** — *İyi*
- Yaptıklarım: `record` ile Query/Command mesajları (`IRequest<T>`), `IRequestHandler<,>` ile query ve command
  handler'ları, controller'da sadece `mediator.Send`, `RegisterServicesFromAssembly`.
- Bildiklerim: Query = okuma (SELECT), Command = durum değiştirme; mesaj tipi ile handler dönüş tipinin birebir aynı olması gerektiği.
- Dikkat: MediatR 13+ ücretli; 12.5.0 kullanıldı. Handler yazılmamış bir mesaj "No service for IRequestHandler" verir.

**6. DTO + AutoMapper** — *İyi*
- Yaptıklarım: Ayrı `Ots.Schema` projesinde `XRequest : BaseRequest`, `XResponse : BaseResponse`;
  `CreateMap<Request, Entity>()`, `CreateMap<Entity, Response>()`, `ForMember(... MapFrom(...))` ile türetilmiş alanlar
  (`CustomerName = FirstName + " " + LastName`, iki seviye öteden `Account.Customer`).
- Bildiklerim: Request'te sistemin ürettiği alanların (Id, IBAN, bakiye, tarihler) olmaması; URL'den gelen id'nin
  command'a ayrı parametre olarak taşınması (`ByCustomerId/{CustomerId}`); eşlemesi olmayan liste alanının çalışma anında hata verdiği.
- Dikkat: `ForMember`'da `dest.` hedefini kopyalarken kaydırmak (aynı alana iki kez eşleme).

**7. FluentValidation** — *Orta-İyi*
- Yaptıklarım: `AbstractValidator<XRequest>`, `RuleFor` ile `NotEmpty`, `Length`, `MinimumLength/MaximumLength`,
  `GreaterThan`, `Matches`, `EmailAddress`, `When(...)`, alanlar arası `NotEqual(x => x.FromAccountId)`, `WithMessage`;
  `AddFluentValidation` + `RegisterValidatorsFromAssemblyContaining`.
- Dikkat: Validator DB kolon sınırlarından gevşek olmamalı (yoksa 500). Regex'te `^...$` (yoksa kısmi eşleşir).
  `bool` için `NotEmpty` yazılmaz (`false` reddedilir). `When`'in ikinci parametresi `() => { ... }`.

**8. ApiResponse, soft delete, BaseEntity** — *İyi*
- `ApiResponse` (veri yok) ve `ApiResponse<T>` (veri var); mesaj verilen constructor'ın `Success = false` yaptığı
  (bu yüzden başarıda parametresiz `new ApiResponse()`).
- Silme yerine `IsActive = false`; hesap kapatmada `CloseDate`; "not found" / "not active" kontrolleri.

**9. JWT** — *Orta*
- Yaptıklarım: `JwtConfig` (Secret, Issuer, Audience, süre) appsettings'ten; `TokenService` ile claim'li token üretimi;
  `AddAuthentication().AddJwtBearer(...)`; `UseAuthentication` → `UseAuthorization` sırası; `[Authorize]`,
  `[Authorize(Roles = "admin,user")]`, `[AllowAnonymous]`; Swagger'da Bearer şeması ve Authorize butonu
  (Swashbuckle 10 / OpenApi 2.x yazımı).
- Bildiklerim: Token imzalı ama şifreli değil (jwt.io'da okunur, içine gizli bilgi konmaz); Secret en az 256 bit olmalı;
  401 (kim olduğu bilinmiyor) ile 403 (yetkisi yok) farkı.
- Dikkat: Token ucu `[AllowAnonymous]` olmazsa hiç token alınamaz. Swagger'a sadece token yapıştırılır ("Bearer" yazılmaz).
  Bütün uçlar kilitliyken ilk kullanıcı sorunu (seed admin ihtiyacı).

**10. Şifre hash'leme** — *Temel*
- MD5 + salt (`Secret`) ile hash; girişte aynı salt ile yeniden hash'leyip karşılaştırma; hash'in geri çevrilemediği.
- Dikkat: MD5 sadece ders için. Debugger'da `GeneratePassword(...)` ifadesinin üzerine gelmek metodu yeniden çalıştırır.

**11. Middleware** — *Orta*
- Yaptıklarım: `HeartBeatMiddleware`, `ErrorHandlerMiddleware` (try/catch ile 500), `RequestLoggingMiddleware`
  (RecyclableMemoryStream ile istek/cevap gövdesini okuyup loglama, `EnableBuffering`), `app.Use` / `app.Run`,
  constructor'ında `Action<T>` alan middleware'i `UseMiddleware<T>(arg)` ile kaydetme.
- Bildiklerim: Middleware sırasının önemi; middleware'in filtrelerin dışında olduğu.
- Dikkat: Ters koşul (`!StartsWithSegments`) bütün API'yi kapatmıştı. Hata cevabını geçici stream'e yazan middleware
  boş 500 döndürüyor.

**12. DI ve servis ömürleri** — *Orta-İyi*
- Singleton / Scoped / Transient farkını sayaç örneğiyle gördüm; `AddScoped<IAccountService, AccountService>()`,
  factory ile kayıt (`AddScoped<IAppSession>(provider => ...)`), "Unable to resolve service" hatasının sebebi.
- Dikkat: Scoped bir servisi (DbContext) kullanan servis Singleton olamaz. Aynı istekte scoped DbContext paylaşılır.

**13. ChangeTracker ile audit, IAppSession** — *Orta*
- `SaveChangesAsync` override ederek Added/Modified/Deleted kayıtlarda Inserted/Updated alanlarını otomatik doldurma,
  Deleted'ı soft delete'e çevirme, `AuditLog` tablosuna eski/yeni değerleri yazma; token claim'lerinden
  `AppSession` doldurma (`JwtManager.GetSession`), `IHttpContextAccessor`.
- Dikkat: `virtual` değil `override`; `AddHttpContextAccessor()`; `HttpContext == null` kontrolü (yoksa `dotnet ef` çöker);
  DbContext'e `IAppSession` constructor'dan verilmeli (yoksa kullanıcı hep "anonymous").

**14. Generic Repository + Unit of Work, transaction** — *Orta*
- `IGenericRepository<T>` (GetAllAsync(predicate, includes), GetByIdAsync(id, includes), AddAsync, Update, Delete),
  `UnitOfWork` içinde repository'ler ve `Complete()` (`BeginTransactionAsync` → `SaveChangesAsync` → `CommitAsync`,
  hata olursa `RollbackAsync` + log). Handler'ları DbContext'ten UnitOfWork'e taşıma (Copilot ile).
- Dikkat: `FindAsync` Include'ları yok sayar, `FirstOrDefaultAsync` kullanılmalı. Bir akışta birden fazla
  `Complete()`/servis kaydı varsa transaction bütünlüğü bozulabilir (EFT/transfer).

**15. Filtreler** — *Orta*
- Authorization, Resource, Action, Exception, Result filtreleri; `[TypeFilter(typeof(...))]` ile bağlama;
  çalışma sırası (Authorization → Resource → [model binding] → Action → metot → Exception → Result → Resource).
- Bildiklerim: Authorization reddederse diğerleri çalışmaz; validasyon hatasında Action filtresine gelinmez.

**16. Serilog** — *Orta*
- `appsettings.json`'dan yapılandırma (`ReadFrom.Configuration`), `UseSerilog()`, konsol ve günlük dönen dosya,
  MinimumLevel/Override, `Log.Information/Error`.
- Dikkat: Enricher'lar (`WithMachineName` vb.) ayrı paket ister; `formatter` ve `outputTemplate` birlikte kullanılmaz.

**17. Cache** — *Orta*
- Yaptıklarım (Country örneği):
  - **Response cache:** `AddResponseCaching()` + `UseResponseCaching()`, `[ResponseCache(Duration=..., Location=...)]`
    ve Startup'ta `CacheProfiles.Add("Default45", ...)` ile `[ResponseCache(CacheProfileName = "Default45")]`.
  - **Memory cache:** `AddMemoryCache()`, `IMemoryCache.TryGetValue` / `Set` / `Remove`, `MemoryCacheEntryOptions`
    (`SlidingExpiration`, `AbsoluteExpiration`).
  - **Redis (distributed):** `Microsoft.Extensions.Caching.StackExchangeRedis`, `AddStackExchangeRedisCache` ile
    `ConfigurationOptions` (Host/Port/InstanceName appsettings'teki `Redis` bölümünden), `IDistributedCache.GetAsync/SetAsync/Remove`,
    nesneyi `JsonConvert.SerializeObject` → `Encoding.UTF8.GetBytes` ile byte[] olarak saklayıp geri çevirme.
  - Akış: önce cache'e bak, yoksa DB'den oku, cache'e yaz, dön. Create/Update/Delete sonrası cache'i silip yeniden doldurma
    (cache invalidation). Aynı sorguyu `Get/Memory` ve `Get/Redis` uçlarıyla iki yöntemle sunma.
- Bildiklerim: Response cache tarayıcı/proxy tarafında, header ile çalışır (sunucu kodu çalışmayabilir). Memory cache uygulama
  yeniden başlayınca ve her sunucuda ayrı; Redis ayrı bir süreçte, uygulama kapansa da durur ve sunucular arasında paylaşılır.
  Redis kapalıysa `IDistributedCache` çağrısı hata (500) verir. Kontrol: `docker exec -it ots-redis redis-cli keys "*"`.
- Dikkat: Hoca invalidation metodunda `GetAllAsync().Result` kullandı (async'i bloklamak); kendi kodumda `await` tercih edilmeli.

**18. LINQ** — *Orta*
- `List` vs `IEnumerable` vs `IQueryable`; ertelenmiş çalışma (deferred execution); `ToListAsync` ile sorgunun o an
  çalışması; `Where`, `FirstOrDefault`, `Aggregate` ile Include zinciri; LinqKit `PredicateBuilder` ile dinamik filtre.

**19. Git ve GitHub** — *Orta*
- clone, branch (`dev`), commit/push farkı, Rider'da Commit and Push, pull, `git mv` ile klasör taşıma,
  commit mesajını düzeltme (Edit Commit Message + force push), amend, push reddinde Rebase/Merge, `git restore`, `.gitignore`.
- Dikkat: Push edilmiş commit'i amend etme. Üstünde commit olan bir commit'in mesajını değiştirmek geçmişi yeniden yazar.
  İki bilgisayar arasında: işe başlarken pull, bitirirken push. "Create PR"a gerekmedikçe basma.

**20. Docker** — *Orta*
- `run` (`--name`, `-e`, `-p`, `-v`, `-d`), `start/stop`, `ps`, `logs`, `exec`, `inspect`; volume ile kalıcı veri;
  SQL Server (şifre kuralları), PostgreSQL, Redis konteynerleri; BIOS sanallaştırma gereksinimi.
- Dikkat: `--name` seçeneğinin kendisi değiştirilmez, sadece değeri. Port eşlemesi olmadan uygulama konteynere ulaşamaz.

**21. Hata ayıklama** — *Orta*
- Rider debugger (F9 breakpoint, F10 satır atla, F11 içine gir, Shift+F11 çık, F5 devam), Variables paneli,
  breakpoint ile istek yolculuğunu takip etme; Run penceresinde ve `logs/log-YYYYMMDD.log` dosyasında gerçek hata mesajını bulma;
  "Build failed" çıktısında asıl hatayı süzme.
- Dikkat: Sarı ok **henüz çalışmamış** satırı gösterir; o satırdaki değişken `null` görünür, F10 sonrası dolar.

**22. Yapay zeka ile kod üretme** — *Orta*
- VS Code Copilot Agent ile örnek dosyaları bağlama ekleyip "şu kalıbı izle" diye refactor ettirme;
  promptta açık kurallar ve "tek seferde tek dosya"; diff'i okuyup derleyip test etme.
- Dikkat: Kısa prompt beklenmedik kapsam getirir ("all query classes"); üretilen kodu kontrol etmeden kabul etme.

## Henüz eksik / sırada

- **Clean Architecture'ın kendisi:** OTS tek projede (Ots.Api) toplandı, katmanlar ayrılmadı.
  Domain EF'yi biliyordu, handler'lar DbContext'e bağımlıydı. Yeni projede Domain / Application /
  Infrastructure / Api ayrımı ve **bağımlılık kuralı** baştan doğru kurulmalı.
- Birim testleri (xUnit, mock) hiç yazılmadı.
- Transaction'ların birden fazla `SaveChanges` arasında tutarlılığı (EFT/transfer akışı) tam oturmadı.
- Güvenlik: secret'ların repoya konmaması (user-secrets / ortam değişkenleri).

## Sık yaptığım hatalar (Claude'un erkenden kontrol etmesi gerekenler)

- **İsim/harf uyuşmazlıkları:** `x =` yerine `x =>`, `Services` / `Service`, `appsetting.json`,
  sınıf adı ≠ constructor adı, record adı ≠ handler'da kullanılan ad.
- Rider'da **Add → Class** sonrası kodu sınıfın içine yapıştırmak (sarmalayıcı sınıf kalıyor;
  Alt+Enter "CustomerPhone." ön eki öneriyorsa sebep bu).
- Kopyala-yapıştırda satır kayması: yanlış `ForMember` hedefi, eksik `namespace`, silinmesi gerekmeyen
  satırların da silinmesi.
- Swagger'ın örnek gövdesini değiştirmeden göndermek: veritabanı `"string"` ile doluyor ya da `HasMaxLength(3)` gibi
  sınırları aşıp 500 veriyor (Country `IsoCode`/`PhoneCode`); benzersiz alanda ikinci denemede yine 500.
- Docker kapalıyken çalıştırmak (boş 500). Önce `docker start ...`.
- Terminali yanlış klasörde çalıştırmak (`dotnet ef` → `.csproj`'un olduğu klasör ya da `--project`).
- Test için geçici yapılanları geri almayı unutmak (yorum satırına alınmış `[Authorize]`, sabit `"123456"` şifre).
- Debugger'da çalışmamış satırın değişkenini `null` görüp hata sanmak.

## Hocanın kodundan bilinçli farklarımız (OTS)

Hocanın kodu .NET 8 ve eski paketlerle yazıldığı için ya da hatalı olduğu için şu farkları bilerek yaptık.
Yeni projede hocanınki örnek alınırsa bunlar yine geçerli:

| Konu | Hocada | Bizde | Neden |
|---|---|---|---|
| Token ucu | `[Authorize]` altında kalıyordu | `[AllowAnonymous]` | Yoksa hiç token alınamıyor (401) |
| AuditLog kolonları | Eksik | Ayrı migration (`AuditLogValues`/`Audit2`) | Kayıt sırasında hata; hoca kendi migration'ını eklerse tekrar eklenmez |
| `IHttpContextAccessor` | Kayıtlı değil | `services.AddHttpContextAccessor()` | `IAppSession` factory'si çözülemiyordu |
| `IAppSession` factory | `HttpContext` kontrolü yok | `if (httpContextAccessor?.HttpContext == null) return new AppSession();` | `dotnet ef` ve arka plan işlerinde çöküyordu |
| `OtsDbContext` constructor | `IAppSession` almıyordu | `(DbContextOptions<OtsDbContext> options, IAppSession appSession)` | Audit kullanıcısı hep boş kalıyordu |
| Swagger Bearer | `OpenApiReference` | `OpenApiSecuritySchemeReference` (OpenApi 2.x) | Swashbuckle 10 ile eski yazım derlenmiyor |
| EFT/Transfer | `InsertedUser = null`, başarıda hata mesajlı `ApiResponse` | Düzeltildi | 500 ve "para düştü ama karşıya geçmedi" hatası |
| RequestLogging | `RequestProfilerModel` erişimi/tipi | `public`, `DateTimeOffset`, `using Serilog` | Derlenmiyordu |
| Paketler | .NET 8 sürümleri | .NET 10 sürümleri (EF, JwtBearer, Redis 10.0.14) | Bkz. sürüm kararları |

## OTS'de açık kalanlar (bilinçli ertelenen)

- `OtsDbContext.SaveChangesAsync` `public virtual` yazıldı; `public override` olmalı. Şu an `OtsDbContext` tipinden çağrıldığı için
  çalışıyor, ama base `DbContext` üzerinden çağrılırsa audit atlanır.
- `UserCommandHandler` Create'te şifre geçici olarak `"123456"` (asıl hali `PasswordGenerator.GeneratePassword(6)`).
- `CountriesController`'da `[Authorize]` satırları test için yorumda.
- Country Update sadece `Name` güncelliyor; `CountryRequest` için validator yok (uzunluk aşımı 500 veriyor).
- AutoMapper 12.0.0 ve bazı alt paketler güvenlik uyarısı veriyor (ders için kabul edildi).

## Sürüm kararları (.NET 10)

- Microsoft.* paketleri .NET sürümünü takip eder: EF Core 10.0.14, JwtBearer 10.0.14,
  Caching.StackExchangeRedis 10.0.14. (Hoca .NET 8 kullanıyordu; sürümleri ona göre değil, .NET 10'a göre seç.)
- MediatR 12.5.0, AutoMapper 12.0.0 (eski ama ücretsiz; güvenlik uyarısı bilinçli kabul edildi),
  FluentValidation.AspNetCore 9.5.2 (hocanınkiyle aynı; `AddFluentValidation` için).
- Swashbuckle 10 → `Microsoft.OpenApi` 2.x yazımı (`OpenApiSecuritySchemeReference`).
- LinqKit: OTS'de `LinqKit` 1.3.2 kurulu. Yeni projede EF Core ile `LinqKit.Core` (sadece PredicateBuilder) ya da
  `LinqKit.Microsoft.EntityFrameworkCore` tercih edilmeli; düz `LinqKit` EF6'ya yöneliktir.
- Serilog: `Serilog.AspNetCore` 10.0.0, `Serilog.Sinks.File` 7.0.0 (+ eski `RollingFile` 3.3.0) ve `ReadFrom.Configuration`.
  appsettings'teki `WithMachineName/WithProcessId/WithThreadId` enricher'larının paketleri kurulu değil (sessizce yok sayılıyor).
- RecyclableMemoryStream 2.3.2 (hocanınkiyle aynı), Swashbuckle.AspNetCore 10.3.1.
- Redis için `Microsoft.Extensions.Caching.StackExchangeRedis` 10.0.14 (hoca 7.0.17 kullandı; .NET sürümüne göre seçildi).

## Yeni projede Claude'dan beklentim

1. Önce projeyi okuyup kısa bir yol haritası çıkar (OTS'deki A/B planı gibi), sonra adım adım ilerle.
2. Her adımda: ne yapılacak, hangi dosya, neden bu sıra, kopyalanabilir kod, ardından test yöntemi.
3. Bildiğim konuları yeniden uzun uzun anlatma; yeni kavramları (özellikle Clean Architecture, test)
   OTS'deki karşılığıyla bağlantı kurarak anlat.
4. Kodu vermeden önce derle/çalıştır; emin olmadığın yeri söyle.
