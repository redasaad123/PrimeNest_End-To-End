using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectApi.Controllers;
using ProjectApi.DTOS;
using ProjectApi.FactoryImplementation;
using ProjectApi.NewFolder;
using Xunit;

namespace ProjectApi.Tests.Controllers
{
    public class PropertyControllerTest
    {
        [Fact]
        public async Task GetAllProperty_ReturnsOk_WhenPropertiesExist()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Id = "90e395ed-8552-401b-920f-6a0c7868d965", user = "reda", Type = "villa", TypeContract = "rent" , MainPhoto = "Screenshot 2025-05-12 134611.png", Price = "5323" },
                new Property { Id = "e139db19-166a-4817-8359-ee3ece58b7a9", user = "reda", Type = "villa", TypeContract = "rent" , MainPhoto = "Screenshot 2025-05-11 000134.png", Price = "356"}
            };
            var users = new List<AppUser>
            {
                new AppUser { Id = "2d16306c-4881-4685-b2fb-a319ee7b6ca2", Name = "reda" }
                
            }.AsQueryable();

            var propertyRepoMock = new Mock<IGenericRepository<Property>>();
            propertyRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(properties);

            var unitOfWorkMock = new Mock<IUnitOfWork<Property>>();
            unitOfWorkMock.Setup(u => u.Entity).Returns(propertyRepoMock.Object);

            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(u => u.Users).Returns(users);

            var controller = new ProjectApi.Controllers.PropertyController(
                null, null, unitOfWorkMock.Object, userManagerMock.Object);

            // Act
            var result = await controller.GetAllProperty();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoList = Assert.IsAssignableFrom<IEnumerable<GetPropertyDTO>>(okResult.Value);
            Assert.Equal(2, dtoList.Count());
            Assert.Contains(dtoList, d => d.OwnerName == "reda");
            
        }

        [Fact]
        public async Task GetAllProperty_ReturnsNotFound_WhenNoPropertiesExist()
        {


            // Arrange
            var propertyRepoMock = new Mock<IGenericRepository<Property>>();
            propertyRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Property>());

            var unitOfWorkMock = new Mock<IUnitOfWork<Property>>();
            unitOfWorkMock.Setup(u => u.Entity).Returns(propertyRepoMock.Object);

            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(u => u.Users).Returns(new List<AppUser>().AsQueryable());

            var controller = new ProjectApi.Controllers.PropertyController(
                null, null, unitOfWorkMock.Object, userManagerMock.Object);

            // Act
            var result = await controller.GetAllProperty();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not Found Any Property", notFoundResult.Value);
        }


        [Fact]
        public async Task GetByIdProperty_ReturnsOk_WhenPropertiesExist()
        {
            
                // Arrange
                var propertyId = "90e395ed-8552-401b-920f-6a0c7868d965";
                var property = new Property
                {
                    Id = propertyId,
                    user = "reda",
                    Type = "villa",
                    Price = "5323"
                };

                var user = new AppUser
                {
                    Id = "2d16306c-4881-4685-b2fb-a319ee7b6ca2",
                    Name = "reda"
                };

                var propertyRepoMock = new Mock<IGenericRepository<Property>>();
                propertyRepoMock.Setup(r => r.GetAsync(propertyId)).ReturnsAsync(property);

                var unitOfWorkMock = new Mock<IUnitOfWork<Property>>();
                unitOfWorkMock.Setup(u => u.Entity).Returns(propertyRepoMock.Object);

                var userManagerMock = new Mock<UserManager<AppUser>>(
                    Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
                userManagerMock.Setup(u => u.Users).Returns(new List<AppUser> { user }.AsQueryable());

                var controller = new ProjectApi.Controllers.PropertyController(null, null, unitOfWorkMock.Object, userManagerMock.Object);

                // Act
                var result = await controller.GetPropertyById(propertyId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var dto = Assert.IsType<GetPropertyDTO>(okResult.Value);
                Assert.Equal(dto.Id,property.Id);
                Assert.Equal(dto.OwnerName,property.user);
                Assert.Equal(dto.Price,property.Price);
                Assert.Equal(dto.Type,property.Type);


        }

       




    }
}
