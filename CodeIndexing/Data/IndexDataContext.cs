using CodeIndexing.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeIndexing.Data
{
    public class IndexDataContext : DbContext
    {
        public DbSet<MethodDto> Methods { get; set; }
        public DbSet<ClassDto> Classes { get; set; }
        public DbSet<ParameterDto> Parameters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = myIndexDb.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MethodRelationship>()
                  .HasKey(e => new { e.CallingMethodId, e.MethodBeingCalledId });
            
            modelBuilder.Entity<MethodRelationship>()
                  .HasOne(e => e.CallingMethod)
                  .WithMany(e => e.MethodsCalled)
                  .HasForeignKey(e => e.CallingMethodId);
            
            modelBuilder.Entity<MethodRelationship>()
                  .HasOne(e => e.MethodBeingCalled)
                  .WithMany(e => e.CalledByMethods)
                  .HasForeignKey(e => e.MethodBeingCalledId);

        }
    }
}
