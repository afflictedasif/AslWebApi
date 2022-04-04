using AslWebApi;
using AslWebApi.DAL;
using AslWebApi.DAL.Repositories;
using AslWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

builder.Services.AddScoped<IJsonDBService, JsonDBService>();
builder.Services.AddScoped<IFileUploader, FileUploader>();
builder.Services.AddScoped<IScreenShotService, ScreenShotService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserStateRepo, UserStateRepo>();
builder.Services.AddScoped<IUserStateService, UserStateService>();


builder.Services.AddTransient<GlobalFunctions>();
//Database Connections
string connectionString = GlobalFunctions.ConnectionString;
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

//Authentication
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:7110",
        ValidAudience = "http://localhost:7110",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//HttpHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
//SeedData.SeedDatabase(app.Services.GetRequiredService<DatabaseContext>());

//if (args.Length == 1 && args[0].ToLower() == "seeddata")
Seed(app);

//Seed Data
void Seed(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory?.CreateScope())
    {
        var dbContext = scope?.ServiceProvider.GetService<DatabaseContext>();
        if (dbContext != null) SeedData.SeedDatabase(dbContext);

        var httpContextAccessor = scope?.ServiceProvider.GetService<IHttpContextAccessor>();
        if (httpContextAccessor != null) HttpHelper.Configure(httpContextAccessor);
    }
}



app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();



