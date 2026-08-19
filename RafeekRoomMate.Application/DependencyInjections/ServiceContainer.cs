using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RafeekRoomMate.Application.Features.Authentication.Commands.Login;
using RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens;
using RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken;
using RafeekRoomMate.Application.Features.Authentication.Queries.GetCurrentUser;
using RafeekRoomMate.Application.Features.Authentication.Register;
using RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.UpdateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.UploadProfileImage;
using RafeekRoomMate.Application.Features.Profiles.Queries.GetMyProfile;
namespace RafeekRoomMate.Application.DependencyInjections
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<RegisterCommandHandler>();
            services.AddScoped<LoginCommandHandler>();
            services.AddScoped<RefreshTokenCommandHandler>();
            services.AddScoped<RevokeRefreshTokenCommandHandler>();
            services.AddScoped<GetCurrentUserQueryHandler>();
            services.AddScoped<CreateProfileCommandHandler>();
            services.AddScoped<GetMyProfileQueryHandler>();
            services.AddScoped<UpdateProfileCommandHandler>();
            services.AddScoped<UploadProfileImageCommandHandler>();


            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            
            return services;
        }
    }
}
