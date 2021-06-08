using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BatchAPI_Demo.Models
{

    public partial class Error
    {
      
        public int CorrelationID { get; set; }
        [Key]
        public string Source { get; set; }
        public string Description { get; set; }
    }
    public partial class SubError
        {
            public string Source { get; set; }
            public string Description { get; set; }

        }
}
