using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BatchAPI_Demo.Models;
using BatchAPI_Demo.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using BatchAPI_Demo.Repository;
using BatchAPI_Demo.Validators;

namespace BatchAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        //  private readonly BatchContext _BatchContext;
        private readonly IBlobStorage _blobService;
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService, IBlobStorage blobService)
        {
            //  _BatchContext = BatchContext;
            _batchService = batchService;
            _blobService = blobService;
        }

        // GET api/<BatchController>/5
        [HttpGet]
        [Route("/batch/{batchid}")]       
        public async Task<IActionResult> Batch(string batchid)
        {
            ResponseService clsrespserv = new ResponseService();

            //addd a list
            var getbatchdetails = await _batchService.FetchBatchDetails(batchid);
            var a = getbatchdetails.GetType();
            if (a.Name == "BadRequestObjectResult")
            {
                return new BadRequestObjectResult(getbatchdetails);
            }
            else if (getbatchdetails != null)
            {
                //  return clsrespserv.GetOKResponse(getbatchdetails).ToString();
                // return HttpStatusCode.OK.ToString();
                return new OkObjectResult(getbatchdetails);
            }
            else
            {
                var error = new List<SubError>
                        {
                            new SubError()
                            {
                                Source = "BatchId",
                                Description = "Batch Details Not Found"
                            }
                        };
                return clsrespserv.NotFoundRes(error);
                //List<SubError> errors = new List<SubError>();
                //// return HttpStatusCode.BadRequest;
                //return clsrespserv.BadResponse(errors);
            }
        }


        [HttpPost]
        [Route("/batch")]
        //[Produces("application/json")]
        public async Task<IActionResult> Batch([FromBody] ReqBatch batchReq)
        {
            ResponseService clsrespservice = new ResponseService();
            if (batchReq == null)
            {
                var error = new List<SubError>
                        {
                            new SubError()
                            {
                                Source = "Batch Request Details",
                                Description = "Invalid Request"
                            }
                        };
                return clsrespservice.BadResponse(error);
            }
            //validate request details by fluent validation
            var validationResult = await _batchService.ValidateCreateBatch(batchReq);
            if (validationResult != null)
            {
                List<SubError> err;
                if (validationResult.BadRequestError(out err))
                {
                    return clsrespservice.BadResponse(err);
                }
            }
            Batch batch = await _batchService.CreateBatch(batchReq);
            if (batch != null)
            {
                var BlobResult = _blobService.UploadFileToBlob(batch.BatchId);
            }
            return Created("", new Batch { BatchId = batch.BatchId });

        }

        [HttpPost]
        [Route("{batchId}/{filename}")]
        //    [Produces("application/json")]
        public async Task<IActionResult> Post_File(string batchId, string filename, [FromHeader(Name = "X-MIME-Type")] string MIME, string ContentSize)
        {
            //   BlobStorageService clsBlob = new BlobStorageService();
            ResponseService clsRespservice = new ResponseService();
            //fetch batch details

            var postbatch_file = await _batchService.PostBatchDetails(batchId, filename, ContentSize);
            var a = postbatch_file.GetType();
            if (a.Name == "BadRequestObjectResult")
            {
                return new BadRequestObjectResult(postbatch_file);
            }
            else if (postbatch_file != null)
            {
                // string strContainerName =  batchId;                                     
                var BlobResult = _blobService.UploadFileToBlob(batchId);
                return new OkObjectResult(postbatch_file);
                // return StatusCodes.Status201Created.ToString();
            }
            else
            {
                var error = new List<SubError>
                        {
                            new SubError()
                            {
                                Source = "BatchId",
                                Description = "Batch Details Not Found Exception"
                            }
                        };
                return clsRespservice.NotFoundRes(error);
                // return new BadRequestObjectResult(postbatch_file);
                //StatusCode(StatusCodes.Status404NotFound);
            }
        }

    }

}

