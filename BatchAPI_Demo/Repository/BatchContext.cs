using System;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;
using BatchAPI_Demo.Models;
using Attribute = BatchAPI_Demo.Models.Attribute;

namespace BatchAPI_Demo.Repository
{
    public partial class BatchContext : DbContext
    {
        public BatchContext()
        {
        }

        public BatchContext(DbContextOptions<BatchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acl> Acls { get; set; }
        public virtual DbSet<Attribute> Attributes { get; set; }
        public DbSet<BatchDetail> BatchDetails { get; set; }
        public virtual DbSet<SubError> Errors { get; set; }
        public virtual DbSet<Files> File { get; set; }
     //   public virtual DbSet<SubFiles> Attribute { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BatchDatabase"].ConnectionString);
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Acl>(entity =>
            {
             
                entity.ToTable("Acl");

                entity.Property(e => e.Acl_Id)
                    .HasMaxLength(10)
                    .HasColumnName("Acl_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.BatchId).HasMaxLength(50);                
              
                entity.Property(e => e.ReadGroups).HasMaxLength(50);

                entity.Property(e => e.ReadUsers).HasMaxLength(50);
            });

            modelBuilder.Entity<Attribute>(entity =>
            {
              
                entity.Property(e => e.A_Id).HasColumnName("A_Id");

                entity.Property(e => e.BatchId).HasMaxLength(50);

                entity.Property(e => e.Key)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Value)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });
//manually added
            modelBuilder.Entity<SubError>(entity =>
            {
                // entity.HasNoKey();

             //   entity.Property(e => e.CorrelationID).HasColumnName("CorrelationID");

                entity.Property(e => e.Source)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Files>(entity =>
            {

                entity.Property(e => e.BatchId).HasColumnName("BatchId");

                entity.Property(e => e.Filename).HasMaxLength(50);
                entity.Property(e => e.Filesize).HasMaxLength(50);
                entity.Property(e => e.MimeType).HasMaxLength(50);
                entity.Property(e => e.Hash).HasMaxLength(50);
                entity.Property(e => e.Key)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Value)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });
            //
            modelBuilder.Entity<BatchDetail>(entity =>
            {
                entity.Property(e => e.Attr_Id)
                    .HasMaxLength(10)
                    .HasColumnName("Attr_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.BatchId).HasMaxLength(50);

                entity.Property(e => e.BatchPublishedDate).HasColumnType("datetime");

                entity.Property(e => e.BusinessUnit)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });
                   
            OnModelCreatingPartial(modelBuilder);
            //var keysProperties = modelBuilder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            //foreach (var property in keysProperties)
            //{
            //    property.ValueGenerated = ValueGenerated.OnAdd;
            //}
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
