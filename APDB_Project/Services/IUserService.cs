using APDB_Project.Dtos;

namespace APDB_Project.Services
{
    public interface IUserService
    {
        public void RegisterUser(UserRegistrationDto dto);
    }
}