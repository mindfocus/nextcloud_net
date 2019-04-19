using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace model
{
    public class NCContext : DbContext
    {
        public DbSet<AccountTable> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTable>()
                .HasIndex(i => i.uid)
                .IsUnique(true);
        }
    }
}
