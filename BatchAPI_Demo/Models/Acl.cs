using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace BatchAPI_Demo.Models
{
    public partial class Acl
    {
       
        public string Acl_Id { get; set; }      
        public string ReadUsers { get; set; }     
        public string ReadGroups { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string BatchId { get; set; }
    }
    public partial class SubAcl
    {
        public List<string> ReadUsers { get; set; }
     
        public List<string> Readgroups { get; set; }

    }
}
