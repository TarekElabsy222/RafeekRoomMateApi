using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Options;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Infrastructure.Authentication;
using RafeekRoomMate.Infrastructure.Persistence.Context;
using RafeekRoomMate.Infrastructure.Persistence.Identity;
using RafeekRoomMate.Infrastructure.Persistence.Seed;
using RafeekRoomMate.Infrastructure.Repositories.Implementations;
using RafeekRoomMate.Infrastructure.Services;
using RafeekRoomMate.Infrastructure.Storage;
using System.Text;

namespace RafeekRoomMate.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            {
                #region register context and identity
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                        //sqlOptions.EnableRetryOnFailure();
                    }).UseExceptionProcessor();
                }, ServiceLifetime.Scoped);


                services.AddIdentityCore<User>(options =>
                  {
                     options.User.RequireUniqueEmail = true;
                     options.Password.RequiredLength = 8;
                     options.Password.RequireDigit = true;
                     options.Password.RequireUppercase = true;
                     options.Password.RequireLowercase = true;
                     options.Password.RequireNonAlphanumeric = true;
                 }).AddRoles<UserRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
                #endregion

                #region Jwt otions
                services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
                // JWT Authentication
                var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                    ?? throw new InvalidOperationException("JWT configuration is missing.");

                var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuer = true,

                                ValidateAudience = true,

                                ValidateLifetime = true,

                                ValidateIssuerSigningKey = true,

                                ValidIssuer = jwtOptions.Issuer,

                                ValidAudience = jwtOptions.Audience,

                                IssuerSigningKey = new SymmetricSecurityKey(key),

                                ClockSkew = TimeSpan.FromSeconds(30)
                            };
                    });
                #endregion

                #region register services
                services.AddScoped<IIdentityService, IdentityService>();
                services.AddScoped<IJwtProvider, JwtProvider>();
                services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();
                services.AddScoped<ICurrentUserService, CurrentUserService>();
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddHttpContextAccessor();
                services.AddScoped<IFileStorageService, LocalFileStorageService>();
                services.Configure<ProfileImageOptions>(configuration.GetSection("Storage:ProfileImage"));
                services.Configure<RoomImageOptions>(configuration.GetSection(RoomImageOptions.SectionName));
                services.AddSingleton<IImageValidator,ProfileImageValidator>();
                services.AddScoped<IRoomImageValidator,RoomImageValidator>();
                #endregion

                return services;
            }
        }

        public static async Task<IApplicationBuilder> UseSeedRolesScopeAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider .GetRequiredService<  RoleManager<UserRole>>();

                await IdentitySeeder.SeedAsync(roleManager);
            }

                return app;
        }
    }
}
