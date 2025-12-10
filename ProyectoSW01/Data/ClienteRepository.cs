using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProyectoSW01.Data
{
    public class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // LISTAR CLIENTES
        public async Task<List<Cliente>> ListarClientesAsync()
        {
            var lista = new List<Cliente>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ListarClientes", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new Cliente
                {
                    ClienteId = dr.GetInt32(0),
                    Nombres = dr.GetString(1),
                    Apellidos = dr.GetString(2),
                    Dni = dr.IsDBNull(3) ? null : dr.GetString(3),
                    Telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Correo = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Direccion = dr.IsDBNull(6) ? null : dr.GetString(6),
                    FechaRegistro = dr.GetDateTime(7)
                });
            }
            return lista;
        }

        // BUSCAR CLIENTES
        public async Task<List<Cliente>> BuscarClientesAsync(string texto)
        {
            var lista = new List<Cliente>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("BuscarClientes", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@texto", texto ?? string.Empty);

            await conn.OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new Cliente
                {
                    ClienteId = dr.GetInt32(0),
                    Nombres = dr.GetString(1),
                    Apellidos = dr.GetString(2),
                    Dni = dr.IsDBNull(3) ? null : dr.GetString(3),
                    Telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Correo = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Direccion = dr.IsDBNull(6) ? null : dr.GetString(6),
                    FechaRegistro = dr.GetDateTime(7)
                });
            }

            return lista;
        }


        // REGISTRAR CLIENTE
        public async Task RegistrarClienteAsync(Cliente c)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("RegistrarCliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombres", c.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", c.Apellidos);
            cmd.Parameters.AddWithValue("@dni", (object?)c.Dni ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object?)c.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@correo", (object?)c.Correo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", (object?)c.Direccion ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // OBTENER CLIENTE POR ID
        public async Task<Cliente> ObtenerClientePorIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ObtenerClientePorId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cliente_id", id); 

            await conn.OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return new Cliente
                {
                    ClienteId = dr.GetInt32(0),
                    Nombres = dr.GetString(1),
                    Apellidos = dr.GetString(2),
                    Dni = dr.IsDBNull(3) ? null : dr.GetString(3),
                    Telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Correo = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Direccion = dr.IsDBNull(6) ? null : dr.GetString(6),
                    FechaRegistro = dr.GetDateTime(7)
                };
            }

            return null;
        }

        // ACTUALIZAR CLIENTE
        public async Task ActualizarClienteAsync(Cliente c)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ActualizarCliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cliente_id", c.ClienteId);
            cmd.Parameters.AddWithValue("@nombres", c.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", c.Apellidos);
            cmd.Parameters.AddWithValue("@dni", (object?)c.Dni ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object?)c.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@correo", (object?)c.Correo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", (object?)c.Direccion ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ELIMINAR CLIENTE
        public async Task EliminarClienteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("EliminarCliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cliente_id", id);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // BUSCAR CLIENTES POR NOMBRE
        public async Task<List<Cliente>> BuscarClientesPorNombreAsync(string nombre)
        {
            var lista = new List<Cliente>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("BuscarClientesPorNombre", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", nombre ?? string.Empty);

            await conn.OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new Cliente
                {
                    ClienteId = dr.GetInt32(0),
                    Nombres = dr.GetString(1),
                    Apellidos = dr.GetString(2),
                    Dni = dr.IsDBNull(3) ? null : dr.GetString(3),
                    Telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Correo = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Direccion = dr.IsDBNull(6) ? null : dr.GetString(6),
                    FechaRegistro = dr.GetDateTime(7)
                });
            }

            return lista;
        }
    }
}
