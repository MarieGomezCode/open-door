namespace AuthService.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string ContraseñaHash { get; set; }

        // Constructor que asegura que las propiedades no sean nulas
        public Usuario(string nombreUsuario, string contraseñaHash)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ContraseñaHash = contraseñaHash ?? throw new ArgumentNullException(nameof(contraseñaHash));
        }
    }
}
