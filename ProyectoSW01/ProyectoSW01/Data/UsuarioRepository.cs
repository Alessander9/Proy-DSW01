using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // ============================
        // LISTAR USUARIOS
        // ============================
        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            var lista = new List<Usuario>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarUsuarios", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = (int)reader["id_usuario"],
                    NombreCompleto = reader["nombre_completo"].ToString(),
                    Correo = reader["correo"].ToString(),
                    Contrasena = reader["contrasena"].ToString(),
                    IdRol = (int)reader["id_rol"]
                });
            }

            return lista;
        }

        // ============================
        // REGISTRAR USUARIO
        // ============================
        public async Task RegistrarUsuarioAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("RegistrarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombre_completo", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@correo", usuario.Correo);
            command.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
            command.Parameters.AddWithValue("@id_rol", usuario.IdRol);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ============================
        // OBTENER POR ID
        // ============================
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerUsuarioPorId", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id_usuario", idUsuario);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = (int)reader["id_usuario"],
                    NombreCompleto = reader["nombre_completo"].ToString(),
                    Correo = reader["correo"].ToString(),
                    Contrasena = reader["contrasena"].ToString(),
                    IdRol = (int)reader["id_rol"]
                };
            }

            return null;
        }

        // ============================
        // ACTUALIZAR
        // ============================
        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ActualizarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
            command.Parameters.AddWithValue("@nombre_completo", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@correo", usuario.Correo);
            command.Parameters.AddWithValue("@id_rol", usuario.IdRol);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }


        public async Task<Usuario?> DetailsAsync(int idUsuario)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerUsuarioPorId", connection);
            // Si el procedimiento almacenado es específico para Obtener por ID, úsalo.
            // Si quieres un nuevo SP llamado 'DetallesUsuario', cambia la cadena.
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id_usuario", idUsuario);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                // Mapeo del objeto Usuario
                return new Usuario
                {
                    IdUsuario = (int)reader["id_usuario"],
                    NombreCompleto = reader["nombre_completo"].ToString(),
                    Correo = reader["correo"].ToString(),
                    Contrasena = reader["contrasena"].ToString(),
                    IdRol = (int)reader["id_rol"]
                };
            }

            return null; // Retorna null si no se encuentra el usuario
        }



        // ============================
        // ELIMINAR
        // ============================
        public async Task EliminarUsuarioAsync(int idUsuario)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("EliminarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id_usuario", idUsuario);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }


        public async Task<List<Usuario>> ListarMecanicosAsync()
        {
            var lista = new List<Usuario>();

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT u.id_usuario, u.nombre_completo, u.correo, u.contrasena, u.id_rol
        FROM usuario u
        INNER JOIN rol r ON r.id_rol = u.id_rol
        WHERE r.nombre = 'MECANICO'
        ORDER BY u.nombre_completo;
    ", cn);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = (int)dr["id_usuario"],          // 👈 OJO
                    NombreCompleto = dr["nombre_completo"].ToString(), // 👈 OJO
                    Correo = dr["correo"].ToString(),
                    Contrasena = dr["contrasena"].ToString(),
                    IdRol = (int)dr["id_rol"]
                });
            }

            return lista;
        }

        public async Task<Usuario?> ValidarUsuarioAsync(string correo, string contrasena)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ValidarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@correo", correo);
            command.Parameters.AddWithValue("@contrasena", contrasena);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = (int)reader["id_usuario"],
                    NombreCompleto = reader["nombre_completo"].ToString(),
                    Correo = reader["correo"].ToString(),
                    Contrasena = reader["contrasena"].ToString(),
                    IdRol = (int)reader["id_rol"]
                };
            }

            return null;
        }


    }
}
