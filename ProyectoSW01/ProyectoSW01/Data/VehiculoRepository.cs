using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;


namespace ProyectoSW01.Data
{
    public class VehiculoRepository
    {
        private readonly string _connectionString;

        public VehiculoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // LISTAR
        public async Task<List<Vehiculo>> ListarVehiculosAsync()
        {
            var lista = new List<Vehiculo>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("ListarVehiculos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Vehiculo
                        {
                            VehiculoId = (int)reader["vehiculo_id"],
                            ClienteId = (int)reader["cliente_id"],
                            Marca = reader["marca"].ToString(),
                            Modelo = reader["modelo"].ToString(),
                            Anio = (int)reader["anio"],
                            Placa = reader["placa"].ToString(),
                            Vin = reader["vin"]?.ToString(),
                            FechaRegistro = (DateTime)reader["fecha_registro"],
                            ClienteNombreCompleto = reader["ClienteNombreCompleto"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }

        // OBTENER POR ID
        public async Task<Vehiculo?> ObtenerVehiculoPorIdAsync(int vehiculoId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerVehiculoPorId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@vehiculo_id", vehiculoId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Vehiculo
                {
                    VehiculoId = (int)reader["vehiculo_id"],
                    ClienteId = (int)reader["cliente_id"],
                    Marca = reader["marca"].ToString(),
                    Modelo = reader["modelo"].ToString(),
                    Anio = (int)reader["anio"],
                    Placa = reader["placa"].ToString(),
                    Vin = reader["vin"]?.ToString(),
                    FechaRegistro = (DateTime)reader["fecha_registro"],
                    ClienteNombreCompleto = reader["ClienteNombreCompleto"]?.ToString()
                };
            }

            return null;
        }

        // REGISTRAR
        public async Task RegistrarVehiculoAsync(Vehiculo v)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("RegistrarVehiculo", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@cliente_id", v.ClienteId);
                command.Parameters.AddWithValue("@marca", v.Marca);
                command.Parameters.AddWithValue("@modelo", v.Modelo);
                command.Parameters.AddWithValue("@anio", v.Anio);
                command.Parameters.AddWithValue("@placa", v.Placa);
                command.Parameters.AddWithValue("@vin", (object?)v.Vin ?? DBNull.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }




        // ACTUALIZAR
        public async Task ActualizarVehiculoAsync(Vehiculo v)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ActualizarVehiculo", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@vehiculo_id", v.VehiculoId);
            command.Parameters.AddWithValue("@cliente_id", v.ClienteId);
            command.Parameters.AddWithValue("@marca", v.Marca);
            command.Parameters.AddWithValue("@modelo", v.Modelo);
            command.Parameters.AddWithValue("@anio", v.Anio);
            command.Parameters.AddWithValue("@placa", v.Placa);
            command.Parameters.AddWithValue("@vin", (object?)v.Vin ?? DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ELIMINAR
        public async Task EliminarVehiculoAsync(int vehiculoId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("EliminarVehiculo", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@vehiculo_id", vehiculoId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        public async Task<List<(int Id, string Texto)>> ListarVehiculosParaDiagnosticoAsync()
        {
            var lista = new List<(int Id, string Texto)>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ListarVehiculosParaDiagnostico", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int id = (int)reader["vehiculo_id"];
                string placa = reader["placa"]?.ToString();
                string marca = reader["marca"]?.ToString();
                string modelo = reader["modelo"]?.ToString();

                string texto = $"{placa} - {marca} {modelo}";

                lista.Add((id, texto));
            }

            return lista;
        }

        public async Task<List<Vehiculo>> BuscarVehiculosAsync(string texto)
        {
            var lista = new List<Vehiculo>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("BuscarVehiculos", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@texto", texto ?? string.Empty);

            await conn.OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                var v = new Vehiculo
                {
                    // nombres de columnas tal cual en la BD
                    VehiculoId = Convert.ToInt32(dr["vehiculo_id"]),
                    ClienteId = Convert.ToInt32(dr["cliente_id"]),
                    Marca = dr["marca"] == DBNull.Value ? string.Empty : dr["marca"].ToString(),
                    Modelo = dr["modelo"] == DBNull.Value ? string.Empty : dr["modelo"].ToString(),
                    Anio = dr["anio"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["anio"]),
                    Placa = dr["placa"] == DBNull.Value ? string.Empty : dr["placa"].ToString(),
                    Vin = dr["vin"] == DBNull.Value ? string.Empty : dr["vin"].ToString(),
                    FechaRegistro = dr["fecha_registro"] == DBNull.Value
                                        ? DateTime.MinValue
                                        : Convert.ToDateTime(dr["fecha_registro"]),
                    ClienteNombreCompleto = dr["cliente_nombre_completo"] == DBNull.Value
                                                ? null
                                                : dr["cliente_nombre_completo"].ToString()
                };

                lista.Add(v);
            }

            return lista;
        }

    }
}
