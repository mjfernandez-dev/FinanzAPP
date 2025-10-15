using FinanzAPP.Models;
using System;

namespace FinanzAPP.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public CategoriaGasto Categoria { get; set; }
        public int? CicloSueldoId { get; set; }
        public CicloSueldo? CicloSueldo { get; set; }
    }

    public enum CategoriaGasto
    {
        Necesidades,  // 50%
        Deseos        // 30%
    }
}