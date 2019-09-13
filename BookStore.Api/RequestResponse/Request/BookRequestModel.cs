using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BookStore.Api.RequestResponse.Request
{
    public class BookRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile ImageUrl { get; set; }
        public long CategoryId { get; set; }
    }

    public class BookRequestModelValidator : AbstractValidator<BookRequestModel>
    {
        private readonly ILoginService _loginService;

        public BookRequestModelValidator(ILoginService loginService)
        {
            _loginService = loginService;
            RuleFor(x => x.Title).NotNull().MinimumLength(5);
            RuleFor(x => x.Description).NotNull().MinimumLength(10).MaximumLength(200);
            RuleFor(x => x.Price).NotNull().GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0).MustAsync(CategoryExits).WithMessage("this category id not in our system");
        
        
        }

        private async Task<bool> CategoryExits(long catId, CancellationToken token)
        {

            var info = await _loginService.GetCategoryById(catId);
            if (info == null)
            {
                return false;
            }

            return true;



        }
    }

    
}
