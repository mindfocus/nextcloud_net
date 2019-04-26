using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace model
{
    [Table("storages")]
    public class StoragesTable
    {
        [Key]
        [MaxLength(20)]
        public long numeric_id { get; set; }
        [MaxLength(64)]
        public string id { get; set; }
        [MaxLength(11)]
        public int available = 1;
        [MaxLength(11)]
        public long last_checked { get; set; }
    }
}