using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace model
{
    [Table("accounts")]
    public class AccountTable
    {
        [Key]
        [MaxLength(64)]
        public string uid { get; set; }
        public string data { get; set; }
    }
}
