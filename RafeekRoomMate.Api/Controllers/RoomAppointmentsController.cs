using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafeekRoomMate.Application.Features.RoomAppointments.Commands.ApproveRoomAppointment;
using RafeekRoomMate.Application.Features.RoomAppointments.Commands.CreateRoomAppointment;
using RafeekRoomMate.Application.Features.RoomAppointments.Commands.RejectRoomAppointment;
using RafeekRoomMate.Application.Features.RoomAppointments.Queries.GetRoomAppointments;

namespace RafeekRoomMate.Api.Controllers
{
    [ApiController]
    [Route("api/rooms/{roomId:guid}/appointments")]
    [Authorize]
    public sealed class RoomAppointmentsController : ControllerBase
    {
        #region fields
        private readonly CreateRoomAppointmentCommandHandler _createHandler;
        private readonly GetRoomAppointmentsQueryHandler _getHandler;
        private readonly ApproveRoomAppointmentCommandHandler _approveHandler;
        private readonly RejectRoomAppointmentCommandHandler _rejectHandler;

        #endregion

        #region constructor
        public RoomAppointmentsController(CreateRoomAppointmentCommandHandler createHandler, GetRoomAppointmentsQueryHandler getHandler, ApproveRoomAppointmentCommandHandler approveHandler, RejectRoomAppointmentCommandHandler rejectHandler)
        {
            _createHandler = createHandler;
            _getHandler = getHandler;
            _approveHandler = approveHandler;
            _rejectHandler = rejectHandler;
        }
        #endregion

        #region handle functions
        [HttpPost]
        public async Task<IActionResult> Create(Guid roomId, CreateRoomAppointmentCommand command, CancellationToken cancellationToken)
        {


            var result = await _createHandler.HandleAsync(roomId, command, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid roomId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetRoomAppointmentsQuery
            {
                RoomId = roomId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _getHandler.HandleAsync(query, cancellationToken);

            return Ok(result);
        }

        [HttpPatch("{appointmentId:guid}/approve")]
        public async Task<IActionResult> Approve(Guid roomId, Guid appointmentId, CancellationToken cancellationToken)
        {
            var command = new ApproveRoomAppointmentCommand
            {
                RoomId = roomId,
                AppointmentId = appointmentId
            };

            var result = await _approveHandler.HandleAsync(command, cancellationToken);

            return Ok(result);
        }


        [HttpPatch("{appointmentId:guid}/reject")]
        public async Task<IActionResult> Reject(Guid roomId, RejectRoomAppointmentCommand command, CancellationToken cancellationToken)
        {
            var data = new RejectRoomAppointmentCommand
            {
                RoomId = roomId,
                AppointmentId = command.AppointmentId,
                Reason = command.Reason
            };

            var result = await _rejectHandler.HandleAsync(command, cancellationToken);

            return Ok(result);
        }


        #endregion
    }

}
