using System;
namespace DiscountCodeAPI.Data
{
    public class DBSettings
    {
        public string Connection { get; set; }
        public string DatabaseName { get; set; }
    }

    public class TestDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;
    }
}
