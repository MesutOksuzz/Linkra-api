using Linkra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Linkra.Infrastructure;

public static class DependencyInjection   //Extension method yazabilmek için sınıfın static olması gerekir. Sınıf adının bir önemi yok, CA projelerinde bu ad gelenek haline geldi.
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
/*this IServiceCollection services: Metodu extension method yapan şey baştaki this. Bu sayede builder.Services.AddInfrastructure(...) diye, sanki IServiceCollection'ın kendi metoduymuş gibi çağırabiliyorsun.
 AddControllers() ve AddDbContext() da aynı yöntemle yazılmış metotlar.
IConfiguration configuration: Connection string'i okumak için. Program.cs'teki builder.Configuration buraya geliyor. appsettings.json'ı ve user-secrets'ı birlikte okuyor, OTS'deki Configuration ile aynı nesne.
Dönüş tipi IServiceCollection: Aynı nesneyi geri veriyor. Böylece istersen .AddInfrastructure(...).AddApplication() diye zincirleyebilirsin.*/    
    {
        var connectionString = configuration.GetConnectionString("LinkraDb");  //ConnectionStrings:LinkraDb anahtarını okur. Önce appsettings.json'a bakar (boş string), sonra user-secrets'a bakar ve onun değeri üste yazılır.
        if (string.IsNullOrWhiteSpace(connectionString)) //Ev bilgisayarında user-secrets'ı tanımlamayı unutursan, uygulama açılırken ne eksik olduğunu söyleyerek durur. Bu kontrol olmasa, ilk istekte anlaşılmaz bir Npgsql hatası alırdın.
            throw new InvalidOperationException("Connection string 'LinkraDb' bulunamadı. user-secrets ile tanımlayın.");

        services.AddDbContext<LinkraDbContext>(options => options.UseNpgsql(connectionString)); //OTS'deki satırın aynısı. Sadece UseSqlServer yerine UseNpgsql var. DbContext'i Scoped olarak kaydeder (profilindeki DI ömürleri notu)

        return services; //Zincirleme için aynı nesneyi geri verir.
    }
}