using System;

namespace FinanzAPP.Models
{
    public class Ingreso
    {
        public int Id { get; set; }
        public decimal MontoMensual { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; } = true; // Solo uno activo a la vez
    }
}