using Microsoft.EntityFrameworkCore;

namespace ef_mssql_char_bug
{
  public class SampleDbContext : DbContext
  {
    public SampleDbContext(DbContextOptions<SampleDbContext> opt) : base(opt)
    {
    }


    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      //Case-Sensitive Collation für die Datenbank verwenden
      modelBuilder.UseCollation("Latin1_General_CS_AS");


      modelBuilder.Entity<OrderModel>()
        .HasMany(o => o.Items)
        .WithOne(o => o.Order)
        .HasForeignKey(o => o.OrderId);
    }


    public DbSet<OrderModel> Order { get; set; }

    public DbSet<OrderItemModel> OrderItem { get; set; }
  }
}
