using Microsoft.UI.Xaml;
using FinanzAPP.Data;

namespace FinanzAPP
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            // Crear la base de datos al iniciar
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted(); // Eliminar si existe
                db.Database.EnsureCreated();  // Crear nueva
            }
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window = null!;
    }
}