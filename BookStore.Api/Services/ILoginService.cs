using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BookStore.Api.Contexts;
using BookStore.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookStore.Api.Services
{
   public interface ILoginService
   {

       Task<ApplicationUser> GetUserByMobileNumber(string mobileNumber);
       Task<Category> GetCategoryById(long catId);
       Task SendEmail(string toMail, string sub, string mailBody);
       Task SendEMailAdmin();
   }

    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginService(ApplicationDbContext context,IConfiguration configuration
            ,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        public Task<ApplicationUser> GetUserByMobileNumber(string mobileNumber)
        {
            var info = _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber);
            return info;
        }

        public async Task<Category> GetCategoryById(long catId)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catId);
        }

        public async Task SendEMailAdmin()
        {
            var admin = await _userManager.FindByEmailAsync("akash.cse10@gmail.com");
           await SendEmail(admin.Email, "book added", "some book added");
        }

        public async Task SendEmail(string toMail,string sub,string mailBody)
        {
            SmtpClient client = new SmtpClient
            {
                Host = _configuration["Smtp:Host"],
                Port = _configuration.GetValue<int>("Smtp:Port"),
                EnableSsl = _configuration.GetValue<bool>("Smtp:Ssl"),
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                    userName: _configuration["Smtp:Username"],
                    password: _configuration["Smtp:Password"]
                )
            };

            var mailMessage = new MailMessage(
                from: _configuration["Smtp:FromMail"],
                to: toMail
            )
            {
                Subject = sub,
                Body = mailBody,
                IsBodyHtml = true,
            };
            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
