using TravellerTicket.Core.Context;

namespace TravellerTicket.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(LogInModel model);
        Task<string> AddRoleAsync(AddRoleModel model);

    }
}


