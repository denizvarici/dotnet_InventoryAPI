namespace InventoryAPI.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string SKU { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        private Product(){}

        public static Product Create(string name, string sku, string category)
        {
            //business rules can be applied here.
            return new Product()
            {
                Id = Guid.NewGuid(),
                Name = name,
                SKU = sku,
                Category = category,
                IsActive = true
            };
        }
        public void Deactivate()=>IsActive = false;
    }
}
