namespace AuthService.Models
{
    public class SolicitudInicioSesion
    {
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }

        // Constructor que asegura que las propiedades no sean nulas
        public SolicitudInicioSesion(string nombreUsuario, string contraseña)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            Contraseña = contraseña ?? throw new ArgumentNullException(nameof(contraseña));
        }
    }
}
