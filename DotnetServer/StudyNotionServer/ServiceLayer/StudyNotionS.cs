using StudyNotionServer.Data;
using StudyNotionServer.RepositoryLayer;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.ServiceLayer
{
    public class StudyNotionS : IStudyNotionS
    {
        private readonly IStudyNotionRepo _repo;

        public StudyNotionS(IStudyNotionRepo repo)
        {
            _repo = repo;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();

            if (!Enum.TryParse<accountType>(request.AccountType, true, out var role))
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Message = "invalid account type",
                    RegisteredUser = null
                };
            }

            bool isUserAlreadyExists = await _repo.UserExist(new LoginUserRequest { Email = request.Email, Password = BCrypt.Net.BCrypt.HashPassword(request.Password) });

            if (isUserAlreadyExists)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Message = "email already registered",
                    RegisteredUser = null
                };
            }

            //First hash the password using "BCrypt" library and then it in the database
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //Retrieve account type
            string UserAccountType = request.AccountType.ToLower();
            accountType account = accountType.Student;
            switch(UserAccountType)
            {
                case "admin":
                    account = accountType.Admin; 
                    break;

                case "student":
                    account = accountType.Student; 
                    break;

                case "instructor":
                    account = accountType.Instructor;
                    break;

                default:
                    account = accountType.Student;
                    break;
            }

            User user = new User 
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,
                AccountType = account,
                Image = request.Image
            };

            return await _repo.RegisterUser(user);
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            return await _repo.LoginUser(request);
        }

        public async Task<bool> UserExist(LoginUserRequest request)
        {
            return await _repo.UserExist(request);
        }
    }
}
