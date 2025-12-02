using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class OrdenTrabajoRepository
    {
        private readonly string _connectionString;

        public OrdenTrabajoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // ============================================
        // LISTAR TODAS LAS ÓRDENES
        // Usa el SP: ListarOrdenes
        // ============================================
        public async Task<List<OrdenTrabajo>> ListarOrdenesAsync()
        {
            var lista = new List<OrdenTrabajo>();

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ListarOrdenes", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new OrdenTrabajo
                {
                    OrdenId = (int)dr["orden_id"],
                    VehiculoId = (int)dr["vehiculo_id"],
                    DiagnosticoId = dr["diagnostico_id"] == DBNull.Value
                        ? (int?)null
                        : (int)dr["diagnostico_id"],
                    MecanicoId = dr["mecanico_id"] == DBNull.Value
                        ? (int?)null
                        : (int)dr["mecanico_id"],
                    Fecha = (DateTime)dr["fecha"],
                    Estado = dr["estado"].ToString(),
                    Total = dr["total"] == DBNull.Value ? 0m : (decimal)dr["total"],
                    Observaciones = dr["observaciones"] == DBNull.Value
                        ? string.Empty
                        : dr["observaciones"].ToString()
                });
            }

            return lista;
        }

        // ============================================
        // OBTENER UNA ORDEN POR ID
        // (consulta directa a la tabla)
        // ============================================
        public async Task<OrdenTrabajo?> ObtenerPorIdAsync(int ordenId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT orden_id, vehiculo_id, diagnostico_id, mecanico_id,
                       fecha, estado, total, observaciones
                FROM orden_trabajo
                WHERE orden_id = @id;
            ", cn);

            cmd.Parameters.AddWithValue("@id", ordenId);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                return new OrdenTrabajo
                {
                    OrdenId = (int)dr["orden_id"],
                    VehiculoId = (int)dr["vehiculo_id"],
                    DiagnosticoId = dr["diagnostico_id"] == DBNull.Value
                        ? (int?)null
                        : (int)dr["diagnostico_id"],
                    MecanicoId = dr["mecanico_id"] == DBNull.Value
                        ? (int?)null
                        : (int)dr["mecanico_id"],
                    Fecha = (DateTime)dr["fecha"],
                    Estado = dr["estado"].ToString(),
                    Total = dr["total"] == DBNull.Value ? 0m : (decimal)dr["total"],
                    Observaciones = dr["observaciones"] == DBNull.Value
                        ? string.Empty
                        : dr["observaciones"].ToString()
                };
            }

            return null;
        }

        // ============================================
        // CAMBIAR ESTADO (usa SP CambiarEstadoOrden)
        // ============================================
        public async Task CambiarEstadoAsync(int ordenId, string nuevoEstado)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("CambiarEstadoOrden", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@orden_id", ordenId);
            cmd.Parameters.AddWithValue("@nuevo_estado", nuevoEstado);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================
        // ACTUALIZAR TOTAL (usa SP ActualizarTotalOrden)
        // ============================================
        public async Task ActualizarTotalAsync(int ordenId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ActualizarTotalOrden", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@orden_id", ordenId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<OrdenTrabajoConDetalle?> ObtenerConDetalleAsync(int ordenId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ObtenerOrdenConDetalle", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orden_id", ordenId);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            var resultado = new OrdenTrabajoConDetalle();

            // 1er resultset: cabecera
            if (await dr.ReadAsync())
            {
                resultado.Orden = new OrdenTrabajo
                {
                    OrdenId = (int)dr["orden_id"],
                    VehiculoId = (int)dr["vehiculo_id"],
                    DiagnosticoId = dr["diagnostico_id"] == DBNull.Value ? (int?)null : (int)dr["diagnostico_id"],
                    MecanicoId = dr["mecanico_id"] == DBNull.Value ? (int?)null : (int)dr["mecanico_id"],
                    Fecha = (DateTime)dr["fecha"],
                    Estado = dr["estado"].ToString(),
                    Total = dr["total"] == DBNull.Value ? 0m : (decimal)dr["total"],
                    Observaciones = dr["observaciones"] == DBNull.Value ? string.Empty : dr["observaciones"].ToString()
                };
            }
            else
            {
                return null; // no existe la orden
            }

            // 2º resultset: detalle
            if (await dr.NextResultAsync())
            {
                while (await dr.ReadAsync())
                {
                    resultado.Detalles.Add(new OrdenTrabajoDetalle
                    {
                        OrdDetId = (int)dr["ord_det_id"],
                        OrdenId = (int)dr["orden_id"],
                        ServicioId = (int)dr["servicio_id"],
                        ServicioNombre = dr["nombre"].ToString(),
                        Cantidad = (int)dr["cantidad"],
                        Precio = (decimal)dr["precio"],
                        Subtotal = (decimal)dr["subtotal"]
                    });
                }
            }

            return resultado;

        }
    }
}
