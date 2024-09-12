using AuthService.Models;

namespace AuthService.Repositorios
{
    public interface IRepositorioUsuario
    {
        Task<Usuario?> ObtenerUsuarioPorNombreAsync(string nombreUsuario);
        Task AgregarUsuarioAsync(Usuario usuario);
    }
}
