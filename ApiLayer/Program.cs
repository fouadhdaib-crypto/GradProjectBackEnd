using businessLayer_GP;
using businessLayer_GP.Service;
using DataAccessLayer;
using GradProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------- SERVICES ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DataAccessLayer")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// CORS (مرة واحدة فقط)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Services
builder.Services.AddScoped<UserDataAccess>();
builder.Services.AddScoped<LoginBusinessLayer>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<RegisterBusinessLayer>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ClsNotificationDataAaccessLayer>();
builder.Services.AddScoped<ClsChatBusinesssLayer>();
builder.Services.AddScoped<ClschatDataAcssesLayer>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ClsProjectTeamsDataAaccess>();
builder.Services.AddScoped<ClsProjectTeamsbusinessLayer>();
builder.Services.AddScoped<ClsProfileBusinessLayer>();
builder.Services.AddScoped<ClsProfileDataAccsessLayer>();
builder.Services.AddHttpContextAccessor();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// ---------------- APP ----------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// ⚠️ ترتيب مهم جداً
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();