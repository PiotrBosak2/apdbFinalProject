using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using APDB_Project.Exceptions;
using Castle.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static APDB_Project.Services.Auxilary.SecurityUtility;

namespace APDB_Project.Services
{
    public class UserService : IUserService

    {
        private readonly AdvertisementContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICalculationService _calculationService;
        private static readonly Regex Regex1 = new Regex("\\d{9}");
        private static readonly Regex Regex2 = new Regex("\\d{3}-\\d{3}-\\d{3}");

        public UserService(AdvertisementContext context,IConfiguration configuration,ICalculationService service)
        {
            _configuration = configuration;
            _context = context;
            _calculationService = service;
        }


        public ICollection<CampaignDto> ListCampaigns()
        {
            var list = new List<CampaignDto>();
            var campaigns = _context.Campaigns.Include(c => c.Banners)
                .Include(c => c.Client);

            foreach (var campaign in campaigns)
            {
                list.Add(new CampaignDto
                {
                    Client = GetClientInformation(campaign.Client),
                    Banners = GetBannersList(campaign.Banners),
                    EndDate = campaign.EndDate,
                    StartDate = campaign.StartDate,
                    FromBuilding = campaign.FromBuilding,
                    ToBuilding = campaign.ToBuilding,
                    PricePerSquareMeter = campaign.PricePerSquareMeter
                });
            }

            list.Sort((dto1, dto2) => (dto1.StartDate.CompareTo(dto2.StartDate)));
            return list;

        }


        private static ClientDto GetClientInformation(Client client)
        {
            return new ClientDto
            {
                Email = client.Email,
                Login = client.Login,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber
            };
        }

        private static ICollection<BannerDto> GetBannersList(ICollection<Banner> banners)
        {
            var collection = new List<BannerDto>();
            foreach (var banner in banners)
            {
                collection.Add(new BannerDto
                {
                    Area = banner.Area,
                    Name =  banner.Name,
                    Price = banner.Price
                });
            }

            return collection;
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
        public CampaignDto CreateCampaign(CampaignCreationDto dto)
        {
            var buildings = GetTwoBuildings(dto.FromIdBuilding, dto.ToIdBuilding);
            var divideBuildings = _calculationService.DivideBuildings(buildings);
            return null;

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

        
        private Pair<Building, Building> GetTwoBuildings(int fromId,int toId)
        {
            var first = _context.Buildings.First(b => b.IdBuilding == fromId);
            var second = _context.Buildings.First(b => b.IdBuilding == toId);
            if (first == null || second == null)
                throw new NoSuchBuildingException();
            if (first.Street != second.Street)
                throw new BuildingsOnDifferentStreetsException();
            return new Pair<Building, Building>(first,second);
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