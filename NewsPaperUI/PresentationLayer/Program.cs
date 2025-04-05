using PresentationLayer.ApiServices;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
       .AddJsonOptions(options =>
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddSession();// Session, uygulama genelinde kullanýma açýlýyor
builder.Services.AddHttpContextAccessor();

//HTTPCLIENT ILE API KABERLESMESI ICIN GECERLI
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiService, ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseSession(); // Session, uygulama genelinde kullanýma açýlýyor
app.UseAuthorization();

app.MapStaticAssets();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "adminPanelDefault",
    areaName: "AdminPanel",
    pattern: "/admin/{controller=Authentication}/{action=LogIn}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
