using System;

namespace FinanzAPP.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public CategoriaTransaccion Categoria { get; set; }
        public TipoTransaccion Tipo { get; set; }
    }

    public enum CategoriaTransaccion
    {
        GastosFijos,
        Gustos,
        AhorroInversion
    }

    public enum TipoTransaccion
    {
        Ingreso,
        Egreso
    }
}