using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;
using Microsoft.UI.Xaml;

namespace FinanzAPP.ViewModels
{
    public partial class TransaccionesViewModel : ObservableObject
    {
        private string _descripcion = string.Empty;
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        private string _montoTexto = string.Empty;
        public string MontoTexto
        {
            get => _montoTexto;
            set => SetProperty(ref _montoTexto, value);
        }

        private CategoriaGasto _categoriaSeleccionada;
        public CategoriaGasto CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set => SetProperty(ref _categoriaSeleccionada, value);
        }

        private DateTimeOffset _fecha = DateTimeOffset.Now;
        public DateTimeOffset Fecha
        {
            get => _fecha;
            set => SetProperty(ref _fecha, value);
        }

        // Propiedades para presupuesto disponible
        private decimal _disponibleNecesidades;
        public decimal DisponibleNecesidades
        {
            get => _disponibleNecesidades;
            set => SetProperty(ref _disponibleNecesidades, value);
        }

        private decimal _disponibleDeseos;
        public decimal DisponibleDeseos
        {
            get => _disponibleDeseos;
            set => SetProperty(ref _disponibleDeseos, value);
        }

        private Visibility _mostrarMensajeVacio = Visibility.Collapsed;
        public Visibility MostrarMensajeVacio
        {
            get => _mostrarMensajeVacio;
            set => SetProperty(ref _mostrarMensajeVacio, value);
        }

        public ObservableCollection<Transaccion> Transacciones { get; set; }
        public ObservableCollection<string> Categorias { get; set; }

        public TransaccionesViewModel()
        {
            Transacciones = new ObservableCollection<Transaccion>();

            Categorias = new ObservableCollection<string>
            {
                "Necesidades",
                "Deseos"
            };

            CargarTransacciones();
            ActualizarPresupuestoDisponible();
        }

        private void CargarTransacciones()
        {
            using var db = new AppDbContext();
            var transacciones = db.Transacciones
                .OrderByDescending(t => t.Fecha)
                .ToList();

            Transacciones.Clear();
            foreach (var t in transacciones)
            {
                Transacciones.Add(t);
            }

            MostrarMensajeVacio = Transacciones.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ActualizarPresupuestoDisponible()
        {
            using var db = new AppDbContext();

            // Obtener ingreso activo
            var ingresoActivo = db.Ingresos.FirstOrDefault(i => i.Activo);

            if (ingresoActivo != null)
            {
                decimal necesidades = ingresoActivo.MontoMensual * 0.5m;
                decimal deseos = ingresoActivo.MontoMensual * 0.3m;

                // Calcular gastos del mes actual
                var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var transaccionesMes = db.Transacciones
                    .Where(t => t.Fecha >= inicioMes)
                    .ToList();

                decimal gastadoNecesidades = transaccionesMes
                    .Where(t => t.Categoria == CategoriaGasto.Necesidades)
                    .Sum(t => t.Monto);

                decimal gastadoDeseos = transaccionesMes
                    .Where(t => t.Categoria == CategoriaGasto.Deseos)
                    .Sum(t => t.Monto);

                DisponibleNecesidades = necesidades - gastadoNecesidades;
                DisponibleDeseos = deseos - gastadoDeseos;
            }
            else
            {
                DisponibleNecesidades = 0;
                DisponibleDeseos = 0;
            }
        }

        [RelayCommand]
        private void AgregarTransaccion()
        {
            if (string.IsNullOrWhiteSpace(Descripcion) || string.IsNullOrWhiteSpace(MontoTexto))
                return;

            if (!decimal.TryParse(MontoTexto, out decimal monto))
                return;

            var nuevaTransaccion = new Transaccion
            {
                Descripcion = Descripcion,
                Monto = monto,
                Fecha = Fecha.DateTime,
                Categoria = Enum.Parse<CategoriaGasto>(CategoriaSeleccionada.ToString())
            };

            using (var db = new AppDbContext())
            {
                db.Transacciones.Add(nuevaTransaccion);
                db.SaveChanges();
            }

            Descripcion = string.Empty;
            MontoTexto = string.Empty;
            Fecha = DateTimeOffset.Now;

            CargarTransacciones();
            ActualizarPresupuestoDisponible();
        }

        [RelayCommand]
        private void EliminarTransaccion(Transaccion transaccion)
        {
            using (var db = new AppDbContext())
            {
                var transaccionDb = db.Transacciones.Find(transaccion.Id);
                if (transaccionDb != null)
                {
                    db.Transacciones.Remove(transaccionDb);
                    db.SaveChanges();
                }
            }

            CargarTransacciones();
            ActualizarPresupuestoDisponible();
        }
    }
}