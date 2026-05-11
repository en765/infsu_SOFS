using Xunit;
using Moq;
using FluentAssertions;

using FitnessStudio.Business.Services;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Tests.Services
{
    public class PaketServiceTests
    {
        [Fact]
        public void GetAll_Should_Return_All_Packages()
        {
            var mockRepo = new Mock<IPaketRepository>();

            mockRepo.Setup(r => r.GetAll())
                .Returns(new List<Paket>
                {
                    new Paket
                    {
                        PaketId = 1,
                        Naziv = "Basic",
                        BrojTreninga = 10,
                        Cijena = 30
                    },
                    new Paket
                    {
                        PaketId = 2,
                        Naziv = "Premium",
                        BrojTreninga = 20,
                        Cijena = 50
                    }
                });

            var service = new PaketService(mockRepo.Object);

            var result = service.GetAll();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void Create_Should_Fail_When_Name_Is_Empty()
        {
            var mockRepo = new Mock<IPaketRepository>();

            var service = new PaketService(mockRepo.Object);

            var paket = new Paket
            {
                Naziv = "",
                BrojTreninga = 10,
                Cijena = 30
            };

            var result = service.Create(paket, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Naziv paketa je obavezan.");
        }

        [Fact]
        public void Create_Should_Fail_When_Package_Already_Exists()
        {
            var mockRepo = new Mock<IPaketRepository>();

            mockRepo.Setup(r => r.ExistsByName("Basic"))
                .Returns(true);

            var service = new PaketService(mockRepo.Object);

            var paket = new Paket
            {
                Naziv = "Basic",
                BrojTreninga = 10,
                Cijena = 30
            };

            var result = service.Create(paket, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Paket s istim nazivom već postoji.");
        }

        [Fact]
        public void Create_Should_Succeed_When_Data_Is_Valid()
        {
            var mockRepo = new Mock<IPaketRepository>();

            mockRepo.Setup(r => r.ExistsByName(It.IsAny<string>()))
                .Returns(false);

            var service = new PaketService(mockRepo.Object);

            var paket = new Paket
            {
                Naziv = "Gold",
                BrojTreninga = 15,
                Cijena = 60
            };

            var result = service.Create(paket, out string? errorMessage);

            result.Should().BeTrue();
            errorMessage.Should().BeNull();

            mockRepo.Verify(r => r.Insert(It.IsAny<Paket>()), Times.Once);
        }

        [Fact]
        public void Delete_Should_Fail_When_Package_Does_Not_Exist()
        {
            var mockRepo = new Mock<IPaketRepository>();

            mockRepo.Setup(r => r.GetById(1))
                .Returns((Paket?)null);

            var service = new PaketService(mockRepo.Object);

            var result = service.Delete(1, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Paket nije pronađen.");
        }
    }
}