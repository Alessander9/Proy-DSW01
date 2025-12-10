using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSW01.Data
{
    public class ServicioRepository
    {
        private readonly string _connectionString;

        public ServicioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // LISTAR
        public async Task<List<Servicio>> ListarServiciosAsync()
        {
            var lista = new List<Servicio>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ListarServicios", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Servicio
                {
                    ServicioId = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    TiempoEstimadoHoras = dr.IsDBNull(4) ? null : dr.GetDecimal(4),
                    Categoria = dr.IsDBNull(5) ? "" : dr.GetString(5)
                });
            }

            return lista;
        }

        // OBTENER POR ID
        public async Task<Servicio> ObtenerServicioPorIdAsync(int id)
        {
            Servicio servicio = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ObtenerServicioPorId", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@servicio_id", id);

            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                servicio = new Servicio
                {
                    ServicioId = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    TiempoEstimadoHoras = dr.IsDBNull(4) ? null : dr.GetDecimal(4),
                    Categoria = dr.IsDBNull(5) ? "" : dr.GetString(5)
                };
            }

            return servicio;
        }

        // REGISTRAR
        public async Task RegistrarServicioAsync(Servicio s)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("RegistrarServicio", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object?)s.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio_base", s.PrecioBase);
            cmd.Parameters.AddWithValue("@tiempo_estimado", (object?)s.TiempoEstimadoHoras ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@categoria", (object?)s.Categoria ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ACTUALIZAR
        public async Task ActualizarServicioAsync(Servicio s)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ActualizarServicio", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@servicio_id", s.ServicioId);
            cmd.Parameters.AddWithValue("@nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object?)s.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio_base", s.PrecioBase);
            cmd.Parameters.AddWithValue("@tiempo_estimado", (object?)s.TiempoEstimadoHoras ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@categoria", (object?)s.Categoria ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ELIMINAR
        public async Task EliminarServicioAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("EliminarServicio", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@servicio_id", id);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
