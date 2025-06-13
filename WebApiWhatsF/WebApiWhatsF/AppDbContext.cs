using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using WebApiWhatsF.Models;


namespace WebApplpdfFinal6
{
    public class AppDbContext : DbContext
    {
        public DbSet<PdfDocument> PdfDocuments { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base((DbContextOptions)options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PdfDocument>((Action<EntityTypeBuilder<PdfDocument>>)(entity =>
            {
                entity.HasKey((Expression<Func<PdfDocument, object>>)(e => (object)e.Id));
                entity.Property<string>((Expression<Func<PdfDocument, string>>)(e => e.FileName)).IsRequired(true).HasMaxLength((int)byte.MaxValue);
                entity.Property<byte[]>((Expression<Func<PdfDocument, byte[]>>)(e => e.FileData)).IsRequired(true);
                entity.Property<string>((Expression<Func<PdfDocument, string>>)(e => e.FilePath)).IsRequired(true);
                entity.Property<string>((Expression<Func<PdfDocument, string>>)(e => e.ContentType)).IsRequired(true).HasMaxLength(100);
            }));
        }
    }
}
