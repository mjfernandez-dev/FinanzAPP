using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;
using Microsoft.UI.Xaml;

namespace FinanzAPP.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        private string _ingresoTexto = string.Empty;
        public string IngresoTexto
        {
            get => _ingresoTexto;
            set
            {
                SetProperty(ref _ingresoTexto, value);
                ActualizarVistaPrevia();
            }
        }

        private Visibility _mostrarVistaPrevia = Visibility.Collapsed;
        public Visibility MostrarVistaPrevia
        {
            get => _mostrarVistaPrevia;
            set => SetProperty(ref _mostrarVistaPrevia, value);
        }

        private decimal _vistaPreviaNecesidades;
        public decimal VistaPreviaNecesidades
        {
            get => _vistaPreviaNecesidades;
            set => SetProperty(ref _vistaPreviaNecesidades, value);
        }

        private decimal _vistaPreviaDeseos;
        public decimal VistaPreviaDeseos
        {
            get => _vistaPreviaDeseos;
            set => SetProperty(ref _vistaPreviaDeseos, value);
        }

        private decimal _vistaPreviaAhorro;
        public decimal VistaPreviaAhorro
        {
            get => _vistaPreviaAhorro;
            set => SetProperty(ref _vistaPreviaAhorro, value);
        }

        public ConfiguracionViewModel()
        {
            CargarIngresoActual();
        }

        private void CargarIngresoActual()
        {
            using var db = new AppDbContext();
            var ingresoActivo = db.Ingresos.FirstOrDefault(i => i.Activo);

            if (ingresoActivo != null)
            {
                IngresoTexto = ingresoActivo.MontoMensual.ToString();
            }
        }

        private void ActualizarVistaPrevia()
        {
            if (decimal.TryParse(IngresoTexto, out decimal monto) && monto > 0)
            {
                VistaPreviaNecesidades = monto * 0.5m;
                VistaPreviaDeseos = monto * 0.3m;
                VistaPreviaAhorro = monto * 0.2m;
                MostrarVistaPrevia = Visibility.Visible;
            }
            else
            {
                MostrarVistaPrevia = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private void GuardarIngreso()
        {
            if (!decimal.TryParse(IngresoTexto, out decimal monto) || monto <= 0)
                return;

            using var db = new AppDbContext();

            // Desactivar todos los ingresos anteriores
            var ingresosAnteriores = db.Ingresos.Where(i => i.Activo).ToList();
            foreach (var ingreso in ingresosAnteriores)
            {
                ingreso.Activo = false;
            }

            // Crear nuevo ingreso activo
            var nuevoIngreso = new Ingreso
            {
                MontoMensual = monto,
                FechaRegistro = System.DateTime.Now,
                Activo = true
            };

            db.Ingresos.Add(nuevoIngreso);
            db.SaveChanges();

            // Aquí podrías mostrar un mensaje de éxito
        }
    }
}