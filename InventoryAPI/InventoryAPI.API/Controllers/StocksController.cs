using InventoryAPI.Application.Commands.AddStock;
using InventoryAPI.Application.Commands.TransferStock;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Application.Queries.GetLowStockProducts;
using InventoryAPI.Application.Queries.GetStockReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ICommandHandler<AddStockCommand, Guid> _addStockHandler;
        private readonly ICommandHandler<TransferStockCommand, Guid> _transferStockHandler;
        private readonly IQueryHandler<GetStockReportQuery, IEnumerable<StockReportDto>> _stockReportHandler;
        private readonly IQueryHandler<GetLowStockProductsQuery, IEnumerable<LowStockProductDto>> _lowStockHandler;

        public StocksController(ICommandHandler<AddStockCommand, Guid> addStockHandler, ICommandHandler<TransferStockCommand, Guid> transferStockHandler, IQueryHandler<GetStockReportQuery, IEnumerable<StockReportDto>> stockReportHandler, IQueryHandler<GetLowStockProductsQuery, IEnumerable<LowStockProductDto>> lowStockHandler)
        {
            _addStockHandler = addStockHandler;
            _transferStockHandler = transferStockHandler;
            _stockReportHandler = stockReportHandler;
            _lowStockHandler = lowStockHandler;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStock([FromBody] AddStockCommand command,
            CancellationToken cancellationToken)
        {
            var stockId = await _addStockHandler.HandleAsync(command, cancellationToken);
            return Ok(new { Message = "Stock added/updated successfully.", StockId = stockId });
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var transferId = await _transferStockHandler.HandleAsync(command, cancellationToken);
                return Ok(new { Message = "Transfer succeeded.", TransferReferenceId = transferId });
            }
            catch (Exception exception)
            {
                return BadRequest(new{Error = exception.Message });
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetStockReport(CancellationToken cancellationToken)
        {
            var query = new GetStockReportQuery();
            var report = await _stockReportHandler.HandleAsync(query, cancellationToken);
            return Ok(report);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts(CancellationToken cancellationToken)
        {
            var query = new GetLowStockProductsQuery();
            var lowStocks = await _lowStockHandler.HandleAsync(query, cancellationToken);
            return Ok(lowStocks);
        }
    }
}
