using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
                    DiagnosticoId = dr.GetInt32(0),
                    VehiculoId = dr.GetInt32(1),
                    MecanicoId = dr.GetInt32(2),
                    Descripcion = dr.GetString(3),
                    Fecha = dr.GetDateTime(4),
                    Estado = dr.GetString(5)
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




    }
}
