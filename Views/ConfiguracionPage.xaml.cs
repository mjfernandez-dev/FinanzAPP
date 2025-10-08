using Microsoft.UI.Xaml.Controls;
using FinanzAPP.ViewModels;

namespace FinanzAPP.Views
{
    public sealed partial class ConfiguracionPage : Page
    {
        public ConfiguracionViewModel ViewModel { get; }

        public ConfiguracionPage()
        {
            this.InitializeComponent();
            ViewModel = new ConfiguracionViewModel();
            DataContext = ViewModel;
        }
    }
}