using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class ApiApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbConnection));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                //SQLitePCL.Batteries_V2.Init();

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddScoped<IDbConnection>(sp => connection);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IDbConnection>();

                    // Initialize the database
                    using (var command = db.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS DDD (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                NumDDD INTEGER,
                                regiao TEXT
                            );

                            CREATE TABLE IF NOT EXISTS Contatos (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Nome TEXT,
                                Telefone TEXT,
                                Email TEXT,
                                DDDID INTEGER NOT NULL,
                                FOREIGN KEY (DDDID) REFERENCES DDD(Id)
                            );
                        ";
                        command.ExecuteNonQuery();
                    }
                }
            });

            return base.CreateHost(builder);
        }
    }
}
