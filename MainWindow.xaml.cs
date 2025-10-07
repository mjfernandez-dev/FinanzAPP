using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FinanzAPP.Views;

namespace FinanzAPP
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(DashboardPage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                var tag = args.SelectedItemContainer.Tag.ToString();

                switch (tag)
                {
                    case "Dashboard":
                        ContentFrame.Navigate(typeof(DashboardPage));
                        break;
                    case "Transacciones":
                        ContentFrame.Navigate(typeof(TransaccionesPage));
                        break;
                }
            }
        }
    }
}