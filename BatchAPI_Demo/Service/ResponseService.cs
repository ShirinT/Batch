using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BatchAPI_Demo.Models;

namespace BatchAPI_Demo.Service
{
    public class ResponseService
    {
        public IActionResult BadResponse(List<SubError> errors = null)
        {
            return new BadRequestObjectResult(new Error
            {
                CorrelationID = 403,
                Errors = errors
            });
        }
        public IActionResult NotFoundRes(List<SubError> errors = null)
        {
            return new NotFoundObjectResult(new Error
            {
                CorrelationID = 400,
                Errors = errors
            });
        }

        public IActionResult PostOKResponse(Batch output)
        {
          //  return new HttpStatusCode.Ok;
            return new OkObjectResult(output);
        }

        public IActionResult GetOKResponse(ResBatch output)
        {
            return new OkObjectResult(output);
        }

       
        //public IActionResult ReturnBatchResponse(ResBatch res, List<SubError> errors = null)
        //{
        //    switch (res.StatusCode)
        //    {
        //        case HttpStatusCode.OK:
        //            //  return StatusCode(StatusCodes.Status201Created);
        //            return Ok(res.BatchId);
        //        case HttpStatusCode.BadRequest:
        //            //  return StatusCode(StatusCodes.Status400BadRequest);
        //            return BadResponse(errors);
        //        case HttpStatusCode.Unauthorized:
        //            return StatusCode(StatusCodes.Status401Unauthorized);
        //        case HttpStatusCode.Forbidden:
        //            return StatusCode(StatusCodes.Status403Forbidden);
        //        case HttpStatusCode.NotFound:
        //            return StatusCode(StatusCodes.Status404NotFound);
        //        case HttpStatusCode.Gone:
        //            return StatusCode(StatusCodes.Status410Gone);
        //        default:
        //            return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //}
    }
}
