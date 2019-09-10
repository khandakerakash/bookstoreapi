using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Models;
using BookStore.Api.RequestResponse.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> CustomerRegister(RegistrationRequestModel request)
        {
            return await RegisterUser(request, "customer");
            
        }

        private async Task<ActionResult> RegisterUser(RegistrationRequestModel request, string roleName)
        {
            var user = await _userManager.CreateAsync(new ApplicationUser()
            {
                Email = request.Email,
                PhoneNumber = request.MobileNumber,
                UserName = request.Email
            }, request.Password);

            if (user.Succeeded)
            {
                var nowInsertedUser = await _userManager.FindByEmailAsync(request.Email);
                var roleInsert = await _userManager.AddToRoleAsync(nowInsertedUser, roleName);
                return Ok(new
                {
                    Success = true,
                    Message = "User created successfully"
                });
            }

            return BadRequest(user.Errors);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "owner")]
        [HttpPost("managerregister")]
        public async Task<ActionResult> ManagerRegistration(RegistrationRequestModel request)
        {
            return await RegisterUser(request, "manager");
        }
    }
}