using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uBuntuSouthAfrica.Areas.Identity.Data;
using uBuntuSouthAfrica.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("uBuntuSouthAfricaDbContextConnection") ?? throw new InvalidOperationException("Connection string 'uBuntuSouthAfricaDbContextConnection' not found.");

//builder.Services.AddDbContext<uBuntuSouthAfricaDbContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDbContextPool<uBuntuSouthAfricaDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<uBuntuSouthAfricaUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<uBuntuSouthAfricaDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
