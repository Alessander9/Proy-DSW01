using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class RolRepository
    {
        private readonly string _connectionString;

        public RolRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection"); ;
        }

        // ============================
        // LISTAR ROLES
        // ============================
        public async Task<List<Rol>> ListarRolesAsync()
        {
            var lista = new List<Rol>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT id_rol, nombre FROM rol ORDER BY nombre",
                connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Rol
                {
                    IdRol = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }

            return lista;
        }
    }
}
