using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.Security;
using Application.Services;
using AutoMapper;
using Domain.Entites;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Moq;
using Xunit;

namespace UnitTests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public void is_isAdmin_false_when_creating_user()
        {
            var userRepository = new Mock<IUserRepository>();
            var mapper = new Mock<IMapper>();
            var userMapper = new Mock<IUserMapper>();
            var personalRequestsService = new Mock<IPersonalRequestsService>();
            var vacationService = new Mock<IVacationService>();
            var securityHashClass = new Mock<ISecurityHashClass>();
            var context = new Mock<Context>();

            userRepository.Setup(x => x.Add(new User())).Returns(new User());


            //Arrange
            var userService = new UserService(userRepository.Object, mapper.Object, userMapper.Object, personalRequestsService.Object, vacationService.Object, securityHashClass.Object);
            CreateUserDto newUser = new CreateUserDto()
            {
                Name = "Name",
                Surname = "Surname",
                Rank = "Ratownik",
                ContractType = "Etat"
            };

            
            
            //Act

            var result = userService.AddNewUser(newUser)?.IsAdmin;

            //Assert
            Assert.False(result);
        }
    }
}
