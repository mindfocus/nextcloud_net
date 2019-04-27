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

        }
    }
}
