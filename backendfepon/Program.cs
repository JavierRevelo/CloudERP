using backendfepon;
using backendfepon.Data;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


//Add Services Controllers
builder.Services.AddControllers();

// Add services of EF Core.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopConnection")));
builder.Services.AddAutoMapper(typeof(Program)); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration["Firebase:ServiceAccountKeyPath"])
});

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/clouderp-93d91";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/clouderp-93d91",
            ValidateAudience = true,
            ValidAudience = "clouderp-93d91",
            ValidateLifetime = true
        };
    });

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrganizationalOnly", policy => policy.RequireRole("Presidente", "Vicepresidente General"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAllOrigins"); // Enable CORS using the policy
app.UseAuthentication(); // Ensure Authentication middleware is added
app.UseAuthorization();
app.MapControllers();

app.Run();
