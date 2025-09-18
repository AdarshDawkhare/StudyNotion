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
            //First check whether user is already exist or not 
            User? user = await GetUser(new LoginUserRequest { Email = request.Email, Password = BCrypt.Net.BCrypt.HashPassword(request.Password) });

            if (user != null)
            {
                 return new LoginUserResponse { Success = true , Message = "login successful" , user = user };
            }
            else
            {
                return new LoginUserResponse 
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }
        }

        public async Task<bool> UserExist(LoginUserRequest request)
        {
            return await _repo.UserExist(request);
        }

        public async Task<User?> GetUser(LoginUserRequest request)
        {
            if (request != null)
            {
                if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.Email))
                {
                    request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                    return await _repo.GetUser(request);
                }
                else
                {
                    return null;
                }
            }
            else 
            {
                return null;
            }
        }
    }
}
