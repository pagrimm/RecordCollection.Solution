using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RecordCollection.Models
{
  public class RecordCollectionContextFactory : IDesignTimeDbContextFactory<RecordCollectionContext>
  {

    RecordCollectionContext IDesignTimeDbContextFactory<RecordCollectionContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<RecordCollectionContext>();
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      builder.UseMySql(connectionString);

      return new RecordCollectionContext(builder.Options);
    }
  }
}