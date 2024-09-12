using AuthService.Models;
using AuthService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly DbContextApp _contexto;

        public RepositorioUsuario(DbContextApp contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario?> ObtenerUsuarioPorNombreAsync(string nombreUsuario)
        {
            return await _contexto.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task AgregarUsuarioAsync(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
        }
    }
}
