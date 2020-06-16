using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using APDB_Project.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static APDB_Project.Services.Auxilary.SecurityUtility;

namespace APDB_Project.Services
{
    public class UserServiceImpl : IUserService

    {
        private readonly AdvertisementContext _context;
        private readonly IConfiguration _configuration;
        private static readonly Regex Regex1 = new Regex("\\d{9}");
        private static readonly Regex Regex2 = new Regex("\\d{3}-\\d{3}-\\d{3}");

        public UserServiceImpl(AdvertisementContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public JwtSecurityToken RegisterUser(UserRegistrationDto dto)
        {
            if (IsRegistrationDtoValid(dto))
            {
                if (IsLoginTaken(dto.Login))
                    throw  new LoginAlreadyTakenException();
                    
                var securePassword = SecurePassword(dto.Password);
                _context.Clients.Add(new Client
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Campaigns = new List<Campaign>(),
                    Email = dto.Email,
                    Login = dto.Login,
                    PhoneNumber = dto.Phone,
                    Password = securePassword
                });
                _context.SaveChanges();
                return CreateToken();

            }

            else throw new InvalidRegistrationDataException();
        }

        public JwtSecurityToken LoginUser(UserLoginDto dto)
        {
             var client = _context.Clients.FirstOrDefault(c => c.Login == dto.Login);
             if (client == null)
             {
                 throw new InvalidLoginException();
             }

             if (IsPasswordCorrect(dto.Password, client.Password))
                 return CreateToken();

             else throw new InvalidPasswordException();
        }

        private JwtSecurityToken CreateToken()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "client"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            Console.WriteLine(_configuration["SecretKey"]);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           return  new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
           
        }
        
        
        

       
        private static bool IsRegistrationDtoValid(UserRegistrationDto dto)
        {
            return dto?.FirstName != null &&
                   dto.LastName != null &&
                   dto.Email != null &&
                   dto.Login != null &&
                   IsPhoneNumberValid(dto.Phone) &&
                   IsPasswordValid(dto.Password);
        }

        private  bool IsLoginTaken(string login)
        {
            return _context.Clients.Any(c => c.Login == login);
        }

        private static bool IsPhoneNumberValid(string phone)
        {
            return phone != null && (
                Regex1.IsMatch(phone) ||
                Regex2.IsMatch(phone));
        }

        private static bool IsPasswordValid(string password)
        {
            return password != null && password.Length >= 8;
        }
    }
}