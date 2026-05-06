namespace InventoryAPI.API.GraphQL;

public class Subscription
{
    [Subscribe]
    [Topic("LowStockAlert")]
    public string OnLowStockAlert([EventMessage] string message)
    {
        return message;
    }
}