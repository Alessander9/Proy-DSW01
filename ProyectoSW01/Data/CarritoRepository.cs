using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoSW01.Models;

namespace ProyectoSW01.Data
{
    public class CarritoRepository
    {
        private readonly string _connectionString;

        public CarritoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // =============================
        // Obtener carrito activo o crearlo
        // =============================
        public async Task<int> ObtenerOCrearCarritoAsync(int vehiculoId)
        {
            using var cn = new SqlConnection(_connectionString);

            // 1) Buscar carrito existente
            using (var cmd = new SqlCommand(@"
                SELECT TOP 1 carrito_id
                FROM carrito
                WHERE vehiculo_id = @vehiculo_id
                ORDER BY creado_en DESC;
            ", cn))
            {
                cmd.Parameters.AddWithValue("@vehiculo_id", vehiculoId);
                await cn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            // 2) Si no existe, crear uno nuevo con SP CrearCarritoParaVehiculo
            using var cn2 = new SqlConnection(_connectionString);
            using var cmd2 = new SqlCommand("CrearCarritoParaVehiculo", cn2);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@vehiculo_id", vehiculoId);

            await cn2.OpenAsync();
            var carritoId = 0;
            using (var dr = await cmd2.ExecuteReaderAsync())
            {
                if (await dr.ReadAsync())
                {
                    carritoId = Convert.ToInt32(dr["carrito_id"]);
                }
            }

            return carritoId;
        }

        // =============================
        // Listar items del carrito por vehículo
        // (consulta directa, incluye car_det_id)
        // =============================
        public async Task<List<CarritoItemViewModel>> ListarPorVehiculoAsync(int vehiculoId)
        {
            var lista = new List<CarritoItemViewModel>();

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT 
                    cd.car_det_id,
                    cd.carrito_id,
                    cd.servicio_id,
                    s.nombre,
                    cd.cantidad,
                    cd.precio
                FROM carrito c
                INNER JOIN carrito_detalle cd ON cd.carrito_id = c.carrito_id
                INNER JOIN servicios s ON s.servicio_id = cd.servicio_id
                WHERE c.vehiculo_id = @vehiculo_id
                ORDER BY cd.car_det_id DESC;
            ", cn);

            cmd.Parameters.AddWithValue("@vehiculo_id", vehiculoId);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new CarritoItemViewModel
                {
                    CarDetId = (int)dr["car_det_id"],
                    CarritoId = (int)dr["carrito_id"],
                    ServicioId = (int)dr["servicio_id"],
                    ServicioNombre = dr["nombre"].ToString(),
                    Cantidad = (int)dr["cantidad"],
                    Precio = (decimal)dr["precio"]
                });
            }

            return lista;
        }

        // =============================
        // Agregar item al carrito
        // Usa SP AgregarItemAlCarrito
        // =============================
        public async Task AgregarItemAsync(int carritoId, int servicioId, int cantidad, decimal precio)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("AgregarItemAlCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@carrito_id", carritoId);
            cmd.Parameters.AddWithValue("@servicio_id", servicioId);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.Parameters.AddWithValue("@precio", precio);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // =============================
        // Eliminar item del carrito
        // =============================
        public async Task EliminarItemAsync(int carDetId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("EliminarItemDelCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@car_det_id", carDetId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // =============================
        // Vaciar carrito
        // =============================
        public async Task VaciarCarritoAsync(int carritoId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("VaciarCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@carrito_id", carritoId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // =============================
        // Crear orden desde carrito
        // (usa SP CrearOrdenDesdeCarrito)
        // =============================
        public async Task<int> CrearOrdenDesdeCarritoAsync(int carritoId, int vehiculoId, int? mecanicoId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("CrearOrdenDesdeCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@carrito_id", carritoId);
            cmd.Parameters.AddWithValue("@vehiculo_id", vehiculoId);
            cmd.Parameters.AddWithValue("@mecanico_id", (object?)mecanicoId ?? DBNull.Value);

            await cn.OpenAsync();
            var ordenId = 0;

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                ordenId = Convert.ToInt32(dr["orden_id"]);
            }

            return ordenId;
        }

        public async Task<List<RepuestoCarritoItemViewModel>> ListarRepuestosPorVehiculoAsync(int vehiculoId)
        {
            var lista = new List<RepuestoCarritoItemViewModel>();

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT 
            cr.car_rep_id,
            cr.carrito_id,
            cr.repuesto_id,
            r.nombre,
            cr.cantidad,
            cr.precio
        FROM carrito c
        INNER JOIN carrito_detalle_repuesto cr ON cr.carrito_id = c.carrito_id
        INNER JOIN repuestos r ON r.repuesto_id = cr.repuesto_id
        WHERE c.vehiculo_id = @vehiculo_id
        ORDER BY cr.car_rep_id DESC;
    ", cn);

            cmd.Parameters.AddWithValue("@vehiculo_id", vehiculoId);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new RepuestoCarritoItemViewModel
                {
                    CarRepId = (int)dr["car_rep_id"],
                    CarritoId = (int)dr["carrito_id"],
                    RepuestoId = (int)dr["repuesto_id"],
                    RepuestoNombre = dr["nombre"].ToString()!,
                    Cantidad = (int)dr["cantidad"],
                    Precio = (decimal)dr["precio"]
                });
            }

            return lista;
        }

        public async Task AgregarRepuestoAsync(int carritoId, int repuestoId, int cantidad, decimal precio)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("AgregarRepuestoAlCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@carrito_id", carritoId);
            cmd.Parameters.AddWithValue("@repuesto_id", repuestoId);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.Parameters.AddWithValue("@precio", precio);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarRepuestoAsync(int carRepId)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("EliminarRepuestoDelCarrito", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@car_rep_id", carRepId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

    }
}
