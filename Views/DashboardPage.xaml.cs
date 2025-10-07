using Microsoft.UI.Xaml.Controls;
using FinanzAPP.ViewModels;

namespace FinanzAPP.Views
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage()
        {
            this.InitializeComponent();
            ViewModel = new DashboardViewModel();
        }
    }
}