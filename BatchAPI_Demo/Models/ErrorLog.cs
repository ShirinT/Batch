using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BatchAPI_Demo.Models
{
    public class ErrorLog
    {
   //  [Key]
        public int CorrelationID { get; set; }
     
        public List<SubError> Error { get; set; }
    }
       
    
}
