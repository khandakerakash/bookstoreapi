using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Api.Models;
using BookStore.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Api.RequestResponse.Request
{
    public class RegistrationRequestModel
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }

    }

    public class RegistrationRequestModelValidator : AbstractValidator<RegistrationRequestModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoginService _loginService;

        public RegistrationRequestModelValidator(UserManager<ApplicationUser> _userManager,ILoginService loginService)
        {
            this._userManager = _userManager;
            _loginService = loginService;
            RuleFor(x => x.Email).NotNull().EmailAddress().MustAsync(EmailExits).WithMessage("email already exits in our system");
            RuleFor(x => x.MobileNumber).NotNull().MinimumLength(11).MustAsync(MobileExits).WithMessage("mobile number already exists in our system");
            RuleFor(x => x.Password).NotNull().MinimumLength(6);
        }

        private async Task<bool> EmailExits(string email, CancellationToken token)
        {

            if (String.IsNullOrEmpty(email))
            {
                return true;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
               return  true;
            }

            return false;
        }

        private async Task<bool> MobileExits(string mobile, CancellationToken token)
        {

            if (String.IsNullOrEmpty(mobile))
            {
                return true;
            }

            var user = await _loginService.GetUserByMobileNumber(mobile);
            if (user == null)
            {
                return true;
            }

            return false;
        }
    }
}
