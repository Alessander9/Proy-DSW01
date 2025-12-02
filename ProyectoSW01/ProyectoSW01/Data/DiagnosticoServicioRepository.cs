using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class DiagnosticoServicioRepository
    {
        private readonly string _connectionString;

        public DiagnosticoServicioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        public async Task<List<DiagnosticoServicio>> ListarPorDiagnosticoAsync(int diagnosticoId)
        {
            var lista = new List<DiagnosticoServicio>();

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ListarServiciosPorDiagnostico", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@diagnostico_id", diagnosticoId);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new DiagnosticoServicio
                {
                    DiagServId = (int)dr["diag_serv_id"],
                    DiagnosticoId = (int)dr["diagnostico_id"],
                    ServicioId = (int)dr["servicio_id"],
                    Precio = (decimal)dr["precio"],
                    ServicioNombre = dr["nombre"].ToString()
                });
            }

            return lista;
        }
    }
}
