using System;

namespace FinanzAPP.Models
{
    public class Aporte
    {
        public int Id { get; set; }
        public int ObjetivoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Nota { get; set; } = string.Empty;

        // Relación
        public Objetivo Objetivo { get; set; } = null!;
    }
}