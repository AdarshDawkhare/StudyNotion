using Domain.Interface;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudyNotionServer.Module.Email;
using StudyNotionServer.ServiceLayer.Classes;
using StudyNotionServer.ServiceLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Get connectionString and DatabaseName from appsettings
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
string databaseName = builder.Configuration.GetValue<string>("ConnectionStrings:DatabaseName")!;

//Register DbContext with MongoDB provider
builder.Services.AddDbContext<StudyNotionDbContext>(options =>
    options.UseMongoDB(connectionString, databaseName)
);

// ✅ Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "StudyNotion.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // in dev you can use SameAsRequest
        options.Cookie.SameSite = SameSiteMode.Lax; // Lax is ok for same-site APIs. Use Strict if possible.
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(14);

        // API-friendly: return status codes instead of redirecting to HTML pages
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx => { ctx.Response.StatusCode = StatusCodes.Status401Unauthorized; return Task.CompletedTask; },
            OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = StatusCodes.Status403Forbidden; return Task.CompletedTask; }
        };
    });

// ✅ Authorization (policies optional; roles can be used via attributes)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("InstructorOnly", p => p.RequireRole("Instructor"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthS, AuthS>();
builder.Services.AddScoped<StudyNotionServer.Module.Email.IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Order matters:
app.UseAuthentication();
app.UseAuthorization();

// Enable CORS
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
