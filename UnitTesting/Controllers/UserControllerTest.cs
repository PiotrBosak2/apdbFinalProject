using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APDB_Project.Controllers;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using APDB_Project.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;
namespace UnitTesting.Controllers
{
    
    public class UserControllerTest

    {
        


        [Fact]
        public void Nothing()
        {
            
            var context = new AdvertisementContext();
            var mock = new Mock<IUserService>();
            var controller = new UserController(mock.Object);
            var dto = new UserRegistrationDto
            {
                Email = "myEmail@gmail.com",
                Login = "myLogin",
                Password = "somePassword",
                Phone = "123123123",
                FirstName = "John",
                LastName = "Smith"
            };

            mock.Setup(m => m.RegisterUser(dto)).Returns(GetToken);
            
            
            
        }

        private static JwtSecurityToken GetToken()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "client"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fjaklsfjghasfafasfaskfasfafsafas"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return  new JwtSecurityToken
            (
                issuer: "Issuer",
                audience: "Clients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
        }
        
    }
}