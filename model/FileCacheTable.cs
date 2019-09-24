using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model
{
    [Table("filecache")]
    public class FileCacheTable
    {
        [Key]
        [MaxLength(20)]
        [Required]
        public Int64 fileid;

        [MaxLength(20)]
        [Required]
        public Int64 storage;

        [MaxLength(4000)]
        public string path;

        [MaxLength(32)]
        [Required]
        public string path_hash;

        [MaxLength(20)]
        [Required]
        public Int64 parent;

        [MaxLength(250)]
        public string name;

        [MaxLength(20)]
        [Required]
        public Int64 mimetype;

        [Required] [MaxLength(20)] public Int64 mimepart;
        [MaxLength(20)]  [Required] public Int64 size;
        [MaxLength(20)] [Required] public Int64 mtime;
        [MaxLength(20)] [Required] public Int64 storage_mtime;
        [MaxLength(11)] [Required] public int encrypted;
        [MaxLength(20)] [Required] public Int64 unencrypted_size;
        [MaxLength(40)] public string etag;
        [MaxLength(11)] public int permissions;
        [MaxLength(255)] public string checksum;
    }
}