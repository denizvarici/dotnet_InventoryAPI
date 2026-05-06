using InventoryAPI.Application.Commands.CreateWarehouse;
using InventoryAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ICommandHandler<CreateWarehouseCommand,Guid> _createHandler;

        public WarehousesController(ICommandHandler<CreateWarehouseCommand, Guid> createHandler)
        {
            _createHandler = createHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command,CancellationToken cancellationToken)
        {
            var warehouseId = await _createHandler.HandleAsync(command, cancellationToken);

            return Created(string.Empty, new { Id = warehouseId });
        }
    }
}
