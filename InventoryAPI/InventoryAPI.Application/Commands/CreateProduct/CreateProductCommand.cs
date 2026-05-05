namespace InventoryAPI.Application.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Sku, string Category);