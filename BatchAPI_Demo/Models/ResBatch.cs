using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
namespace BatchAPI_Demo.Models
{
    public class ResBatch
    {
      //  private List<ResBatch> listRes;


        public string BatchId { get; set; }
        public string Status { get; set; }
        public List<SubAttribute> attribute { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime? BatchPublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public List<SubFiles> files { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        //[NotMapped]
        ///public IFormFile File { get; set; }
    }
}
