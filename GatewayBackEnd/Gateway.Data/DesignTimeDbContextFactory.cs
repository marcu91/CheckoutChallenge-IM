using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gateway.Data
{
    /// <summary>
    /// Implementation of the IDesignTimeDbContextFactory interface to bypass the requirment of specifying
    /// the DBContext in the application startup for Migrations
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GatewayDBContext>
    {
        public GatewayDBContext CreateDbContext(string[] args)
        {
            //TODO store the db config in Azure Key Vault. Temp for now, do not commit for security purposes
            var builder = new DbContextOptionsBuilder<GatewayDBContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=GatewayDataDB;Integrated Security=True;");

            return new GatewayDBContext(builder.Options);
        }
    }
}