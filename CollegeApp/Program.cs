
using CollegeApp;
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogging;
using CollegeApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Logging.AddLog4Net();

#region Serilog Settings
//Log.Logger = new LoggerConfiguration().
//    MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt",
//    rollingInterval: RollingInterval.Minute)
//    .CreateLogger();

//use this line to override the built-in loggers
//builder.Host.UseSerilog();

//Use serilog alogn with built-in loggers
//builder.Logging.AddSerilog();
#endregion

builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});



// Add services to the container.
builder.Services.AddControllers(
//options => options.ReturnHttpNotAcceptable = true
).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer swersdf877sdf",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",

                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com", "http://gmail.com", "http://drive.google.com").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        policy.WithOrigins("http://outlook.com", "http://microsoft.com", "http://onedrive.google.com").AllowAnyHeader().AllowAnyMethod();
    });
});
var keyJWTSecretforGoogle = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretforGoogle"));
var keyJWTSecretforMicrosoft = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretforMicrosoft"));
var keyJWTSecretforLocal = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretforLocal"));
string GoogleAudience = builder.Configuration.GetValue<string>("GoogleAudience");
string MicrosoftAudience = builder.Configuration.GetValue<string>("MicrosoftAudience");
string LocalAudience = builder.Configuration.GetValue<string>("LocalAudience");
string GoogleIssuer = builder.Configuration.GetValue<string>("GoogleIssuer");
string MicrosoftIssuer = builder.Configuration.GetValue<string>("MicrosoftIssuer");
string LocalIssuer = builder.Configuration.GetValue<string>("LocalIssuer");

//JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGooleUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretforGoogle),

        ValidateIssuer = true,
        ValidIssuer = GoogleIssuer,

        ValidateAudience = true,
        ValidAudience = GoogleAudience
    };
}).AddJwtBearer("LoginForMicrosoftUsers", options =>
 {
     //options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretforMicrosoft),

         ValidateIssuer = true,
         ValidIssuer = MicrosoftIssuer,

         ValidateAudience = true,
         ValidAudience = MicrosoftAudience
     };
 }).AddJwtBearer("LoginForLocalUsers", options =>
 {
     //options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretforLocal),

         ValidateIssuer = true,
         ValidIssuer = LocalIssuer,

         ValidateAudience = true,
         ValidAudience = LocalAudience
     };
 });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response"))
        .RequireCors("AllowOnlyLocalhost");

    endpoints.MapControllers()
             .RequireCors("AllowAll");

    endpoints.MapGet("api/testendpoint2",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));

});

app.Run();
