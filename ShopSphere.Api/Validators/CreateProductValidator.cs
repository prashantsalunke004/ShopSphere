using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopSphere.API.Data;
using ShopSphere.API.DTOs;

namespace ShopSphere.API.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        
        public CreateProductValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Is Required").MaximumLength(100);
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price Can Not Be Empty").GreaterThan(0).WithMessage("Price Should Be Greater Than Zero");
            RuleFor(x => x.Stock).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Stock Can Not Be Zero Or Empty");
            RuleFor(x => x.CategoryId).GreaterThan(0);
           
        }

    }
}
