using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using BatchAPI_Demo.Models;
using BatchAPI_Demo.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using FluentValidation.Results;

namespace BatchAPI_Demo.Validators
{
    public interface IReqBatchValidator
    {
        Task<ValidationResult> ValidateBatchdetails(ReqBatch req);
    }

    public class ReqBatchValidator : AbstractValidator<ReqBatch>, IReqBatchValidator
    {

        public ReqBatchValidator()
        {
            RuleFor(x => x.Businessunit).NotEmpty().NotNull().
                WithMessage("BusinessUnit is required!")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());
            RuleFor(x => x.ExpiryDate).NotEmpty()
                .Must(date => date != default(DateTime))
                .WithMessage("ExpiryDate is not in correct format!")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());

            RuleFor(x => x.Acl.ReadUsers).NotEmpty().NotNull()
                .When(y => y.Acl != null && y.Acl.ReadUsers != null)
                .WithMessage("ReadUsers cannot be blank")
                 .WithErrorCode(HttpStatusCode.BadRequest.ToString());

            RuleFor(x => x.Acl.Readgroups).NotEmpty().NotNull()
                .When(y => y.Acl != null && y.Acl.Readgroups != null)
                .WithMessage("Readgroups cannot be blank")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());

            RuleFor(x => x.Attribute).NotEmpty().NotNull()
                .When(y => y.Attribute != null && y.Attribute.All(z => !string.IsNullOrEmpty(z.Key) && !string.IsNullOrEmpty(z.Value)))
                .WithMessage("Attribute Key & Value cannot be blank")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString());


            //   RuleFor(x => x.Acl).SetValidator(new AttributeValidator());
            ///  RuleFor(x => x.Attribute).SetValidator(new AttributeValidator());
        }

        public Task<ValidationResult> ValidateBatchdetails(ReqBatch req)
        {
        return ValidateAsync(req);
        }
    }

}
        

