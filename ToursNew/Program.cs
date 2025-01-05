using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToursNew.Data;
using ToursNew.Models;
using ToursNew.Repository;
using ToursNew.Services;
using ToursNew.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToursContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ToursContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddHttpClient();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

//repo
builder.Services.AddScoped<ITripRepository, TripRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

//services
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//validators
builder.Services.AddScoped<IValidator<Client>, ClientValidator>();

builder.Services.AddScoped<IValidator<Reservation>, ReservationValidator>();

builder.Services.AddScoped<IValidator<Trip>, TripValidator>();

//logging part
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//activity logger
builder.Services.AddScoped<IActivityLogger, ActivityLogger>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ToursContext>();
        DBInitializer.Initialize(context);

        //rolemanager 
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "Manager", "User", "Tester" };

        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var useradmin = "admin@gmail.com";
        var usermanager = "manager@gmail.com";
        var userdefault = "user@gmail.com";
        var usertester = "tester@gmail.com";
            
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        var user = await userManager.FindByEmailAsync(useradmin);
        if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
            await userManager.AddToRoleAsync(user, "Admin");

        user = await userManager.FindByEmailAsync(usermanager);
        if (user != null && !await userManager.IsInRoleAsync(user, "Manager"))
            await userManager.AddToRoleAsync(user, "Manager");

        user = await userManager.FindByEmailAsync(userdefault);
        if (user != null && !await userManager.IsInRoleAsync(user, "User"))
            await userManager.AddToRoleAsync(user, "User");
        
        user = await userManager.FindByEmailAsync(usertester);
        if(user != null && !await userManager.IsInRoleAsync(user, "Tester"))
            await userManager.AddToRoleAsync(user, "Tester");
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
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();