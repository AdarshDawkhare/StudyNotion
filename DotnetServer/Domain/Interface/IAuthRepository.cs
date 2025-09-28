using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interface
{
    public interface IAuthRepository
    {
        public Task<Response> AddUser(User request);

        public Task<bool> UserExist(string Email);

        public Task<User?> GetUser(string Email);
    }
}
