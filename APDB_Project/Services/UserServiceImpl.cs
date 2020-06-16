using System.Collections.Generic;
using System.Text.RegularExpressions;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using APDB_Project.Exceptions;
using static APDB_Project.Services.Auxilary.SecurityUtility;

namespace APDB_Project.Services
{
    public class UserServiceImpl : IUserService

    {
        private readonly AdvertisementContext _context;

        public UserServiceImpl(AdvertisementContext context)
        {
            _context = context;
        }

        public void RegisterUser(UserRegistrationDto dto)
        {
            if (IsRegistrationDtoValid(dto))
            {
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
            }

            else  throw new InvalidRegistrationDataException();
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

        private static bool IsPhoneNumberValid(string phone)
        {
            var regex = new Regex("\\d{9}");
            return phone != null && regex.IsMatch(phone);

        }

        private static bool IsPasswordValid(string password)
        {
            return password != null && password.Length >= 8;
        }

        
    }
    
    
}