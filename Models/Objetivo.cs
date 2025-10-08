using System;

namespace FinanzAPP.Models
{
    public class Objetivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public decimal MontoAhorrado { get; set; }
        public CategoriaObjetivo Categoria { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaObjetivo { get; set; }
        public bool Completado { get; set; }

        // Calculado
        public decimal Progreso => MontoTotal > 0 ? (MontoAhorrado / MontoTotal) * 100 : 0;
        public decimal MontoRestante => MontoTotal - MontoAhorrado;
        public decimal MontoRestanteProgreso => 100 - Progreso;
    }

    public enum CategoriaObjetivo
    {
        Deseo,      // 30%
        Ahorro      // 20%
    }
}