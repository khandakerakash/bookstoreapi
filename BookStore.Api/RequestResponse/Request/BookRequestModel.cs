using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace BookStore.Api.RequestResponse.Request
{
    public class BookRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class BookRequestModelValidator : AbstractValidator<BookRequestModel>
    {
        public BookRequestModelValidator()
        {
            RuleFor(x => x.Title).NotNull().MinimumLength(5);
            RuleFor(x => x.Description).NotNull().MinimumLength(10).MaximumLength(200);
            RuleFor(x => x.Price).NotNull().GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
        
        
        }
    }
}
