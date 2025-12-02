using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class ServicioRepuestoRepository
    {
        private readonly string _connectionString;

        public ServicioRepuestoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // ============================================
        // LISTAR POR DIAG_SERV_ID
        // ============================================
        public async Task<List<ServicioRepuesto>> ListarPorDiagnosticoAsync(int diagServId)
        {
            var lista = new List<ServicioRepuesto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarServicioRepuestosPorDiagnostico", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@diag_serv_id", diagServId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ServicioRepuesto
                {
                    ServRepId = (int)reader["serv_rep_id"],
                    DiagServId = (int)reader["diag_serv_id"],
                    RepuestoId = (int)reader["repuesto_id"],
                    Cantidad = (int)reader["cantidad"],
                    Precio = (decimal)reader["precio"]
                });
            }

            return lista;
        }

        // ============================================
        // OBTENER POR ID
        // ============================================
        public async Task<ServicioRepuesto?> ObtenerPorIdAsync(int servRepId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerServicioRepuestoPorId", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@serv_rep_id", servRepId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ServicioRepuesto
                {
                    ServRepId = (int)reader["serv_rep_id"],
                    DiagServId = (int)reader["diag_serv_id"],
                    RepuestoId = (int)reader["repuesto_id"],
                    Cantidad = (int)reader["cantidad"],
                    Precio = (decimal)reader["precio"]
                };
            }

            return null;
        }

        // ============================================
        // REGISTRAR
        // ============================================
        public async Task RegistrarAsync(ServicioRepuesto entidad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("RegistrarServicioRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@diag_serv_id", entidad.DiagServId);
            command.Parameters.AddWithValue("@repuesto_id", entidad.RepuestoId);
            command.Parameters.AddWithValue("@cantidad", entidad.Cantidad);
            command.Parameters.AddWithValue("@precio", entidad.Precio);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ============================================
        // ACTUALIZAR
        // ============================================
        public async Task ActualizarAsync(ServicioRepuesto entidad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ActualizarServicioRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@serv_rep_id", entidad.ServRepId);
            command.Parameters.AddWithValue("@diag_serv_id", entidad.DiagServId);
            command.Parameters.AddWithValue("@repuesto_id", entidad.RepuestoId);
            command.Parameters.AddWithValue("@cantidad", entidad.Cantidad);
            command.Parameters.AddWithValue("@precio", entidad.Precio);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ============================================
        // ELIMINAR
        // ============================================
        public async Task EliminarAsync(int servRepId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("EliminarServicioRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@serv_rep_id", servRepId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
