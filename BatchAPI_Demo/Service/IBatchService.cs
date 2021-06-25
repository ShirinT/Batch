using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using BatchAPI_Demo.Models;

namespace BatchAPI_Demo.Service
{
    public interface IBatchService
    {
        Task<Batch> PostBatchDetails(string batchId, string filename, string ContentSize);
        Task<ResBatch> FetchBatchDetails(string batchid);

        Task<ValidationResult> ValidateCreateBatch(ReqBatch reqBatch);
        Task<Batch> CreateBatch(ReqBatch reqBatch);
    }
}
