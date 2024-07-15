namespace BlazorCheckboxes;

using Microsoft.EntityFrameworkCore;

public class CounterContext(DbContextOptions<CounterContext> options) : DbContext(options)
{
    public DbSet<Counter> Counters { get; set; }
}
