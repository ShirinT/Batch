using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using BatchAPI_Demo.Models;
//using BatchAPI_Demo.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using FluentValidation.Results;
//using Microsoft.Extensions.Primitives;

namespace BatchAPI_Demo.Validators
{
    public interface IBatchValidator
    {
       // Task<ValidationResult> ValidateBatch(Batch BatchId);
    }
    public class BatchValidator : AbstractValidator<Batch>, IBatchValidator
    {
        public BatchValidator()
        {
            RuleFor(x => x.BatchId)
                .NotEmpty().NotNull()
                .WithMessage("BatchId cannot be empty!")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());
            //RuleFor(x => x.BatchId)
            //    .NotEmpty().
            //    WithMessage("BatchId  does not exist!")
            //    .WithErrorCode(HttpStatusCode.BadRequest.ToString())
            //    .When(x=> !StringValues.IsNullOrEmpty(x.BatchId) && 
            //   RuleFor(x => x.Acl).SetValidator(new AttributeValidator());
            ///  RuleFor(x => x.Attribute).SetValidator(new AttributeValidator());
        }
    }
}
