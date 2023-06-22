using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SQL_INJ_API
{
    public class PoliticiansDbContext : DbContext
    {
        public PoliticiansDbContext(DbContextOptions<PoliticiansDbContext> options) : base(options)
        {
        }

        public DbSet<Politician> Politicians { get; set; }

    }

}
