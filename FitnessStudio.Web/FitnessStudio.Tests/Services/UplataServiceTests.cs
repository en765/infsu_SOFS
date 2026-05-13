using Xunit;
using Moq;
using FluentAssertions;

using FitnessStudio.Business.Services;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Tests.Services
{
    public class UplataServiceTests
    {
        [Fact]
        public void GetByClanarinaId_Should_Return_Uplate()
        {
            var mockUplataRepo = new Mock<IUplataRepository>();
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();

            mockUplataRepo.Setup(r => r.GetByClanarinaId(1))
                .Returns(new List<Uplata>
                {
                    new Uplata { UplataId = 1, ClanarinaId = 1, Iznos = 20, Datum = DateTime.Today },
                    new Uplata { UplataId = 2, ClanarinaId = 1, Iznos = 30, Datum = DateTime.Today }
                });

            var service = new UplataService(
                mockUplataRepo.Object,
                mockClanarinaRepo.Object,
                mockPaketRepo.Object);

            var result = service.GetByClanarinaId(1);

            result.Should().HaveCount(2);
        }

        [Fact]
        public void Create_Should_Fail_When_Amount_Is_Negative()
        {
            var mockUplataRepo = new Mock<IUplataRepository>();
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();

            mockClanarinaRepo.Setup(r => r.GetById(1))
                .Returns(new Clanarina { ClanarinaId = 1, PaketId = 1 });

            mockPaketRepo.Setup(r => r.GetById(1))
                .Returns(new Paket { PaketId = 1, Cijena = 100 });

            var service = new UplataService(
                mockUplataRepo.Object,
                mockClanarinaRepo.Object,
                mockPaketRepo.Object);

            var uplata = new Uplata
            {
                ClanarinaId = 1,
                Iznos = -10,
                Datum = DateTime.Today
            };

            var result = service.Create(uplata, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Iznos uplate mora biti veći od 0.");
        }

        [Fact]
        public void Create_Should_Fail_When_Total_Exceeds_Package_Price()
        {
            var mockUplataRepo = new Mock<IUplataRepository>();
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();

            mockUplataRepo.Setup(r => r.GetTotalForClanarina(1))
                .Returns(40);

            mockClanarinaRepo.Setup(r => r.GetById(1))
                .Returns(new Clanarina { ClanarinaId = 1, PaketId = 1 });

            mockPaketRepo.Setup(r => r.GetById(1))
                .Returns(new Paket { PaketId = 1, Cijena = 50 });

            var service = new UplataService(
                mockUplataRepo.Object,
                mockClanarinaRepo.Object,
                mockPaketRepo.Object);

            var uplata = new Uplata
            {
                ClanarinaId = 1,
                Iznos = 20,
                Datum = DateTime.Today
            };

            var result = service.Create(uplata, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Ukupan iznos uplata ne smije biti veći od cijene paketa.");
        }

        [Fact]
        public void Create_Should_Succeed_When_Data_Is_Valid()
        {
            var mockUplataRepo = new Mock<IUplataRepository>();
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();

            mockUplataRepo.Setup(r => r.GetTotalForClanarina(1))
                .Returns(20);

            mockClanarinaRepo.Setup(r => r.GetById(1))
                .Returns(new Clanarina { ClanarinaId = 1, PaketId = 1 });

            mockPaketRepo.Setup(r => r.GetById(1))
                .Returns(new Paket { PaketId = 1, Cijena = 100 });

            var service = new UplataService(
                mockUplataRepo.Object,
                mockClanarinaRepo.Object,
                mockPaketRepo.Object);

            var uplata = new Uplata
            {
                ClanarinaId = 1,
                Iznos = 20,
                Datum = DateTime.Today
            };

            var result = service.Create(uplata, out string? errorMessage);

            result.Should().BeTrue();
            errorMessage.Should().BeNull();

            mockUplataRepo.Verify(r => r.Insert(It.IsAny<Uplata>()), Times.Once);
        }

        [Fact]
        public void Delete_Should_Fail_When_Uplata_Does_Not_Exist()
        {
            var mockUplataRepo = new Mock<IUplataRepository>();
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();

            mockUplataRepo.Setup(r => r.GetById(1))
                .Returns((Uplata?)null);

            var service = new UplataService(
                mockUplataRepo.Object,
                mockClanarinaRepo.Object,
                mockPaketRepo.Object);

            var result = service.Delete(1, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Uplata nije pronađena.");
        }
    }
}