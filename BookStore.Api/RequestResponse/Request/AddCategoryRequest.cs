using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Models;
using FluentValidation;

namespace BookStore.Api.RequestResponse.Request
{

    public class AddCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
    {
        public AddCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(4);
            RuleFor(x => x.Description).NotNull();
        }
    }
}
