using DemoWebApi.EFCore;
using DemoWebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Text;

namespace DemoWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .WriteTo.File("Logs/logs.txt")
               .CreateLogger();

            try
            {
                Log.Information("Starting Host.");

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllers();

                builder.Services.AddSwaggerGen();

                // Add services to the container.
                builder.Services.AddAuthorization();

                // thiết lập kết nối đến CSDL

                // lấy ra chuỗi kết nối từ config appsetting.json
                string connection = builder.Configuration.GetConnectionString("Default");

                // bát đầu kết nối
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PVI ReSmart API", Version = "v1" });
                    options.CustomSchemaIds(type => type.FullName);
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = HeaderNames.Authorization,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new List<string>()
                        }
                    });
                });

                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                        };
                    });

                builder.Services.AddLocalization();

                builder.Services.AddDistributedMemoryCache();

                builder.Services.RegisterServices();

                builder.Services.AddMvc();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseRouting();

                // Configure the HTTP request pipeline.
                app.UseAuthentication();
                app.UseAuthorization();

                var options = new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(new CultureInfo("vi"))
                };

                app.UseStaticFiles();

                app.UseMiddleware<LocalizationMiddleware>();

                app.UseMiddleware<AutoLogRequestMiddleware>();

                app.UseMiddleware<HandleExceptionMiddleware>();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}