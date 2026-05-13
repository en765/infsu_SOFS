using Xunit;
using Moq;
using FluentAssertions;

using FitnessStudio.Business.Services;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Tests.Services
{
    public class ClanarinaServiceTests
    {
        [Fact]
        public void Create_Should_Succeed_When_Data_Is_Valid()
        {
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockClanRepo = new Mock<IClanRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();
            var mockUplataRepo = new Mock<IUplataRepository>();

            mockClanRepo.Setup(r => r.GetById(1))
                .Returns(new Clan { ClanId = 1 });

            mockPaketRepo.Setup(r => r.GetById(1))
                .Returns(new Paket { PaketId = 1, Cijena = 100 });

            var service = new ClanarinaService(
                mockClanarinaRepo.Object,
                mockClanRepo.Object,
                mockPaketRepo.Object,
                mockUplataRepo.Object);

            var clanarina = new Clanarina
            {
                ClanId = 1,
                PaketId = 1,
                DatumPocetka = DateTime.Today,
                DatumZavrsetka = DateTime.Today.AddMonths(1)
            };

            var result = service.Create(clanarina, out string? errorMessage);

            result.Should().BeTrue();
            errorMessage.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Fail_When_EndDate_Is_Before_StartDate()
        {
            var mockClanarinaRepo = new Mock<IClanarinaRepository>();
            var mockClanRepo = new Mock<IClanRepository>();
            var mockPaketRepo = new Mock<IPaketRepository>();
            var mockUplataRepo = new Mock<IUplataRepository>();

            mockClanRepo.Setup(r => r.GetById(1))
                .Returns(new Clan { ClanId = 1 });

            mockPaketRepo.Setup(r => r.GetById(1))
                .Returns(new Paket { PaketId = 1, Cijena = 100 });

            var service = new ClanarinaService(
                mockClanarinaRepo.Object,
                mockClanRepo.Object,
                mockPaketRepo.Object,
                mockUplataRepo.Object);

            var clanarina = new Clanarina
            {
                ClanId = 1,
                PaketId = 1,
                DatumPocetka = DateTime.Today,
                DatumZavrsetka = DateTime.Today.AddDays(-1)
            };

            var result = service.Create(clanarina, out string? errorMessage);

            result.Should().BeFalse();
            errorMessage.Should().Be("Datum završetka mora biti nakon datuma početka.");
        }
    }
}