using FinanzAPP.Models;
using FinanzAPP.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FinanzAPP.Views
{
    public sealed partial class TransaccionesPage : Page
    {
        public TransaccionesViewModel ViewModel { get; }

        public TransaccionesPage()
        {
            this.InitializeComponent();
            ViewModel = new TransaccionesViewModel();
            DataContext = ViewModel;
        }

        private async void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var transaccion = button?.Tag as Transaccion;

            if (transaccion == null) return;

            // Mostrar diálogo de confirmación
            ContentDialog dialog = new ContentDialog
            {
                Title = "Eliminar Transacción",
                Content = $"¿Estás seguro de eliminar '{transaccion.Descripcion}' por ${transaccion.Monto}?",
                PrimaryButtonText = "Eliminar",
                CloseButtonText = "Cancelar",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.EliminarTransaccionCommand.Execute(transaccion);
            }
        }
    }
}