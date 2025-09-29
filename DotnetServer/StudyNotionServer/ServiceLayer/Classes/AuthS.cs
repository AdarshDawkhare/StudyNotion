using Domain.Entities;
using Domain.Interface;
using StudyNotionServer.Module.Email;
using StudyNotionServer.ServiceLayer.Interfaces;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.ServiceLayer.Classes
{
    public class AuthS : IAuthS
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmailSender _emailSender;
        public AuthS(IAuthRepository authRepository,IEmailSender emailSender)
        {
            _authRepository = authRepository;
            _emailSender = emailSender;
        }

        public async Task<Response> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();

            if (!Enum.TryParse<accountType>(request.AccountType, true, out var role))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "invalid account type",
                };
            }

            User? user = await _authRepository.GetUser(request.Email);

            if (user != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "email already registered",
                };
            }

            //First hash the password using "BCrypt" library and then it in the database
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //Retrieve account type
            string UserAccountType = request.AccountType.ToLower();
            accountType account = accountType.Student;
            switch (UserAccountType)
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

            User newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,
                AccountType = account,
                Image = request.Image
            };

            return await _authRepository.AddUser(newUser);
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            //First check whether user is already exist or not 
            User? user = await GetUser(new LoginUserRequest { Email = request.Email });

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new LoginUserResponse { Success = true, Message = "login successful" , user = user};
                }
                else
                {
                    return new LoginUserResponse { Success = false, Message = "Invalid credentials"};
                }
            }
            else
            {
                return new LoginUserResponse { Success = false , Message = "Invalid credentials" };
            }
        }

        public async Task<User?> GetUser(LoginUserRequest request)
        {
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.Email))
                {
                    return await _authRepository.GetUser(request.Email);
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

        public async Task<bool> SendEmail(string email, string subject, string message)
        {
            return await _emailSender.SendEmailAsync(email,subject,message); 
        }
    }
}
