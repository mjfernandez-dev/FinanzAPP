using FinanzAPP.Models;
using FinanzAPP.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;

namespace FinanzAPP.Views
{
    public sealed partial class ObjetivosPage : Page
    {
        public ObjetivosViewModel ViewModel { get; }

        public ObjetivosPage()
        {
            this.InitializeComponent();
            ViewModel = new ObjetivosViewModel();
            DataContext = ViewModel;
        }

        private async void AportarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var objetivo = button?.Tag as Objetivo;

            if (objetivo == null) return;

            ViewModel.ObjetivoSeleccionado = objetivo;

            ContentDialog dialog = new ContentDialog
            {
                Title = $"Aportar a: {objetivo.Nombre}",
                PrimaryButtonText = "Aportar",
                CloseButtonText = "Cancelar",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var stackPanel = new StackPanel { Spacing = 12 };

            var infoText = new TextBlock
            {
                Text = $"Meta: ${objetivo.MontoTotal}\nAhorrado: ${objetivo.MontoAhorrado}\nFalta: ${objetivo.MontoRestante}",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            stackPanel.Children.Add(infoText);

            var montoBox = new TextBox
            {
                Header = "Monto a aportar",
                PlaceholderText = "0.00",
                InputScope = new InputScope
                {
                    Names = { new InputScopeName(InputScopeNameValue.Number) }
                }
            };

            montoBox.TextChanged += (s, args) =>
            {
                ViewModel.MontoAporteTexto = montoBox.Text;
            };

            stackPanel.Children.Add(montoBox);

            dialog.Content = stackPanel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.RealizarAporteCommand.Execute(null);
            }
        }
    }
}