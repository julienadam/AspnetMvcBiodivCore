using Microsoft.EntityFrameworkCore;

namespace AspNetBiodiv.Core.Web.Entities;

public partial class EspecesContext : DbContext
{
    public EspecesContext()
    {

    }


    public EspecesContext(DbContextOptions<EspecesContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Espece> Especes { get; set; }
    public DbSet<Observation> Observations { get; set; }
}
