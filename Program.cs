using IdentityApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityApp.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionsString = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionsString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;

});

builder.Services.AddAuthorization(options =>
{
   options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddScoped<IAuthorizationHandler, InvoiceCreatorAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceManagerAuthorization>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceAdminAuthorization>();
var app = builder.Build();


using(var scop = app.Services.CreateScope())
{
    var services = scop.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var seedUserPass = builder.Configuration.GetValue<string>("SeedUserPass");
    await SeedData.initialize(services, seedUserPass);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();


app.MapRazorPages();

app.Run();
