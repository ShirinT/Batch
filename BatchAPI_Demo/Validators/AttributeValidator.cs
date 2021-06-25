
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using BatchAPI_Demo.Models;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BatchAPI_Demo.Validators
{
    public class AttributeValidator : AbstractValidator<SubAttribute>
    {
        public AttributeValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty()
                .WithMessage("Key is required")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString()); 
            RuleFor(x => x.Value).NotNull().NotEmpty()
                .WithMessage("Value is required")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());
            //etc
        }
    }

}
