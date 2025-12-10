using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


using Dapper;
using System.Data.SqlClient;
 
namespace ProyectoSW01.Data
{
    public class DiagnosticoRepository
    {
        private readonly string _connectionString;

        public DiagnosticoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

    

        // LISTAR TODOS
        public async Task<List<Diagnostico>> ListarDiagnosticosAsync()
        {
            var lista = new List<Diagnostico>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarDiagnosticos", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Diagnostico
                {
                    DiagnosticoId = (int)reader["diagnostico_id"],
                    VehiculoId = (int)reader["vehiculo_id"],
                    Placa = reader["placa"].ToString(),
                    Modelo = reader["modelo"].ToString(),
                    MecanicoId = (int)reader["mecanico_id"],
                    MecanicoNombre = reader["mecanico_nombre"].ToString(),
                    Descripcion = reader["descripcion"].ToString(),
                    Fecha = (DateTime)reader["fecha"],
                    Estado = reader["estado"].ToString()
                });
            }

            return lista;
        }

        // OBTENER POR ID
        public async Task<Diagnostico> ObtenerDiagnosticoPorIdAsync(int id)
        {
            Diagnostico diag = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ObtenerDiagnosticoPorId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@diagnostico_id", id);

            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                diag = new Diagnostico
                {
                    DiagnosticoId = (int)dr["diagnostico_id"],
                    VehiculoId = (int)dr["vehiculo_id"],
                    Placa = dr["placa"].ToString(),
                    Modelo = dr["modelo"].ToString(),
                    PropietarioNombre = dr["propietario_nombre"].ToString(),
                    MecanicoId = (int)dr["mecanico_id"],
                    MecanicoNombre = dr["mecanico_nombre"].ToString(),
                    Descripcion = dr["descripcion"].ToString(),
                    Fecha = (DateTime)dr["fecha"],
                    Estado = dr["estado"].ToString()
                };
            }

            return diag;
        }


        // REGISTRAR
        public async Task RegistrarDiagnosticoAsync(Diagnostico d)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("RegistrarDiagnostico", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@vehiculo_id", d.VehiculoId);
            cmd.Parameters.AddWithValue("@mecanico_id", d.MecanicoId);
            cmd.Parameters.AddWithValue("@descripcion", d.Descripcion);
            cmd.Parameters.AddWithValue("@estado", d.Estado);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ACTUALIZAR
        public async Task ActualizarDiagnosticoAsync(Diagnostico d)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ActualizarDiagnostico", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@diagnostico_id", d.DiagnosticoId);
            cmd.Parameters.AddWithValue("@vehiculo_id", d.VehiculoId);
            cmd.Parameters.AddWithValue("@mecanico_id", d.MecanicoId);
            cmd.Parameters.AddWithValue("@descripcion", d.Descripcion);
            cmd.Parameters.AddWithValue("@estado", d.Estado);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ELIMINAR
        public async Task EliminarDiagnosticoAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("EliminarDiagnostico", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@diagnostico_id", id);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<(int Id, string Texto)>> ListarVehiculosParaDiagnosticoAsync()
        {
            var lista = new List<(int, string)>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarVehiculosParaDiagnostico", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int id = (int)reader["vehiculo_id"];
                string placa = reader["placa"].ToString();
                string marca = reader["marca"].ToString();
                string modelo = reader["modelo"].ToString();

                string texto = $"{placa} - {marca} {modelo}";

                lista.Add((id, texto));
            }

            return lista;
        }

        public async Task<IEnumerable<Usuario>> ListarMecanicosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT 
                u.id_usuario      AS IdUsuario,
                u.nombre_completo AS NombreCompleto,
                u.correo          AS Correo,
                u.contrasena      AS Contrasena,
                u.id_rol          AS IdRol
            FROM usuario u
            INNER JOIN rol r ON u.id_rol = r.id_rol
            WHERE r.nombre = @RolNombre;
        ";

                var mecanicos = await connection.QueryAsync<Usuario>(
                    sql,
                    new { RolNombre = "Mecánico" }   // O "Mecanico" según esté en tu BD
                );

                return mecanicos;
            }
        }








    }
}
