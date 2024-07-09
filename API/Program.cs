
using Application.Interfaces;
using Infrastructure.Repositories;
using System.Data;
using System.Data.SqlClient;
using TechChallenge_Contatos.Repository;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IContatoCadastro, ContatoRepository>();
            builder.Services.AddScoped<IDDDCadastro, DDDRepository>();

            var stringConexao = configuration.GetValue<string>("ConnectionStringSQL");

            builder.Services.AddScoped<IDbConnection>((conexao) => new SqlConnection(stringConexao));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
