using System;
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

         // LISTAR TODOS LOS USUARIOS
          public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var lista = new List<Usuario>();

            string sql = @"SELECT id_usuario, nombre_completo, correo, contrasena, id_rol
                           FROM usuario";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                            NombreCompleto = reader.GetString(reader.GetOrdinal("nombre_completo")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                            IdRol = reader.GetInt32(reader.GetOrdinal("id_rol"))
                        };
                        lista.Add(u);
                    }
                }
            }

            return lista;
        }

         // OBTENER UN USUARIO POR ID
         public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            Usuario? usuario = null;

            string sql = @"SELECT id_usuario, nombre_completo, correo, contrasena, id_rol
                           FROM usuario
                           WHERE id_usuario = @id_usuario";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                            NombreCompleto = reader.GetString(reader.GetOrdinal("nombre_completo")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                            IdRol = reader.GetInt32(reader.GetOrdinal("id_rol"))
                        };
                    }
                }
            }

            return usuario;
        }

         // AGREGAR USUARIO (RegistrarUsuario)
       
        public async Task AgregarUsuarioAsync(Usuario u)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("RegistrarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre_completo", u.NombreCompleto);
                cmd.Parameters.AddWithValue("@correo", u.Correo);
                cmd.Parameters.AddWithValue("@contrasena", u.Contrasena);
                cmd.Parameters.AddWithValue("@id_rol", u.IdRol);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
         
        // ACTUALIZAR USUARIO 
        
        public async Task ActualizarUsuarioAsync(Usuario u)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("ActualizarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", u.IdUsuario);
                cmd.Parameters.AddWithValue("@nombre_completo", u.NombreCompleto);
                cmd.Parameters.AddWithValue("@correo", u.Correo);
                cmd.Parameters.AddWithValue("@id_rol", u.IdRol);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

     
        // ELIMINAR USUARIO 
     
        public async Task EliminarUsuarioAsync(int idUsuario)
        {
            string sql = @"DELETE FROM usuario WHERE id_usuario = @id_usuario";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
