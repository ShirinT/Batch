using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BatchAPI_Demo.Models
{

    public partial class Attribute
    {
       
        public int? A_Id { get; set; }
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public string BatchId { get; set; }
    }
    public partial class SubAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
