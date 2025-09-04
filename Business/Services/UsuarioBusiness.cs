using Business.Interfaces;
using Data.Interfaces;
using Entities;
using Entities.DTOs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IRepository<Usuario> _repository;

        public UsuarioBusiness(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(Usuario entity)
        {
            return await _repository.Add(entity);
        }

        public async Task Delete(string id)
        {
            await _repository.Delete(id);
        }

        public async Task<Usuario> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task Update(Usuario entity)
        {
            await _repository.Update(entity);
        }

        public async Task<AuthResponseDTO> AutenticarUsuario(AuthRequestDTO authRequest)
        {
            try
            {
                // Validar que se envíen los parámetros requeridos
                if (string.IsNullOrEmpty(authRequest.correo) || string.IsNullOrEmpty(authRequest.clave))
                {
                    return new AuthResponseDTO
                    {
                        success = false,
                        message = "Correo y clave son requeridos",
                        data = null
                    };
                }

                // Obtener todos los usuarios para buscar por correo
                var usuarios = await _repository.GetAll();
                var usuario = usuarios.FirstOrDefault(u => 
                    u.Email?.ToLower() == authRequest.correo.ToLower() && 
                    u.Activo == true);

                // Verificar si el usuario existe
                if (usuario == null)
                {
                    return new AuthResponseDTO
                    {
                        success = false,
                        message = "Credenciales inválidas",
                        data = null
                    };
                }

                // Verificar la contraseña (aquí deberías implementar hash/verificación)
                // Por ahora comparamos directamente (NO RECOMENDADO para producción)
                if (usuario.Password != authRequest.clave)
                {
                    return new AuthResponseDTO
                    {
                        success = false,
                        message = "Credenciales inválidas",
                        data = null
                    };
                }

                // Generar token JWT
                var token = GenerarJwtToken(usuario);
                var refreshToken = GenerarRefreshToken();

                // Crear información del usuario
                var userInfo = new UserInfoDTO
                {
                    id = int.TryParse(usuario.Id, out int userId) ? userId : 0,
                    nombre = $"{usuario.Nombre} {usuario.Apellido}".Trim(),
                    correo = usuario.Email,
                    idPerfil = int.TryParse(usuario.IdPerfil, out int perfilId) ? perfilId : 0,
                    activo = usuario.Activo,
                    ultimoAcceso = DateTime.UtcNow
                };

                // Crear datos de autenticación
                var authData = new AuthDataDTO
                {
                    token = token,
                    refreshToken = refreshToken,
                    expiresIn = 3600, // 1 hora en segundos
                    user = userInfo
                };

                // Actualizar último acceso del usuario
                usuario.FechaLog = DateTime.UtcNow;
                await _repository.Update(usuario);

                return new AuthResponseDTO
                {
                    success = true,
                    message = "Autenticación exitosa",
                    data = authData
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    success = false,
                    message = $"Error interno del servidor: {ex.Message}",
                    data = null
                };
            }
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            // Clave secreta para firmar el token (debería estar en configuración)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MiClaveSecretaSuperSegura123456789"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("IdPerfil", usuario.IdPerfil ?? "0"),
                new Claim("Activo", usuario.Activo.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "MuebleriaLaPlata",
                audience: "MuebleriaLaPlata",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerarRefreshToken()
        {
            // Generar un refresh token simple (en producción usar algo más seguro)
            return Guid.NewGuid().ToString();
        }
    }
}
