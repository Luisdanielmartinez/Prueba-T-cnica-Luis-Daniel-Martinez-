using Microsoft.EntityFrameworkCore;
using RuletteCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuletteInfrastructure.Data
{
    public class RouletteContext : DbContext
    {
        public RouletteContext(DbContextOptions<RouletteContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
