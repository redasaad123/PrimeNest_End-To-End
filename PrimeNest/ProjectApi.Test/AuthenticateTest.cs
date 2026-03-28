using Core.AuthenticationDTO;
using Core.Interfaces;
using Core.Models;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ProjectAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectApi.Test
{
    public class AuthenticateTest
    {

        [Fact]
        public async Task RegisterUser_ReturnsOk_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Name = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(
                mockUserStore.Object, null, null, null, null, null, null, null, null
            );
            userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                           .ReturnsAsync(IdentityResult.Success);

            var passwordHasher = new PasswordHasher<AppUser>();

            var jwtSettings = new JWTSettings
            {
                key = "TestKey1234567890TestKey1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                DurationInDays = 60
            };
            var optionsMock = new Mock<IOptions<JWTSettings>>();
            optionsMock.Setup(o => o.Value).Returns(jwtSettings);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(r => r.RoleExistsAsync("User")).ReturnsAsync(true);

            var userRepoMock = new Mock<IGenericRepository<AppUser>>();
            var unitOfWorkMock = new Mock<IUnitOfWork<AppUser>>();
            unitOfWorkMock.Setup(u => u.Entity).Returns(userRepoMock.Object);

            var authMock = new Mock<IAuthentication>();
            authMock.Setup(a => a.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync(new AuthenticateDTO
                {
                    IsAuthenticated = true,
                    Message = "User registered successfully"
                });

            var controller = new AuthenticateController(
                userManagerMock.Object,
                passwordHasher,
                optionsMock.Object,
                roleManagerMock.Object,
                unitOfWorkMock.Object,
                authMock.Object
            );

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var authResult = Assert.IsType<AuthenticateDTO>(okResult.Value);
            Assert.Equal("User registered successfully", authResult.Message);
        }


        [Fact]
        public async Task LoginUser_ReturnsOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDto = new LogInDTo
            {
                Email = "reda@gmail.com",
                Password = "reda"
            };

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(
                mockUserStore.Object, null, null, null, null, null, null, null, null
            );

            var passwordHasher = new PasswordHasher<AppUser>();

            var jwtSettings = new JWTSettings
            {
                key = "TestKey1234567890TestKey1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                DurationInDays = 60
            };
            var optionsMock = new Mock<IOptions<JWTSettings>>();
            optionsMock.Setup(o => o.Value).Returns(jwtSettings);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            var userRepoMock = new Mock<IGenericRepository<AppUser>>();
            var unitOfWorkMock = new Mock<IUnitOfWork<AppUser>>();
            unitOfWorkMock.Setup(u => u.Entity).Returns(userRepoMock.Object);

            var authMock = new Mock<IAuthentication>();
            authMock.Setup(a => a.LoginAsync(It.IsAny<LogInDTo>()))
             .ReturnsAsync(new AuthenticateDTO
             {
                 IsAuthenticated = true,
                 Message = "Login successful",
                 Email = loginDto.Email,
                 Name = "testuser",
                 RefreshToken = "dummy-refresh-token", // Add this
                 RefreshTokenExpiration = DateTime.UtcNow.AddDays(7) // And this
             });

            var controller = new AuthenticateController(
                userManagerMock.Object,
                passwordHasher,
                optionsMock.Object,
                roleManagerMock.Object,
                unitOfWorkMock.Object,
                authMock.Object
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var authResult = Assert.IsType<AuthenticateDTO>(okResult.Value);
            Assert.True(authResult.IsAuthenticated);
            Assert.Equal("Login successful", authResult.Message);
            Assert.Equal(loginDto.Email, authResult.Email);

        }












    }
}
