using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SQL_INJ_API;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<PoliticiansDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Obs�uga b��d�w
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://0.0.0.0:4000");
                webBuilder.UseStartup<Program>();
                webBuilder.ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<PoliticiansDbContext>(options =>
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("DefaultConnection2")));
                    services.AddScoped<DbContext, PoliticiansDbContext>();
                    services.AddControllers();
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.AllowAnyOrigin() // Dodaj adresy domen, kt�re maj� dost�p do API
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                    });
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    
                    app.UseCors();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers(); // Mapowanie kontroler�w
                    });
                });
            });
}


