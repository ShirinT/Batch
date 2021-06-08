using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchAPI_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime;
using Attribute = BatchAPI_Demo.Models.Attribute;
using Microsoft.EntityFrameworkCore;
//using Acl = BatchAPI_Demo.Models.Acl;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BatchAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {

        private readonly BatchContext _BatchContext;
        
        public BatchController(BatchContext BatchContext)
        {
            _BatchContext = BatchContext;
        }

        // GET: api/<BatchController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<BatchController>/5
        [HttpGet]
        [Route("/batch")]
        public string Batch(string Batchid)
        {
            ErrorLog objError = new ErrorLog();
            SubError objsubErr = new SubError();
            List<ResBatch> listRes = new List<ResBatch>();
            ResBatch objRes = new ResBatch();
            if (Batchid != null)
            {
                var batchList = (from Batch in _BatchContext.BatchDetails
                                 where Batch.BatchId == Batchid
                                 select new
                                 {
                                     Batch.BatchId,
                                     Batch.BusinessUnit,
                                     Batch.BatchPublishedDate,
                                     Batch.Status,
                                     Batch.ExpiryDate
                                 }).ToList();

                var attrList = (from attr in _BatchContext.Attributes
                                where attr.BatchId == Batchid
                                select new
                                {
                                    attr.Key,
                                    attr.Value
                                }).ToList();

                var fileList = (from Lfile in _BatchContext.File
                                where Lfile.BatchId == Batchid
                                select new {
                                    Lfile.Filename,
                                    Lfile.Filesize,
                                    Lfile.MimeType,
                                    Lfile.Hash,
                                    Lfile.Key,
                                    Lfile.Value
                                }).ToList();

                if (batchList.Count == 0)
                {
                    objError.CorrelationID = 400;
                    var viewModel = (from ErrorList in _BatchContext.Errors
                                     where ErrorList.CorrelationID == 6
                                     select new
                                     {
                                         ErrorList.Source,
                                         ErrorList.Description
                                     }).ToList();

                    objError.Error = new List<SubError>();
                    foreach (var a in viewModel)
                    {
                        objsubErr.Source = a.Source;
                        objsubErr.Description = a.Description;
                        objError.Error.Add(objsubErr);
                    }
                    return JsonConvert.SerializeObject(objError);
                }

                foreach (var item in batchList)
                {                   
                    objRes.BatchId = item.BatchId;
                    objRes.BusinessUnit = item.BusinessUnit.Trim();
                    objRes.BatchPublishedDate = item.BatchPublishedDate;
                    objRes.Status = item.Status.Trim();
                    objRes.ExpiryDate = item.ExpiryDate;
                }
                if (objRes.ExpiryDate <= DateTime.Now)
                {
                    objError.CorrelationID = 400;
                    var viewModel = (from ErrorList in _BatchContext.Errors
                                     where ErrorList.CorrelationID == 7
                                     select new
                                     {
                                         ErrorList.Source,
                                         ErrorList.Description
                                     }).ToList();

                    objError.Error = new List<SubError>();
                    foreach (var a in viewModel)
                    {
                        objsubErr.Source = a.Source;
                        objsubErr.Description = a.Description;
                        objError.Error.Add(objsubErr);
                    }
                    return JsonConvert.SerializeObject(objError);

                }
                objRes.attribute = new List<SubAttribute>();
                foreach (var a in attrList)
                {
                    SubAttribute objAtr = new SubAttribute();
                    objAtr.Key = a.Key;
                    objAtr.Value = a.Value;
                    objRes.attribute.Add(objAtr);
                }

                objRes.files = new List<SubFiles>();
                foreach (var a in fileList)
                {
                    SubFiles objfile = new SubFiles();
                    objfile.Filename = a.Filename;
                    objfile.Filesize = a.Filesize;
                    objfile.MimeType = a.MimeType;
                    objfile.Hash = a.Hash;
                  //  objRes.files = new List<SubAttribute>();
                    foreach (var item in attrList)
                    {
                        SubAttribute objAtr = new SubAttribute();
                        objAtr.Key = a.Key;
                        objAtr.Value = a.Value;
                        objRes.attribute.Add(objAtr);
                    }
                    objRes.files.Add(objfile);
                }
                //SubFiles objfile = new SubFiles();
                //objRes.files = new List<SubFiles>();
                
                //objRes.files.Add(objfile);

                listRes.Add(objRes);
            }
            else
            {
                objError.CorrelationID = 400;
                var viewModel = (from ErrorList in _BatchContext.Errors
                                 where ErrorList.CorrelationID == 5
                                 select new
                                 {
                                     ErrorList.Source,
                                     ErrorList.Description
                                 }).ToList();

                objError.Error = new List<SubError>();
                foreach (var a in viewModel)
                {
                    objsubErr.Source = a.Source;
                    objsubErr.Description = a.Description;
                    objError.Error.Add(objsubErr);
                }
                return JsonConvert.SerializeObject(objError);
            }
            return JsonConvert.SerializeObject(objRes);
        }


        // POST api/<BatchController>
        [HttpPost]
        // [Route("~/batch")]       
        [Route("/batch")]
        public async Task<string> Batch(ReqBatch req)
        {
            ErrorLog objError = new ErrorLog();
            SubError objsubErr = new SubError();
                       
            DateTime currentdate = DateTime.Now;
            //create guid
            Guid objGuid = Guid.NewGuid();
            string Batchid = objGuid.ToString();

            if (req.Businessunit == "")
            {
                objError.CorrelationID = 400;
                var viewModel = (from ErrorList in _BatchContext.Errors
                                 where ErrorList.CorrelationID == 1
                                 select new
                                 {
                                     ErrorList.Source,
                                     ErrorList.Description
                                 }).ToList();

                objError.Error = new List<SubError>();
                foreach (var a in viewModel)
                {
                    objsubErr.Source = a.Source;
                    objsubErr.Description = a.Description;
                    objError.Error.Add(objsubErr);
                }
                return JsonConvert.SerializeObject(objError);
            }
            // save batch details
            BatchDetail objBatch = new BatchDetail
            {
                BatchId = Batchid,
                Status = "Initiate",
                BusinessUnit = req.Businessunit,
                BatchPublishedDate = currentdate,
                ExpiryDate = req.ExpiryDate,
            };

            _BatchContext.BatchDetails.Add(objBatch);

            //save attribute table details
            List<SubAttribute> lstattr = new List<SubAttribute>();
            lstattr = req.Attribute;
            for (int i = 0; i < lstattr.Count; i++)
            {
                if (lstattr[i].Key == "" || lstattr[i].Value == "")
                {
                    objError.CorrelationID = 400;
                    var viewModel = (from ErrorList in _BatchContext.Errors
                                     where ErrorList.CorrelationID == 2
                                     select new
                                     {
                                         ErrorList.Source,
                                         ErrorList.Description
                                     }).ToList();

                    objError.Error = new List<SubError>();
                    foreach (var a in viewModel)
                    {
                        objsubErr.Source = a.Source;
                        objsubErr.Description = a.Description;
                        objError.Error.Add(objsubErr);
                    }
                    return JsonConvert.SerializeObject(objError);
                }
                Attribute attr = new Attribute()
                {
                    Key = lstattr[i].Key,
                    Value = lstattr[i].Value,
                    BatchId = Batchid,
                };
                _BatchContext.Attributes.Add(attr);
            }
            //save files data
            List<SubAttribute> lstfiles = new List<SubAttribute>();
            lstfiles = req.Attribute;
            for (int i = 0; i < lstfiles.Count; i++)
            {
                Files objfile = new Files()
                {
                    Filename = "",
                    Filesize="0",
                    MimeType="",
                    Hash="###",
                    Key = lstfiles[i].Key,
                    Value = lstfiles[i].Value,
                    BatchId = Batchid,
                };
                _BatchContext.File.Add(objfile);
            }

                //save acl table details
                SubAcl objAcl = new SubAcl();
            objAcl = req.Acl;

            List<string> lstRdUsers = new List<string>();
            lstRdUsers = objAcl.ReadUsers;

            for (int i = 0; i < lstRdUsers.Count; i++)
            {
                if (lstRdUsers[i].ToString() == "")
                {
                    objError.CorrelationID = 400;
                    var viewModel = (from ErrorList in _BatchContext.Errors
                                     where ErrorList.CorrelationID == 3
                                     select new
                                     {
                                         ErrorList.Source,
                                         ErrorList.Description
                                     }).ToList();

                    objError.Error = new List<SubError>();
                    foreach (var a in viewModel)
                    {
                        objsubErr.Source = a.Source;
                        objsubErr.Description = a.Description;
                        objError.Error.Add(objsubErr);
                    }
                    return JsonConvert.SerializeObject(objError);
                }
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
                if (lstRdGroups[i].ToString() == "")
                {
                    var viewModel = (from ErrorList in _BatchContext.Errors
                                     where ErrorList.CorrelationID == 4
                                     select new
                                     {
                                         ErrorList.Source,
                                         ErrorList.Description
                                     }).ToList();

                    objError.Error = new List<SubError>();
                    foreach (var a in viewModel)
                    {
                        objsubErr.Source = a.Source;
                        objsubErr.Description = a.Description;
                        objError.Error.Add(objsubErr);
                    }
                    return JsonConvert.SerializeObject(objError);
                }
                var acl2 = new Acl
                {
                    ReadGroups = lstRdGroups[i].ToString(),
                    BatchId = Batchid
                };
                _BatchContext.Acls.Add(acl2);
            }
            //FileSize

            _BatchContext.SaveChanges();

            Batch objBD = new Batch();
            objBD.BatchId = Batchid;

            return JsonConvert.SerializeObject(objBD);
        }
    }
}
