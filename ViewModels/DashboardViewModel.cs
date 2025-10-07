using CommunityToolkit.Mvvm.ComponentModel;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;

namespace FinanzAPP.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private decimal _totalIngresos;
        public decimal TotalIngresos
        {
            get => _totalIngresos;
            set => SetProperty(ref _totalIngresos, value);
        }

        private decimal _gastosFijos;
        public decimal GastosFijos
        {
            get => _gastosFijos;
            set => SetProperty(ref _gastosFijos, value);
        }

        private decimal _gustos;
        public decimal Gustos
        {
            get => _gustos;
            set => SetProperty(ref _gustos, value);
        }

        private decimal _ahorroInversion;
        public decimal AhorroInversion
        {
            get => _ahorroInversion;
            set => SetProperty(ref _ahorroInversion, value);
        }

        private decimal _porcentajeGastosFijos;
        public decimal PorcentajeGastosFijos
        {
            get => _porcentajeGastosFijos;
            set => SetProperty(ref _porcentajeGastosFijos, value);
        }

        private decimal _porcentajeGustos;
        public decimal PorcentajeGustos
        {
            get => _porcentajeGustos;
            set => SetProperty(ref _porcentajeGustos, value);
        }

        private decimal _porcentajeAhorro;
        public decimal PorcentajeAhorro
        {
            get => _porcentajeAhorro;
            set => SetProperty(ref _porcentajeAhorro, value);
        }

        public DashboardViewModel()
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            using var db = new AppDbContext();

            var transacciones = db.Transacciones.ToList();

            TotalIngresos = transacciones
                .Where(t => t.Tipo == TipoTransaccion.Ingreso)
                .Sum(t => t.Monto);

            GastosFijos = transacciones
                .Where(t => t.Categoria == CategoriaTransaccion.GastosFijos && t.Tipo == TipoTransaccion.Egreso)
                .Sum(t => t.Monto);

            Gustos = transacciones
                .Where(t => t.Categoria == CategoriaTransaccion.Gustos && t.Tipo == TipoTransaccion.Egreso)
                .Sum(t => t.Monto);

            AhorroInversion = transacciones
                .Where(t => t.Categoria == CategoriaTransaccion.AhorroInversion && t.Tipo == TipoTransaccion.Egreso)
                .Sum(t => t.Monto);

            CalcularPorcentajes();
        }

        private void CalcularPorcentajes()
        {
            if (TotalIngresos > 0)
            {
                PorcentajeGastosFijos = (GastosFijos / TotalIngresos) * 100;
                PorcentajeGustos = (Gustos / TotalIngresos) * 100;
                PorcentajeAhorro = (AhorroInversion / TotalIngresos) * 100;
            }
        }
    }
}