using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BatchAPI_Demo.Models
{

    public class Error
    {      
        public int CorrelationID { get; set; }
        [Key]
        public List<SubError> Errors { get; set; }
   //     public string Source { get; set; }
   //     public string Description { get; set; }
    }
    public class SubError
        {
        [Key]
            public string Source { get; set; }
            public string Description { get; set; }

        }
}
