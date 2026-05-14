using Xunit;
using FitnessStudio.Business.Services;
using FitnessStudio.Data.Repositories;
using FitnessStudio.Data.Database;
using FitnessStudio.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace FitnessStudio.Tests.Integration
{
    public class PaketIntegrationTests
    {
        private readonly PaketService _service;

        public PaketIntegrationTests()
        {
            var configurationData = new Dictionary<string, string?>
{
    {
        "ConnectionStrings:DefaultConnection",
        "Host=localhost;Port=5432;Database=fitness_studio;Username=postgres;Password=bazepodataka"
    }
};

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            var connectionFactory = new DatabaseConnectionFactory(configuration);

            var repository = new PaketRepository(connectionFactory);

            _service = new PaketService(repository);
        }

        [Fact]
        public void GetAll_Should_Return_Packages_From_Database()
        {
            var result = _service.GetAll();

            Assert.NotNull(result);
        }

        [Fact]
        public void Create_Update_Delete_Flow_Should_Work()
        {
            var paket = new Paket
            {
                Naziv = "Test Paket " + System.Guid.NewGuid(),
                BrojTreninga = 10,
                Cijena = 25
            };

            var createResult = _service.Create(paket, out string? createError);
            Assert.True(createResult);

            var saved = _service.SearchByName(paket.Naziv);
            var savedPaket = System.Linq.Enumerable.FirstOrDefault(saved);

            Assert.NotNull(savedPaket);

            savedPaket!.Naziv = "Updated Paket";
            var updateResult = _service.Update(savedPaket, out string? updateError);
            Assert.True(updateResult);

            var deleteResult = _service.Delete(savedPaket.PaketId, out string? deleteError);
            Assert.True(deleteResult);
        }
    }
}