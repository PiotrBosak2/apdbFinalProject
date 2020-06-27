using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using APDB_Project.Dtos;
using APDB_Project.Utilities;

namespace APDB_Project.Services
{
    public interface IUserService
    {
        public JwtSecurityToken RegisterUser(UserRegistrationDto dto);
        public JwtSecurityToken LoginUser(UserLoginDto dto);
        public ICollection<CampaignDto> ListCampaigns();
        public CampaignDto CreateCampaign(CampaignCreationDto dto);
        public JwtSecurityToken GetNewAccessToken(Token token);

    }
}