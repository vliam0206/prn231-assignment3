using BusinessObjects.AutoMapper;
using CarRentingWebClient.AccessAPIs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using CarRentingWebClient.AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DIs
builder.Services.AddScoped<IAuthAPIs, AuthAPIs>();
builder.Services.AddScoped<ICarInformationAPIs, CarInformationAPIs>();
builder.Services.AddScoped<ICustomerAPIs, CustomerAPIs>();
builder.Services.AddScoped<IManufacturerAPIs, ManufacturerAPIs>();
builder.Services.AddScoped<IRentingDetailAPIs, RentingDetailAPIs>();
builder.Services.AddScoped<IRentingTransactionAPIs, RentingTransactionAPIs>();
builder.Services.AddScoped<ISupplierAPIs, SupplierAPIs>();

// Add auto maper
builder.Services.AddAutoMapper(typeof(MappingModelsProfile));
builder.Services.AddAutoMapper(typeof(MappingDTOsProfile));

// Add session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(opt => {          
    opt.Cookie.Name = "rentingcookies";
    opt.IdleTimeout = TimeSpan.FromMinutes(60);
});

// Add authentication
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Home/Login";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        opt.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
