using ApiCaching.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCaching.Data;

public class AppDbContext : DbContext
{
	public DbSet<Driver> Drivers { get; set; }
	public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
	{

	}
}
