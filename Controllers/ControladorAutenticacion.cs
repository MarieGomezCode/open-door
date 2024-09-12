using AuthService.Models;
using AuthService.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorAutenticacion : ControllerBase
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IConfiguration _configuracion;

        public ControladorAutenticacion(IRepositorioUsuario repositorioUsuario, IConfiguration configuracion)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuracion = configuracion;
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] SolicitudInicioSesion solicitud)
        {
            var usuario = await _repositorioUsuario.ObtenerUsuarioPorNombreAsync(solicitud.NombreUsuario);
            if (usuario == null || !VerificarContraseña(solicitud.Contraseña, usuario.ContraseñaHash))
            {
                return Unauthorized();
            }

            var token = GenerarTokenJwt(usuario);
            return Ok(new { Token = token });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] SolicitudInicioSesion solicitud)
        {
            var usuarioExistente = await _repositorioUsuario.ObtenerUsuarioPorNombreAsync(solicitud.NombreUsuario);
            if (usuarioExistente != null)
            {
                return Conflict("El usuario ya existe");
            }

            var usuario = new Usuario(solicitud.NombreUsuario, HashearContraseña(solicitud.Contraseña));

            await _repositorioUsuario.AgregarUsuarioAsync(usuario);
            return Ok();
        }

        private string GenerarTokenJwt(Usuario usuario)
        {
            var configuracionJwt = _configuracion.GetSection("Jwt");

            var claveString = configuracionJwt["Key"];
            if (string.IsNullOrEmpty(claveString))
            {
                throw new InvalidOperationException("La clave JWT no está configurada.");
            }

            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveString));
            var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

            var reclamaciones = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.NombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiryInMinutesString = configuracionJwt["ExpiryInMinutes"];
            if (string.IsNullOrEmpty(expiryInMinutesString))
            {
                throw new InvalidOperationException("El valor de ExpiryInMinutes no está configurado.");
            }

            int expiryInMinutes = int.Parse(expiryInMinutesString);

            var token = new JwtSecurityToken(
                issuer: configuracionJwt["Issuer"],
                audience: configuracionJwt["Audience"],
                claims: reclamaciones,
                expires: DateTime.Now.AddMinutes(expiryInMinutes),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashearContraseña(string contraseña)
        {
            // Implementa la lógica de hash de contraseña aquí (por ejemplo, usando BCrypt)
            return contraseña; // Marcador de posición
        }

        private bool VerificarContraseña(string contraseña, string contraseñaHasheada)
        {
            // Implementa la lógica de verificación de contraseña aquí (por ejemplo, usando BCrypt)
            return contraseña == contraseñaHasheada; // Marcador de posición
        }
    }
}
