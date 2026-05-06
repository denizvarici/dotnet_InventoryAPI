using InventoryAPI.Application.Commands.CreateProduct;
using InventoryAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICommandHandler<CreateProductCommand,Guid> _createHandler;

        public ProductsController(ICommandHandler<CreateProductCommand, Guid> createHandler)
        {
            _createHandler = createHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductCommand command,CancellationToken cancellationToken)
        {
            var productId = await _createHandler.HandleAsync(command, cancellationToken);
            return Created(string.Empty, new { Id = productId });
        }
    }
}
