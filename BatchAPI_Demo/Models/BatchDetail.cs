using System;
using System.Collections.Generic;

#nullable disable

namespace BatchAPI_Demo.Models
{
    public partial class BatchDetail
    {
        public int Id { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public string Attr_Id { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime? BatchPublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
