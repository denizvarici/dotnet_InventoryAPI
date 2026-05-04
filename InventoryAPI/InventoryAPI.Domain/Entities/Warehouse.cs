namespace InventoryAPI.Domain.Entities
{
    public class Warehouse
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Stock> _stocks = new();
        public IReadOnlyList<Stock> Stocks => _stocks.AsReadOnly();

        private Warehouse(){}

        public static Warehouse Create(string name, string location)
        {
            return new Warehouse()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Location = location,
                IsActive = true
            };
        }

        public void Diactivate() => IsActive = false;

    }
}
