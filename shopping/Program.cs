using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using shopping.data.Facade;
using shopping.data.Repositories;
using shopping.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.                                              
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ShoppingDBcontext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConn")));
var connectionString = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<shoppingContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddIdentity<Member,Role>().AddEntityFrameworkStores<shoppingContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";

    });
builder.Services.AddScoped<IRepository<Member>,Repository<Member>>();
builder.Services.AddScoped<IRepository<Merchant>,Repository<Merchant>>();
builder.Services.AddScoped<IRepository<Prodect>,Repository<Prodect>>();
//
builder.Services.AddScoped<FProduct>();
//builder.Services.AddScoped<FCartItem>();

// add session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddTransient<FCartItem>();


//email service 
//builder. Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder. Services.AddScoped<IEmailService, EmailService>();

//builder.Services.AddControllersWithViews(options =>
//{
//    options.ModelMetadataDetailsProviders.Add(new DataAnnotationsMetadataProvider());
//});
//------------------------



//------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
           
        });
app.Run();
