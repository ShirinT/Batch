using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI_Demo.Models
{
    public class CreateBatch
    {
        public string BatchId { get; set; }
        public string Status { get; set; }     
        public string BusinessUnit { get; set; }
        public string ReadUsers { get; set; }
        public string Readgroups { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? BatchPublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
