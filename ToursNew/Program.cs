using Microsoft.EntityFrameworkCore;
using ToursNew.Data;
using ToursNew.Repository;
using ToursNew.Services;
using FluentValidation;
using ToursNew.Models;
using ToursNew.Validators;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ToursContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ToursContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITripRepository, TripRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();  

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();    

builder.Services.AddScoped<IClientService, ClientService>();    

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IValidator<Client>, ClientValidator>();

builder.Services.AddScoped<IValidator<Reservation>, ReservationValidator>();

builder.Services.AddScoped<IValidator<Trip>, TripValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ToursContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during database creation.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
