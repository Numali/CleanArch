using Application.StudentCRUD;
using Domain.StudentCRUD;
using Infrastructure.StudentCRUD;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

//AddAuthentication
builder.Services.AddAuthentication()

      .AddBearerToken(IdentityConstants.BearerScheme);

//Add Authorization
builder.Services.AddAuthorizationBuilder();


//Add Identity Services
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddRoles<IdentityRole>()
    .AddApiEndpoints();

//Configure DbContext
builder.Services.AddDbContext<ApplicationDBContext>();

//Add Identity Core services
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddApiEndpoints();   


builder.Services.AddScoped<IStudentService, StudentService>();
var app = builder.Build();

//seed roles into the database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //define roles
    string[] roles = { "Admin", "User" };

    // Seed roles if they don't exist
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            Console.WriteLine("Hello from inseid");
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
        Console.WriteLine("Hello");
    }



    app.MapIdentityApi<AppUser>();//to show Api



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
