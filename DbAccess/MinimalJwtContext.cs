using Microsoft.EntityFrameworkCore;
using MinimalJwt.Models;

namespace MinimalJwt.DbAccess
{
    public class MinimalJwtContext : DbContext
    {

        public MinimalJwtContext(DbContextOptions options) : base (options)
        {

        }

        public DbSet<Movie> Movies { get; set; }

    }
}
