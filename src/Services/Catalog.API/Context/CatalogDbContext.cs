using MongoRepo.Context;

namespace Catalog.API.Context
{
    public class CatalogDbContext : ApplicationDbContext
    {
        //Configuration
        static IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        //Get ConnectionString using Configuration
        static string connectionString = configuration.GetConnectionString("Catalog.API");

        //Get DatabaseName using Configuration
        static string databaseName = configuration.GetValue<string>("DatabaseName");

        //CTOR
        public CatalogDbContext() : base(connectionString, databaseName)
        {

        }
    }
}
