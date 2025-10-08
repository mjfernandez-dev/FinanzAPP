using CommunityToolkit.Mvvm.ComponentModel;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;
using System;

namespace FinanzAPP.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private decimal _ingresoMensual;
        public decimal IngresoMensual
        {
            get => _ingresoMensual;
            set => SetProperty(ref _ingresoMensual, value);
        }

        private decimal _necesidades;
        public decimal Necesidades
        {
            get => _necesidades;
            set => SetProperty(ref _necesidades, value);
        }

        private decimal _deseos;
        public decimal Deseos
        {
            get => _deseos;
            set => SetProperty(ref _deseos, value);
        }

        private decimal _ahorro;
        public decimal Ahorro
        {
            get => _ahorro;
            set => SetProperty(ref _ahorro, value);
        }

        private decimal _gastadoNecesidades;
        public decimal GastadoNecesidades
        {
            get => _gastadoNecesidades;
            set => SetProperty(ref _gastadoNecesidades, value);
        }

        private decimal _gastadoDeseos;
        public decimal GastadoDeseos
        {
            get => _gastadoDeseos;
            set => SetProperty(ref _gastadoDeseos, value);
        }

        private decimal _ahorradoTotal;
        public decimal AhorradoTotal
        {
            get => _ahorradoTotal;
            set => SetProperty(ref _ahorradoTotal, value);
        }

        // Disponibles
        public decimal DisponibleNecesidades => Necesidades - GastadoNecesidades;
        public decimal DisponibleDeseos => Deseos - GastadoDeseos;
        public decimal DisponibleAhorro => Ahorro - AhorradoTotal;

        public DashboardViewModel()
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            using var db = new AppDbContext();

            // Obtener ingreso activo
            var ingresoActivo = db.Ingresos.FirstOrDefault(i => i.Activo);

            if (ingresoActivo != null)
            {
                IngresoMensual = ingresoActivo.MontoMensual;
                Necesidades = IngresoMensual * 0.5m;
                Deseos = IngresoMensual * 0.3m;
                Ahorro = IngresoMensual * 0.2m;
            }

            // Calcular gastos del mes actual
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var transaccionesMes = db.Transacciones
                .Where(t => t.Fecha >= inicioMes)
                .ToList();

            GastadoNecesidades = transaccionesMes
                .Where(t => t.Categoria == CategoriaGasto.Necesidades)
                .Sum(t => t.Monto);

            GastadoDeseos = transaccionesMes
                .Where(t => t.Categoria == CategoriaGasto.Deseos)
                .Sum(t => t.Monto);

            // Calcular total ahorrado en objetivos
            AhorradoTotal = db.Objetivos
                .Where(o => !o.Completado)
                .Sum(o => o.MontoAhorrado);
        }
    }
}