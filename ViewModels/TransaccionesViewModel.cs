using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;

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

        private TipoTransaccion _tipoSeleccionado;
        public TipoTransaccion TipoSeleccionado
        {
            get => _tipoSeleccionado;
            set => SetProperty(ref _tipoSeleccionado, value);
        }

        private CategoriaTransaccion _categoriaSeleccionada;
        public CategoriaTransaccion CategoriaSeleccionada
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

        public ObservableCollection<Transaccion> Transacciones { get; set; }
        public ObservableCollection<string> TiposTransaccion { get; set; }
        public ObservableCollection<string> Categorias { get; set; }

        public TransaccionesViewModel()
        {
            Transacciones = new ObservableCollection<Transaccion>();

            TiposTransaccion = new ObservableCollection<string>
            {
                "Ingreso",
                "Egreso"
            };

            Categorias = new ObservableCollection<string>
            {
                "GastosFijos",
                "Gustos",
                "AhorroInversion"
            };

            CargarTransacciones();
        }

        private void CargarTransacciones()
        {
            using var db = new AppDbContext();
            var transacciones = db.Transacciones.OrderByDescending(t => t.Fecha).ToList();

            Transacciones.Clear();
            foreach (var t in transacciones)
            {
                Transacciones.Add(t);
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
                Tipo = Enum.Parse<TipoTransaccion>(TipoSeleccionado.ToString()),
                Categoria = Enum.Parse<CategoriaTransaccion>(CategoriaSeleccionada.ToString())
            };

            using (var db = new AppDbContext())
            {
                db.Transacciones.Add(nuevaTransaccion);
                db.SaveChanges();
            }

            // Limpiar formulario
            Descripcion = string.Empty;
            MontoTexto = string.Empty;

            CargarTransacciones();
        }
    }
}