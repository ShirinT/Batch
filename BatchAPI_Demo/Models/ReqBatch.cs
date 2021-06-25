using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI_Demo.Models
{
    public class ReqBatch
    {     
        public string Businessunit { get; set; }
        public SubAcl Acl { get; set; }
        public List<SubAttribute> Attribute { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }


}
