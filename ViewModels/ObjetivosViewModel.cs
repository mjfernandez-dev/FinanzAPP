using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using FinanzAPP.Models;
using FinanzAPP.Data;
using System.Linq;
using Microsoft.UI.Xaml;
using System;

namespace FinanzAPP.ViewModels
{
    public partial class ObjetivosViewModel : ObservableObject
    {
        private string _nombreObjetivo = string.Empty;

        private Objetivo? _objetivoSeleccionado;
        public Objetivo? ObjetivoSeleccionado
        {
            get => _objetivoSeleccionado;
            set => SetProperty(ref _objetivoSeleccionado, value);
        }

        private string _montoAporteTexto = string.Empty;
        public string MontoAporteTexto
        {
            get => _montoAporteTexto;
            set => SetProperty(ref _montoAporteTexto, value);
        }

        public string NombreObjetivo
        {
            get => _nombreObjetivo;
            set => SetProperty(ref _nombreObjetivo, value);
        }

        private string _montoObjetivoTexto = string.Empty;
        public string MontoObjetivoTexto
        {
            get => _montoObjetivoTexto;
            set
            {
                SetProperty(ref _montoObjetivoTexto, value);
                CalcularTiempoEstimado();
            }
        }

        private string _montoMensualTexto = string.Empty;
        public string MontoMensualTexto
        {
            get => _montoMensualTexto;
            set
            {
                SetProperty(ref _montoMensualTexto, value);
                CalcularTiempoEstimado();
            }
        }

        private CategoriaObjetivo _categoriaSeleccionada;
        public CategoriaObjetivo CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set => SetProperty(ref _categoriaSeleccionada, value);
        }

        private Visibility _mostrarTiempoEstimado = Visibility.Collapsed;
        public Visibility MostrarTiempoEstimado
        {
            get => _mostrarTiempoEstimado;
            set => SetProperty(ref _mostrarTiempoEstimado, value);
        }

        private string _tiempoEstimado = string.Empty;
        public string TiempoEstimado
        {
            get => _tiempoEstimado;
            set => SetProperty(ref _tiempoEstimado, value);
        }

        public ObservableCollection<Objetivo> Objetivos { get; set; }
        public ObservableCollection<string> CategoriasObjetivo { get; set; }

        public ObjetivosViewModel()
        {
            Objetivos = new ObservableCollection<Objetivo>();

            CategoriasObjetivo = new ObservableCollection<string>
            {
                "Deseo",
                "Ahorro"
            };

            CargarObjetivos();
        }

        private void CargarObjetivos()
        {
            using var db = new AppDbContext();
            var objetivos = db.Objetivos
                .Where(o => !o.Completado)
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();

            Objetivos.Clear();
            foreach (var obj in objetivos)
            {
                Objetivos.Add(obj);
            }
        }

        private void CalcularTiempoEstimado()
        {
            if (decimal.TryParse(MontoObjetivoTexto, out decimal montoTotal) &&
                decimal.TryParse(MontoMensualTexto, out decimal montoMensual) &&
                montoTotal > 0 && montoMensual > 0)
            {
                int meses = (int)Math.Ceiling(montoTotal / montoMensual);

                if (meses == 1)
                    TiempoEstimado = "1 mes";
                else if (meses < 12)
                    TiempoEstimado = $"{meses} meses";
                else
                {
                    int años = meses / 12;
                    int mesesRestantes = meses % 12;

                    if (mesesRestantes == 0)
                        TiempoEstimado = años == 1 ? "1 año" : $"{años} años";
                    else
                        TiempoEstimado = años == 1
                            ? $"1 año y {mesesRestantes} meses"
                            : $"{años} años y {mesesRestantes} meses";
                }

                MostrarTiempoEstimado = Visibility.Visible;
            }
            else
            {
                MostrarTiempoEstimado = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private void CrearObjetivo()
        {
            if (string.IsNullOrWhiteSpace(NombreObjetivo) ||
                string.IsNullOrWhiteSpace(MontoObjetivoTexto))
                return;

            if (!decimal.TryParse(MontoObjetivoTexto, out decimal monto) || monto <= 0)
                return;

            var nuevoObjetivo = new Objetivo
            {
                Nombre = NombreObjetivo,
                MontoTotal = monto,
                MontoAhorrado = 0,
                Categoria = Enum.Parse<CategoriaObjetivo>(CategoriaSeleccionada.ToString()),
                FechaCreacion = DateTime.Now,
                Completado = false
            };

            using (var db = new AppDbContext())
            {
                db.Objetivos.Add(nuevoObjetivo);
                db.SaveChanges();
            }

            // Limpiar formulario
            NombreObjetivo = string.Empty;
            MontoObjetivoTexto = string.Empty;
            MontoMensualTexto = string.Empty;
            MostrarTiempoEstimado = Visibility.Collapsed;

            CargarObjetivos();
        }

        [RelayCommand]
        private void RealizarAporte()
        {
            if (ObjetivoSeleccionado == null ||
                string.IsNullOrWhiteSpace(MontoAporteTexto))
                return;

            if (!decimal.TryParse(MontoAporteTexto, out decimal montoAporte) || montoAporte <= 0)
                return;

            using (var db = new AppDbContext())
            {
                var objetivo = db.Objetivos.Find(ObjetivoSeleccionado.Id);
                if (objetivo == null) return;

                // Registrar el aporte
                var nuevoAporte = new Aporte
                {
                    ObjetivoId = objetivo.Id,
                    Monto = montoAporte,
                    Fecha = DateTime.Now
                };

                db.Aportes.Add(nuevoAporte);

                // Actualizar el objetivo
                objetivo.MontoAhorrado += montoAporte;

                // Marcar como completado si alcanzó la meta
                if (objetivo.MontoAhorrado >= objetivo.MontoTotal)
                {
                    objetivo.Completado = true;
                }

                db.SaveChanges();
            }

            MontoAporteTexto = string.Empty;
            ObjetivoSeleccionado = null;
            CargarObjetivos();
        }
    }
}