using Microsoft.UI.Xaml.Controls;
using FinanzAPP.ViewModels;

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
    }
}