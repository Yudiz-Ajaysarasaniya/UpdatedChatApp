using ChatApp.notify;
using ChatApp.notify.Implementation;
using ChatApp.notify.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UpdatedChatApp.AppContext;
using UpdatedChatApp.Helper;
using UpdatedChatApp.Interfaces;
using UpdatedChatApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// 1. Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

// 2. Configuration for Email Settings
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));

// 3. Register Services
builder.Services.AddTransient<IMessages, Messages>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserService, UserService>();

// 4. Add HttpContextAccessor, SignalR, and other middleware services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();

// 5. Add Session Handling
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Cookie settings for security
    options.Cookie.IsEssential = true; // Make the session cookie essential for GDPR compliance
});

// 6. JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

// 7. CORS Policy to allow Blazor app on localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://localhost:7094") // Blazor WASM app origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Important for SignalR
    });
});

// 8. Add Controllers
builder.Services.AddControllers();

// 9. Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. Enable CORS
app.UseCors("AllowAll");

// 2. Enable Session Middleware
app.UseSession();

// 3. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 4. Map SignalR Hubs and Controllers
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalRHelper>("/signalrhelper");
});

app.MapControllers();

// Run the application
app.Run();
