using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace model
{
    public class NCContext : DbContext
    {
        public DbSet<AccountTable> Accounts { get; set; }
        public DbSet<StoragesTable> Storages { get; set; }
        public DbSet<AppConfigTable> AppConfigs { get; set; }
        public DbSet<FileCacheTable> FileCaches { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTable>()
                .HasIndex(i => i.uid)
                .IsUnique(true);
            modelBuilder.Entity<StoragesTable>()
                .HasIndex(i => i.numeric_id)
                .IsUnique(true);
            modelBuilder.Entity<StoragesTable>()
                .HasIndex(i => i.id)
                .IsUnique(true);
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => i.fileid)
                .HasName("PRIMARY")
                .IsUnique(true);
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => new {i.storage, i.path_hash})
                .HasName("fs_storage_path_hash")
                .IsUnique(true);
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => new {i.parent, i.name})
                .HasName("fs_parent_name_hash");
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => new {i.storage, i.mimetype})
                .HasName("fs_storage_mimetype");
                        
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => new {i.storage, i.mimepart})
                .HasName("fs_storage_mimepart");                       
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => new {i.storage, i.size, i.fileid})
                .HasName("fs_storage_size");
                        
            modelBuilder.Entity<FileCacheTable>()
                .HasIndex(i => i.mtime)
                .HasName("fs_mtime");
                        
            
        }
    }
}
