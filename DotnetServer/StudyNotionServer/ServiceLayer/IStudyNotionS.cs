using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.ServiceLayer
{
    public interface IStudyNotionS
    {
        public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request); 

        public Task<LoginUserResponse> LoginUser(LoginUserRequest request);
        
        public Task<bool> UserExist(LoginUserRequest request);
        
        public Task<User?> GetUser(LoginUserRequest request);
    }
}
