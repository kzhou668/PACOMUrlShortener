using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PACOMUrlShortener.Models
{
    public partial class SqlDBContext : DbContext
    {
        public SqlDBContext()
        {
        }

        public SqlDBContext(DbContextOptions<SqlDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Urlshortener> Urlshortener { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
