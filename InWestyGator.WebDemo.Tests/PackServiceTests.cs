using Moq;
using InWestyGator.WebDemo.Core.Contracts;
using InWestyGator.WebDemo.Core.Models;
using InWestyGator.WebDemo.Business.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace InWestyGator.WebDemo.Tests.Services
{
    [TestClass]
    public class PackServiceTests
    {
        private Mock<IPackRepository> _packRepositoryMock;
        private PackService _packService;

        [TestInitialize]
        public void TestInitialize()
        {
            _packRepositoryMock = new Mock<IPackRepository>();
            _packService = new PackService(_packRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetPackWithHierarchyAsync_ShouldReturnPackHierarchy()
        {
            // Arrange
            var rootPack = new Pack { Id = "1", PackName = "Root Pack" };
            var childPack = new Pack { Id = "2", PackName = "Child Pack" };
            rootPack.Children = new List<Pack> { childPack };
            childPack.Parents = new List<Pack> { rootPack };

            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync(rootPack);
            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("2"))
                .ReturnsAsync(childPack);

            // Act
            var result = (await _packService.GetPackWithHierarchyAsync("1")).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("2", result[1].Id);
            Assert.AreEqual(1, result[0].ChildPackIds.Count);
            Assert.AreEqual("2", result[0].ChildPackIds[0]);
        }

        [TestMethod]
        public async Task GetPackWithHierarchyAsync_ShouldReturnMultiLayeredHierarchy()
        {
            // Arrange
            var rootPack = new Pack { Id = "1", PackName = "Root Pack" };
            var childPack = new Pack { Id = "2", PackName = "Child Pack" };
            var grandChildPack = new Pack { Id = "3", PackName = "Grandchild Pack" };
            rootPack.Children = new List<Pack> { childPack };
            childPack.Children = new List<Pack> { grandChildPack };
            childPack.Parents = new List<Pack> { rootPack };
            grandChildPack.Parents = new List<Pack> { childPack };

            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync(rootPack);
            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("2"))
                .ReturnsAsync(childPack);
            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("3"))
                .ReturnsAsync(grandChildPack);

            // Act
            var result = (await _packService.GetPackWithHierarchyAsync("1")).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("2", result[1].Id);
            Assert.AreEqual("3", result[2].Id);
            Assert.AreEqual(1, result[0].ChildPackIds.Count);
            Assert.AreEqual("2", result[0].ChildPackIds[0]);
            Assert.AreEqual(1, result[1].ChildPackIds.Count);
            Assert.AreEqual("3", result[1].ChildPackIds[0]);
        }

        [TestMethod]
        public async Task GetPaginatedPacksAsync_ShouldReturnFirstPage()
        {
            // Arrange
            var packs = new List<Pack>
            {
                new Pack { Id = "1", PackName = "Pack 1" },
                new Pack { Id = "2", PackName = "Pack 2" },
                new Pack { Id = "3", PackName = "Pack 3" },
                new Pack { Id = "4", PackName = "Pack 4" }
            }.AsQueryable();

            _packRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Func<IQueryable<Pack>, IQueryable<Pack>>>()))
                .ReturnsAsync((Func<IQueryable<Pack>, IQueryable<Pack>> query) => query(packs).ToList());

            // Act
            var result = (await _packService.GetPaginatedPacksAsync(1, 2)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("2", result[1].Id);
        }

        [TestMethod]
        public async Task GetPaginatedPacksAsync_ShouldReturnThirdPage()
        {
            // Arrange
            var packs = new List<Pack>
            {
                new Pack { Id = "1", PackName = "Pack 1" },
                new Pack { Id = "2", PackName = "Pack 2" },
                new Pack { Id = "3", PackName = "Pack 3" },
                new Pack { Id = "4", PackName = "Pack 4" },
                new Pack { Id = "5", PackName = "Pack 5" },
                new Pack { Id = "6", PackName = "Pack 6" }
            }.AsQueryable();

            _packRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Func<IQueryable<Pack>, IQueryable<Pack>>>()))
                .ReturnsAsync((Func<IQueryable<Pack>, IQueryable<Pack>> query) => query(packs).ToList());

            // Act
            var result = (await _packService.GetPaginatedPacksAsync(3, 2)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("5", result[0].Id);
            Assert.AreEqual("6", result[1].Id);
        }

        [TestMethod]
        public async Task AddPackAsync_ShouldAddPack()
        {
            // Arrange
            var newPack = new Pack { Id = "5", PackName = "New Pack", ChildPackIds = new List<string> { "2" } };
            var childPack = new Pack { Id = "2", PackName = "Child Pack" };

            _packRepositoryMock.Setup(repo => repo.GetByIdAsync("2"))
                .ReturnsAsync(childPack);

            // Act
            await _packService.AddPackAsync(newPack);

            // Assert
            _packRepositoryMock.Verify(repo => repo.AddAsync(newPack), Times.Once);
            Assert.AreEqual(1, newPack.Children.Count);
            Assert.AreEqual("2", newPack.Children.First().Id);
        }
    }
}
