using apiChat.Models;
using Microsoft.EntityFrameworkCore;

namespace apiChat.Contexto
{
    public class DatosContexto : DbContext
    {
        public DatosContexto(DbContextOptions<DatosContexto> options) : base (options){  }

        public DbSet<User> usuarios { get; set; }
        
    }

    
}
