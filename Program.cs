using HospitalSanVicente.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.OpenConnection();
        Console.WriteLine("Base de datos conectada ✅");
        context.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error de conexión a la base de datos:");
        Console.WriteLine(ex.Message);          // Mensaje corto
        Console.WriteLine(ex.StackTrace);       // Traza completa (útil para depuración)
        if (ex.InnerException != null)
            Console.WriteLine($"Detalle: {ex.InnerException.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
