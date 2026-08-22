using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RafeekRoomMate.Application.Features.Authentication.Commands.Login;
using RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens;
using RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken;
using RafeekRoomMate.Application.Features.Authentication.Queries.GetCurrentUser;
using RafeekRoomMate.Application.Features.Authentication.Register;
using RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.DeleteProfileImage;
using RafeekRoomMate.Application.Features.Profiles.Commands.UpdateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.UploadProfileImage;
using RafeekRoomMate.Application.Features.Profiles.Queries.GetMyProfile;
using RafeekRoomMate.Application.Features.RoomFavorites.Commands.AddRoomFavorite;
using RafeekRoomMate.Application.Features.RoomFavorites.Commands.RemoveRoomFavorite;
using RafeekRoomMate.Application.Features.RoomFavorites.Queries.GetMyFavoriteRooms;
using RafeekRoomMate.Application.Features.Rooms.Commands.AddRoomImage;
using RafeekRoomMate.Application.Features.Rooms.Commands.CreateRoom;
using RafeekRoomMate.Application.Features.Rooms.Commands.DeleteRoom;
using RafeekRoomMate.Application.Features.Rooms.Commands.DeleteRoomImage;
using RafeekRoomMate.Application.Features.Rooms.Commands.UpdateRoom;
using RafeekRoomMate.Application.Features.Rooms.Queries.GetRoomDetails;
using RafeekRoomMate.Application.Features.Rooms.Queries.GetRooms;
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
            services.AddScoped<DeleteProfileImageCommandHandler>();
            services.AddScoped<CreateRoomCommandHandler>();
            services.AddScoped<AddRoomImageCommandHandler>();
            services.AddScoped<GetRoomDetailsQueryHandler>();
            services.AddScoped<GetRoomsQueryHandler>();
            services.AddScoped<UpdateRoomCommandHandler>();
            services.AddScoped<DeleteRoomCommandHandler>();
            services.AddScoped<DeleteRoomImageCommandHandler>();
            services.AddScoped<AddRoomFavoriteCommandHandler>();
            services.AddScoped<RemoveRoomFavoriteCommandHandler>();
            services.AddScoped<GetMyFavoriteRoomsQueryHandler>();

            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            
            return services;
        }
    }
}
