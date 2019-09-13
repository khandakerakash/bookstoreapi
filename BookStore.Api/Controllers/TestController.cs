using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using BookStore.Api.Contexts;
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
        private readonly ApplicationDbContext _context;

        public TestController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        [HttpGet]
        // seed the database with some dummy role and user
        public async Task Get()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "owner"
            });
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "manager"
            });
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "customer"
            });

            var user = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = "akash.cse10@gmail.com",
                PhoneNumber = "01670047320",
                UserName = "akash.cse10@gmail.com"
            }, "Akash&core007");
            
            if (user.Succeeded)
            {
            
                var nowInsertedUser = await userManager.FindByEmailAsync("akash.cse10@gmail.com");
                var roleInsert = await userManager.AddToRoleAsync(nowInsertedUser, "Owner");
            }

            var user1 = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = "tapos.aa@gmail.com",
                PhoneNumber = "01684750869",
                UserName = "tapos.aa@gmail.com"
            }, "Akash&core007");

            if (user1.Succeeded)
            {

                var nowInsertedUser1 = await userManager.FindByEmailAsync("tapos.aa@gmail.com");
                var roleInsert1 = await userManager.AddToRoleAsync(nowInsertedUser1, "Manager");
            }

            var testBlogPosts = new Faker<Category>()
                .RuleFor(bp => bp.Name, f => f.Name.FullName())
                .RuleFor(bp => bp.Description, f => f.Lorem.Paragraphs());

            var listData = testBlogPosts.Generate(100);
          await  _context.Categories.AddRangeAsync(listData);
          await  _context.SaveChangesAsync();

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