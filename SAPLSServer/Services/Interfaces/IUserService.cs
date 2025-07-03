using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.User;

namespace SAPLSServer.Services.Interfaces
{
    public interface IUserService
    {
        Task UpdateUserPassword(UpdateUserPasswordRequest dto);
        Task UpdateUserProfileImage(UpdateUserProfileImageRequest dto);
        Task DeleteUser(DeleteRequest request);
    }
}
