using Xunit;
using Dapper;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class ContatosControllerIntegrationTests : IClassFixture<ApiApplication>
    {
        private readonly HttpClient _client;
        private readonly IDbConnection _dbConnection;

        public ContatosControllerIntegrationTests(ApiApplication factory)
        {
            _client = factory.CreateClient();
            _dbConnection = factory.Services.GetRequiredService<IDbConnection>();
        }

        [Fact]
        public async Task GetContato_ReturnsSuccess()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var response = await _client.GetAsync("/api/Contatos/Listar");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            await CleanupDatabase();
        }

        private async Task SeedDatabase()
        {
            var commandText = @"
                INSERT INTO DDD (NumDDD, regiao) VALUES (11, 'São Paulo'); 
                INSERT INTO Contatos (Nome, Telefone, Email, DDDID) VALUES ('Teste', '11123456789', 'teste@example.com', (SELECT Id FROM DDD WHERE NumDDD = 11));
            ";
            await _dbConnection.ExecuteAsync(commandText);
        }

        private async Task CleanupDatabase()
        {
            var commandText = @"DELETE FROM Contatos; DELETE FROM DDD;";
            await _dbConnection.ExecuteAsync(commandText);
        }

        // Adicione mais testes conforme necessário
    }
}
