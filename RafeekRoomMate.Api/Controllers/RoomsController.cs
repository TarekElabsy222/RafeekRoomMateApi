using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace RafeekRoomMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController: ControllerBase
    {
        #region fields
        private readonly CreateRoomCommandHandler _createRoomHandler;
        private readonly AddRoomImageCommandHandler _addRoomImageHandler;
        private readonly GetRoomDetailsQueryHandler _getRoomDetailsQueryHandler;
        private readonly GetRoomsQueryHandler _getRoomsQueryHandler;
        private readonly UpdateRoomCommandHandler _updateRoomCommandHandler;
        private readonly DeleteRoomCommandHandler _deleteRoomCommandHandler;
        private readonly DeleteRoomImageCommandHandler _deleteRoomImageCommandHandler;
        private readonly AddRoomFavoriteCommandHandler _addRoomFavoriteCommandHandler;
        private readonly RemoveRoomFavoriteCommandHandler _removeRoomFavoriteCommandHandler;
        private readonly GetMyFavoriteRoomsQueryHandler _getMyFavoriteRoomsQueryHandler;
        #endregion

        #region constructor
        public RoomsController(CreateRoomCommandHandler createRoomHandler, AddRoomImageCommandHandler addRoomImageHandler, GetRoomDetailsQueryHandler getRoomDetailsQueryHandler, GetRoomsQueryHandler getRoomsQueryHandler, UpdateRoomCommandHandler updateRoomCommandHandler, DeleteRoomCommandHandler deleteRoomCommandHandler, DeleteRoomImageCommandHandler deleteRoomImageCommandHandler, AddRoomFavoriteCommandHandler addRoomFavoriteCommandHandler, RemoveRoomFavoriteCommandHandler removeRoomFavoriteCommandHandler, GetMyFavoriteRoomsQueryHandler getMyFavoriteRoomsQueryHandler)
        {
            _createRoomHandler = createRoomHandler;
            _addRoomImageHandler = addRoomImageHandler;
            _getRoomDetailsQueryHandler = getRoomDetailsQueryHandler;
            _getRoomsQueryHandler = getRoomsQueryHandler;
            _updateRoomCommandHandler = updateRoomCommandHandler;
            _deleteRoomCommandHandler = deleteRoomCommandHandler;
            _deleteRoomImageCommandHandler = deleteRoomImageCommandHandler;
            _addRoomFavoriteCommandHandler = addRoomFavoriteCommandHandler;
            _removeRoomFavoriteCommandHandler = removeRoomFavoriteCommandHandler;
            _getMyFavoriteRoomsQueryHandler = getMyFavoriteRoomsQueryHandler;
        }
        #endregion

        #region handle functions

        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <remarks>
        /// Creates a new room using the information provided in the request body.
        /// </remarks>
        /// <response code="200">Room created successfully.</response>
        /// <response code="400">The room data is invalid or the room could not be created.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomCommand command,CancellationToken cancellationToken)
        {
            var result = await _createRoomHandler.HandleAsync(command,cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Adds an image to a room.
        /// </summary>
        /// <remarks>
        /// Uploads an image for the specified room.
        /// The image must be sent using multipart/form-data.
        /// </remarks>
        /// <response code="200">Room image added successfully.</response>
        /// <response code="400">The image is invalid or the image could not be added.</response>
        [HttpPost("{roomId:guid}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage(Guid roomId,IFormFile image, CancellationToken cancellationToken)
        {
            await using var stream = image.OpenReadStream();

            var command = new AddRoomImageCommand
            {
                RoomId = roomId,
                Stream = stream,
                FileName = image.FileName,
                ContentType = image.ContentType,
                Length = image.Length
            };

            var result =  await _addRoomImageHandler.HandleAsync(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the details of a specific room.
        /// </summary>
        /// <remarks>
        /// Returns the complete details of the specified room.
        /// This endpoint is publicly accessible and does not require authentication.
        /// </remarks>
        /// <response code="200">Room details retrieved successfully.</response>
        /// <response code="404">The specified room was not found.</response>
        [HttpGet("{roomId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomDetails(Guid roomId,CancellationToken cancellationToken)
        {
            var query = new GetRoomDetailsQuery
            {
                RoomId = roomId
            };

            var result =await _getRoomDetailsQueryHandler.HandleAsync(query,cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of available rooms.
        /// </summary>
        /// <remarks>
        /// Returns rooms based on the filtering and pagination options
        /// provided in the query string.
        /// This endpoint is publicly accessible and does not require authentication.
        /// </remarks>
        /// <response code="200">Rooms retrieved successfully.</response>
        /// <response code="400">The provided query parameters are invalid.</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRooms([FromQuery] GetRoomsQuery query,CancellationToken cancellationToken)
        {
            var result = await _getRoomsQueryHandler.HandleAsync(query,cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <remarks>
        /// Updates the details of the specified room using the information
        /// provided in the request body.
        /// Requires a valid JWT access token.
        /// </remarks>
        /// <response code="200">Room updated successfully.</response>

        [HttpPut("{roomId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateRoom(Guid roomId,[FromBody] UpdateRoomCommand command,CancellationToken cancellationToken)
        {
            var result = await _updateRoomCommandHandler.HandleAsync(roomId,command,cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing room.
        /// </summary>
        /// <remarks>
        /// Deletes the specified room.
        /// Requires a valid JWT access token.
        /// </remarks>
        /// <response code="200">Room deleted successfully.</response>
        [HttpDelete("{roomId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoom( Guid roomId, CancellationToken cancellationToken)
        {
            var result =
                await _deleteRoomCommandHandler.HandleAsync( roomId, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{roomId:guid}/images/{imageId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoomImage( Guid roomId, Guid imageId, CancellationToken cancellationToken)
        {
            var result =
                await _deleteRoomImageCommandHandler.HandleAsync( roomId, imageId, cancellationToken);

            return Ok(result);
        }
        [HttpPost("{roomId:guid}/favorite")]
        [Authorize]
        public async Task<IActionResult> AddFavorite( Guid roomId, CancellationToken cancellationToken)
        {
            var result = await _addRoomFavoriteCommandHandler.HandleAsync( roomId, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{roomId:guid}/favorite")]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite( Guid roomId, CancellationToken cancellationToken)
        {
            
            var result = await _removeRoomFavoriteCommandHandler.HandleAsync( roomId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("favorites")]
        [Authorize]
        public async Task<IActionResult> GetMyFavorites( [FromQuery] GetMyFavoriteRoomsQuery query, CancellationToken cancellationToken)
        {
            var result = await _getMyFavoriteRoomsQueryHandler.HandleAsync( query, cancellationToken);

            return Ok(result);
        }
        #endregion
    }
}
