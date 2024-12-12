using Microsoft.EntityFrameworkCore;

public class CarrosDbContext : DbContext
{
    public CarrosDbContext(DbContextOptions<CarrosDbContext> options) : base(options) { }

    public DbSet<ModeloCarro> ModelosCarros { get; set; }
}
