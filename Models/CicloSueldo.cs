using System;
using System.Collections.Generic;

namespace FinanzAPP.Models
{
    public class CicloSueldo
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }  // null = ciclo actual
        public decimal SueldoCobrado { get; set; }
        public bool EsCicloActivo { get; set; }

        // Navegación
        public List<Transaccion> Transacciones { get; set; }

        // Propiedades calculadas
        public decimal TotalGastos => Transacciones?
            .Where(t => t.Tipo == TipoTransaccion.Gasto)
            .Sum(t => t.Monto) ?? 0;

        public decimal TotalIngresos => Transacciones?
            .Where(t => t.Tipo == TipoTransaccion.Ingreso)
            .Sum(t => t.Monto) ?? 0;

        public decimal Balance => TotalIngresos - TotalGastos;

        public int DiasTranscurridos =>
            (DateTime.Now - FechaInicio).Days;

        public decimal GastoDiarioPromedio =>
            DiasTranscurridos > 0 ? TotalGastos / DiasTranscurridos : 0;
    }
}