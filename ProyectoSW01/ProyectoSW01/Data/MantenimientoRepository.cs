using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class MantenimientoRepository
    {
        private readonly string _connectionString;

        public MantenimientoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        public async Task<IEnumerable<Mantenimiento>> ListarMantenimientosAsync()
        {
            var lista = new List<Mantenimiento>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("ListarMantenimientos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Mantenimiento
                        {
                            MantenimientoId = reader.GetInt32(reader.GetOrdinal("mantenimiento_id")),
                            VehiculoId = reader.GetInt32(reader.GetOrdinal("vehiculo_id")),
                            ServicioId = reader.GetInt32(reader.GetOrdinal("servicio_id")),
                            FechaProgramada = reader["fecha_programada"] as DateTime?,
                            FechaEntrega = reader["fecha_entrega"] as DateTime?,
                            Estado = reader.GetString(reader.GetOrdinal("estado")),
                            Notas = reader["notas"] as string,
                            Placa = reader["placa"].ToString(),
                            Marca = reader["marca"].ToString(),
                            Modelo = reader["modelo"].ToString(),
                            ServicioNombre = reader["servicio_nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public async Task<Mantenimiento?> ObtenerMantenimientoPorIdAsync(int id)
        {
            Mantenimiento? mantenimiento = null;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("ObtenerMantenimientoPorId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mantenimiento_id", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        mantenimiento = new Mantenimiento
                        {
                            MantenimientoId = reader.GetInt32(reader.GetOrdinal("mantenimiento_id")),
                            VehiculoId = reader.GetInt32(reader.GetOrdinal("vehiculo_id")),
                            ServicioId = reader.GetInt32(reader.GetOrdinal("servicio_id")),
                            FechaProgramada = reader["fecha_programada"] as DateTime?,
                            FechaEntrega = reader["fecha_entrega"] as DateTime?,
                            Estado = reader.GetString(reader.GetOrdinal("estado")),
                            Notas = reader["notas"] as string
                        };
                    }
                }
            }

            return mantenimiento;
        }

        public async Task RegistrarMantenimientoAsync(Mantenimiento m)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("RegistrarMantenimiento", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@vehiculo_id", m.VehiculoId);
                command.Parameters.AddWithValue("@servicio_id", m.ServicioId);
                command.Parameters.AddWithValue("@fecha_programada",
                    (object?)m.FechaProgramada ?? DBNull.Value);
                command.Parameters.AddWithValue("@fecha_entrega",
                    (object?)m.FechaEntrega ?? DBNull.Value);
                command.Parameters.AddWithValue("@estado", m.Estado ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@notas",
                    (object?)m.Notas ?? DBNull.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }



        public async Task ActualizarMantenimientoAsync(Mantenimiento m)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("ActualizarMantenimiento", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@mantenimiento_id", m.MantenimientoId);
                command.Parameters.AddWithValue("@vehiculo_id", m.VehiculoId);
                command.Parameters.AddWithValue("@servicio_id", m.ServicioId);
                command.Parameters.AddWithValue("@fecha_programada", (object?)m.FechaProgramada ?? DBNull.Value);
                command.Parameters.AddWithValue("@fecha_entrega", (object?)m.FechaEntrega ?? DBNull.Value);
                command.Parameters.AddWithValue("@estado", m.Estado);
                command.Parameters.AddWithValue("@notas", (object?)m.Notas ?? DBNull.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task EliminarMantenimientoAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("EliminarMantenimiento", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mantenimiento_id", id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
