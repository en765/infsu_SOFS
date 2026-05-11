using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

using FitnessStudio.Web.Controllers;
using FitnessStudio.Business.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Tests.Controllers
{
    public class PaketControllerTests
    {
        [Fact]
        public void Index_Should_Return_ViewResult()
        {
            var mockService = new Mock<IPaketService>();

            mockService.Setup(s => s.GetAll())
                .Returns(new List<Paket>());

            var controller = new PaketController(mockService.Object);

            var result = controller.Index(null);

            Assert.IsType<ViewResult>(result);
        }
    }
}