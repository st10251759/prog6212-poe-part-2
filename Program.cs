using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adding DB Context builder services with options with roles
builder.Services.AddDbContext<Prog6212DbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("Prog6212DEV")));

//Added service for Authorization for Role based Access
builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<Prog6212DbContext>();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
