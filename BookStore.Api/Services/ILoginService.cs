using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Contexts;
using BookStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Services
{
   public interface ILoginService
   {

       Task<ApplicationUser> GetUserByMobileNumber(string mobileNumber);
   }

    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ApplicationUser> GetUserByMobileNumber(string mobileNumber)
        {
            var info = _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber);
            return info;
        }
    }
}
