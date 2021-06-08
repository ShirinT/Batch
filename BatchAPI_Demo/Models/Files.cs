using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BatchAPI_Demo.Models
{
    public partial class Files
    {
        [Key]
        public string BatchId { get; set; }
        public string Filename { get; set; }
        public string Filesize { get; set; }
        public string MimeType { get; set; }
        public string Hash { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public partial class SubFiles
    {
        public string Filename { get; set; }
        public string Filesize { get; set; }
        public string MimeType { get; set; }
        public string Hash { get; set; }
        public List<SubAttribute> attribute { get; set; }
    }
}
