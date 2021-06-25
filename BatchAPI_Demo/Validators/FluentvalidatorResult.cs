using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using BatchAPI_Demo.Models;
using System.Net;

namespace BatchAPI_Demo.Validators
{
    public static class FluentvalidatorResult
    {
        public static bool CheckErrors(this ValidationResult vResult, HttpStatusCode errorcode, out List<SubError> RequestErr)
        {
            RequestErr = new List<SubError>();
            if (vResult.Errors.Any(i => i.ErrorCode.Equals(errorcode.ToString())))
            {
                RequestErr = vResult.Errors.Where(i => i.ErrorCode.Equals(errorcode.ToString()))
                    .Select(e => new SubError { Source = e.ToString(), Description = e.ToString() })
                    .ToList();
                return true;
            }
            return false;
        }

        public static bool BadRequestError(this ValidationResult vResult, out List<SubError> BadRequestErr)
        {
            BadRequestErr = new List<SubError>();
            return CheckErrors(vResult, HttpStatusCode.BadRequest, out BadRequestErr);
        }

        public static bool NotFoundError(this ValidationResult vResult, out List<SubError> NotFoundErr)
        {
            NotFoundErr = new List<SubError>();
            return CheckErrors(vResult, HttpStatusCode.NotFound, out NotFoundErr);

        }
        public static bool ForbiddenError(this ValidationResult vResult, out List<SubError> ForbiddenErr)
        {
            ForbiddenErr = new List<SubError>();
            return CheckErrors(vResult, HttpStatusCode.Forbidden, out ForbiddenErr);

        }
    }
}
