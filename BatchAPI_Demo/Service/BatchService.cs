using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchAPI_Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Attribute = BatchAPI_Demo.Models.Attribute;
using FluentValidation.Results;
using BatchAPI_Demo.Validators;
using Microsoft.AspNetCore.Mvc;
using BatchAPI_Demo.Repository;
namespace BatchAPI_Demo.Service
{
    //  public class BatchService
    public class BatchService : IBatchService
    {

        private readonly BatchContext _BatchContext;
        private readonly IReqBatchValidator _validBatch;
        private readonly CreateBatchRepository _createBatchRep;
       
        public BatchService(BatchContext BatchContext, IReqBatchValidator validBatch, CreateBatchRepository createBatchRep)
        {
            _BatchContext = BatchContext;
            _validBatch = validBatch;
            _createBatchRep = createBatchRep;
        }

        public Task<ValidationResult> ValidateCreateBatch(ReqBatch req)
        {
            return _validBatch.ValidateBatchdetails(req);
        }
        //  public Task<ValidationResult> ValidateCreateBatch(ReqBatch req) => _validBatch.ValidateBatchdetails(req);
        public async Task<Batch> CreateBatch(ReqBatch reqBatch)
        {
            DateTime currentdate = DateTime.Now;
            //create guid
            
            Guid objGuid = Guid.NewGuid();
            string Batchid = objGuid.ToString();

            BatchDetail objBatch = new BatchDetail
            {
                BatchId = Batchid,
                Status = "Initiate",
                BusinessUnit = reqBatch.Businessunit,
                BatchPublishedDate = currentdate,
                ExpiryDate = reqBatch.ExpiryDate,
            };

            //save attribute data
            List<SubAttribute> lstattr = new List<SubAttribute>();
            lstattr = reqBatch.Attribute;
            for (int i = 0; i < lstattr.Count; i++)
            {
                Attribute attr = new Attribute()
                {
                    Key = lstattr[i].Key,
                    Value = lstattr[i].Value,
                    BatchId = Batchid,
                };
                Files objfile = new Files()
                {
                    Filename = "File2.txt",
                    Filesize = "270",
                    MimeType = "",
                    Hash = "###",
                    Key = lstattr[i].Key,
                    Value = lstattr[i].Value,
                    BatchId = Batchid,
                };

                 await _createBatchRep.SaveFileAttributes(attr,objfile);
            }
           

            //save acl table details
            SubAcl objAcl = new SubAcl();
            objAcl = reqBatch.Acl;

            List<string> lstRdUsers = new List<string>();
            lstRdUsers = objAcl.ReadUsers;

            for (int i = 0; i < lstRdUsers.Count; i++)
            {               
                var acl1 = new Acl
                {
                    ReadUsers = lstRdUsers[i].ToString(),
                    BatchId = Batchid
                };
                _BatchContext.Acls.Add(acl1);
                _BatchContext.SaveChanges();
                _BatchContext.Entry<Acl>(acl1).State = EntityState.Detached;
            }
            List<string> lstRdGroups = new List<string>();
            lstRdGroups = objAcl.Readgroups;

            for (int i = 0; i < lstRdGroups.Count; i++)
            {                
                var acl2 = new Acl
                {
                    ReadGroups = lstRdGroups[i].ToString(),
                    BatchId = Batchid
                };
                _BatchContext.Acls.Add(acl2);
            }

            string strBatchId = await _createBatchRep.SaveBatch(objBatch);

           // Batch objResbatch = new Batch();
            return new Batch() { BatchId = strBatchId };
        }
        
        //    public async Task<ResBatch> GetBatchRequest()
        public async Task<Batch> PostBatchDetails(string batchId, string filename, string ContentSize)
        {
         //   SubError objErr = new SubError();
            ResponseService clsRespService = new ResponseService();
            if (batchId != null)
            {
                var batchList = (from Batch in _BatchContext.BatchDetails
                                 where Batch.BatchId == batchId
                                 select new
                                 {
                                     Batch.BatchId,
                                     Batch.BusinessUnit,
                                     Batch.BatchPublishedDate,
                                     Batch.Status,
                                     Batch.ExpiryDate
                                 }).ToList();

                if (batchList.Count == 0)
                {
                    var error = new List<SubError>
                        {
                            new SubError()
                            {
                                Source = "BatchId",
                                Description = "BatchId does not exist"
                            }
                        };
                    //  return objErr.errors.Add(error);
                  //  return clsRespService.BadResponse(error);
                }
            }

            //check if both condition will satisfy the requirement or else add filesize condition later
            var fileList = (from Lfile in _BatchContext.File
                            where Lfile.Filename == filename && Lfile.Filesize == ContentSize
                            select new
                            {
                                Lfile.Filename,
                                Lfile.Filesize,
                                Lfile.MimeType,
                                Lfile.Hash,
                                Lfile.Key,
                                Lfile.Value
                            }).ToList();

            if (fileList.Count == 0)
            {
                var error = new List<SubError>
                        {
                            new SubError()
                            {
                                Source = "FileName",
                                Description = "Bad File name"
                            }
                        };
            //    return clsRespService.BadResponse(error);
            }
            //  ResBatch objBD = new ResBatch();
            Batch objBD = new Batch();
            objBD.BatchId = batchId;
            //    return clsRespService.PostOKResponse(objBD);
            return null;
        }

        public async Task<ResBatch> FetchBatchDetails(string batchid)
        {            
            var batchDetail =  _createBatchRep.FetchBatchDetails(batchid);
            var attribute =  _createBatchRep.FetchAttributes(batchid);
            var files =  _createBatchRep.FetchFiles(batchid);

            return await BindBatchResponse(batchDetail, attribute, files);            
        }


        public Task<ResBatch> BindBatchResponse(List<BatchDetail> batchDetail, List<Attribute> attribute, List<Files> files)
        {
          //  List<ResBatch> listRes = new List<ResBatch>();
            ResBatch objRes = new ResBatch();
            foreach (var item in batchDetail)
            {
                objRes.BatchId = item.BatchId;
                objRes.BusinessUnit = item.BusinessUnit.Trim();
                objRes.BatchPublishedDate = item.BatchPublishedDate;
                objRes.Status = item.Status.Trim();
                objRes.ExpiryDate = item.ExpiryDate;
            }
            objRes.attribute = new List<SubAttribute>();
            foreach (var a in attribute)
            {
                SubAttribute objAtr = new SubAttribute();
                objAtr.Key = a.Key;
                objAtr.Value = a.Value;
                objRes.attribute.Add(objAtr);
            }
            objRes.files = new List<SubFiles>();
            foreach (var a in files)
            {
                SubFiles objfile = new SubFiles();
                objfile.Filename = a.Filename;
                objfile.Filesize = a.Filesize;
                objfile.MimeType = a.MimeType;
                objfile.Hash = a.Hash;              
                objRes.files.Add(objfile);
            }
          //  listRes.Add(objRes);

            //   return listRes.ToList();
            return objRes;
        }
       

        //public Task<ValidationResult> ValidateBatchId(Batch BatchId)
        //{
        //    return _validBatch.Validate(BatchId);
        //}
    }
}
