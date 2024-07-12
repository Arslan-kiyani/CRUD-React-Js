
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniExcelLibs;
using OfficeOpenXml;
using ReadFile_Mini.Context;
using ReadFile_Mini.Helper;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Models.Repository;
using ReadFile_Mini.Repository;
using ReadFile_Mini.Services;
using System.Text;

namespace ReadFile_Mini
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<SeniorDb>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("SawagarConnection")));

            builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
                 options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));


            // Set the ExcelPackage license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600; // 100 MB
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 104857600; // 100 MB
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100 MB
            });

            builder.Services.AddControllers();

            // // Add Hangfire services
            builder.Services.AddHangfire(configuration => configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage(builder.Configuration.GetConnectionString("SawagarConnection")));

            builder.Services.AddHangfireServer();

            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole(); // Example: Console logging
                                      
            });

            // start jwt token 
            var key = Encoding.ASCII.GetBytes("YourSs342@uperSecretKeyHere45srs@dfdfsdasdadsad");
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var secretKey = "YourSs342@uperSecretKeyHere45srs@dfdfsdasdadsad";
            var issuer = "yourdomain.com";
            var audience = "yourdomain.com";
            builder.Services.AddSingleton<IJwtService>(new JwtService(secretKey, issuer, audience));

            // Add Swagger
                    builder.Services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                        // Add JWT Authentication
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value."
                        });
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            // end code of jwt token

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()  // Allow all methods
                           .AllowAnyHeader();
                });
            });

            

            builder.Services.AddTransient<sdvaHelper>();
            builder.Services.AddTransient<InventoryHelper>();
            builder.Services.AddScoped<JournalHelper>();
            builder.Services.AddScoped<ExcelFileUploadHelper>();
            builder.Services.AddTransient<EmailService>();

            builder.Services.AddScoped<ExcelHelper>(); 
            builder.Services.AddScoped<HelperExcel>();
           

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ITripRepository, TripRepository>();
            builder.Services.AddTransient<ITripService, TripService>();

            builder.Services.AddTransient<IUserTableRepository, UserTableRepository>();
            builder.Services.AddTransient<IUserTableService, UserTableService>();

            builder.Services.AddTransient<IUserTripRepository, UserTripRepository>();
            builder.Services.AddTransient<IUserTripService, UserTripService>();

            builder.Services.AddTransient<IpersonalInfoServicecs, PersonalInfoService>();
            builder.Services.AddScoped<ILanguagesserver, Languagesserver>();
            builder.Services.AddTransient<ILanguagerepos, Languagerepos>();
            builder.Services.AddTransient<IraceRepository, RaceRepository>();
            builder.Services.AddTransient<IRaceservice, RaceService>();
            builder.Services.AddTransient<IpersonalInfoRepository, personalInfoRepository>();

            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Register the GenericRepository
            builder.Services.AddTransient<DataTransferJob>(); // register job service

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
            
            app.UseHttpsRedirection();

            app.UseHangfireDashboard();
            //app.UseMiddleware<ApiKeyMiddleware>(); 
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors("AllowAll"); 

            app.MapControllers();

            app.Run();
        }
    }
}
