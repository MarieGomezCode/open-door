using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class DbContextApp : DbContext
    {
        public DbContextApp(DbContextOptions<DbContextApp> opciones) : base(opciones)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
