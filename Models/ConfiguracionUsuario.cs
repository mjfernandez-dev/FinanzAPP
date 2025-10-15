using System;
using System.Collections.Generic;

namespace FinanzAPP.Models
{
    public class ConfiguracionUsuario
    {
        public int Id { get; set; }
        public int DiaCobro { get; set; }  // Día del mes que cobra (15, 30, etc.)
        public decimal SueldoBase { get; set; }
        public bool UsarCiclosSueldo { get; set; } = true;

        // Distribución automática
        public decimal PorcentajeAhorro { get; set; } = 20;
        public decimal PorcentajeNecesidades { get; set; } = 50;
        public decimal PorcentajeDeseos { get; set; } = 30;
    }
}