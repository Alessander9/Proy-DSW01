using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class RepuestoRepository
    {
        private readonly string _connectionString;

        public RepuestoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // ============================================
        // LISTAR REPUESTOS
        // ============================================
        public async Task<List<Repuesto>> ListarRepuestosAsync()
        {
            var lista = new List<Repuesto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarRepuestos", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Repuesto
                {
                    RepuestoId = (int)reader["repuesto_id"],
                    Nombre = reader["nombre"].ToString(),
                    Descripcion = reader["descripcion"].ToString(),
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"]
                });
            }

            return lista;
        }

        // ============================================
        // OBTENER POR ID
        // ============================================
        public async Task<Repuesto?> ObtenerRepuestoPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerRepuestoPorId", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@repuesto_id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Repuesto
                {
                    RepuestoId = (int)reader["repuesto_id"],
                    Nombre = reader["nombre"].ToString(),
                    Descripcion = reader["descripcion"].ToString(),
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"]
                };
            }

            return null;
        }

        // ============================================
        // REGISTRAR REPUESTO
        // ============================================
        public async Task RegistrarRepuestoAsync(Repuesto repuesto)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("RegistrarRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombre", repuesto.Nombre);
            command.Parameters.AddWithValue("@descripcion", repuesto.Descripcion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@precio", repuesto.Precio);
            command.Parameters.AddWithValue("@stock", repuesto.Stock);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ============================================
        // ACTUALIZAR REPUESTO
        // ============================================
        public async Task ActualizarRepuestoAsync(Repuesto repuesto)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ActualizarRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@repuesto_id", repuesto.RepuestoId);
            command.Parameters.AddWithValue("@nombre", repuesto.Nombre);
            command.Parameters.AddWithValue("@descripcion", repuesto.Descripcion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@precio", repuesto.Precio);
            command.Parameters.AddWithValue("@stock", repuesto.Stock);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ============================================
        // ELIMINAR REPUESTO
        // ============================================
        public async Task EliminarRepuestoAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("EliminarRepuesto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@repuesto_id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
