using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public TestController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        // seed the database with some dummy role and user
        public async Task Get()
        {
//            await roleManager.CreateAsync(new IdentityRole()
//            {
//                Name = "owner"
//            });
//            await roleManager.CreateAsync(new IdentityRole()
//            {
//                Name = "manager"
//            });
//            await roleManager.CreateAsync(new IdentityRole()
//            {
//                Name = "customer"
//            });

            var user = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = "akash@gmail.com",
                PhoneNumber = "01670047320",
                UserName = "akash@gmail.com"
            }, "Akash&core007");

            if (user.Succeeded)
            {
                var nowInsertedUser = await userManager.FindByEmailAsync("akash@gmail.com");
                var roleInsert = await userManager.AddToRoleAsync(nowInsertedUser, "owner");
            }
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [HttpGet("get1")]
        public string Get1()
        {
            return "hello world";
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpGet("owner")]
        public string Get2()
        {
            return "I am from Owner";
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme, Roles = "Manager")]
        [HttpGet("manager")]
        public string Get3()
        {
            return "I am from Manager";

        }
    }
}