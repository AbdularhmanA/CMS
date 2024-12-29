using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CMSApp.Data;
using CMSApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ≈⁄œ«œ«  ﬁ«⁄œ… «·»Ì«‰« 
builder.Services.AddDbContext<CMSDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ≈⁄œ«œ«  «·„’«œﬁ… »«” Œœ«„ ÂÊÌ… «·„” Œœ„
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<CMSDBContext>()
.AddDefaultTokenProviders();

// ≈÷«›… Œœ„«  «·„’«œﬁ… „⁄ „·›«   ⁄—Ì› «·«— »«ÿ
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; //  ÕœÌœ „”«— ’›Õ…  ”ÃÌ· «·œŒÊ·
    });

builder.Services.AddAuthorization(); //  ›⁄Ì· «· ›ÊÌ÷

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ≈÷«›… «·„’«œﬁ…
app.UseAuthentication(); // ≈÷«›… Â–« «·”ÿ—

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
