using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchAPI_Demo.Models;

using Attribute = BatchAPI_Demo.Models.Attribute;

namespace BatchAPI_Demo.Repository
{
    public class CreateBatchRepository
    {
        private readonly BatchContext _batchContext;
        public CreateBatchRepository(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }
        public async Task<string> SaveBatch(BatchDetail batchDetail)
        {
            _batchContext.BatchDetails.Add(batchDetail);
            await _batchContext.SaveChangesAsync();
            return batchDetail.BatchId;
        }
        public async Task SaveFileAttributes(Attribute attribute, Files file)
        {
            _batchContext.Attributes.Add(attribute);
            _batchContext.File.Add(file);
            await _batchContext.SaveChangesAsync();
        }

        //public async Task<BatchDetail> FetchBatch(string BatchId)
        //{
        //    return await _batchContext.BatchDetails.SingleOrDefault(b => b.BatchId == BatchId);
        //}
        public  List<BatchDetail> FetchBatchDetails(string BatchId)
        {
            IEnumerable<BatchDetail> x = _batchContext.BatchDetails.Where(p => p.BatchId == BatchId).ToList();
           
            return x.ToList();
           // return (Task<BatchDetail>)x;
          //  return (Task<BatchDetail>)batchdetails;
        }

        public List<Attribute> FetchAttributes(string BatchId)
        {
            IEnumerable<Attribute> y = _batchContext.Attributes.Where(p => p.BatchId == BatchId);

            //  return (Task<Attribute>)y;
            return y.ToList();
        }

        public List<Files> FetchFiles(string BatchId)
        {
            IEnumerable<Files> z = _batchContext.File.Where(p => p.BatchId == BatchId).ToList();
            //   return (Task<Files>)z;
            return z.ToList();
        }
    }
}
