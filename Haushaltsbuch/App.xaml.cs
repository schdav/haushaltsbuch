using System.Globalization;
using System.Threading;

namespace Haushaltsbuch
{
    /// <summary>
    /// Interaktionslogik für App.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="App" /> Klasse.
        /// </summary>
        public App()
        {
#if DEBUG
            SetCultureInfo();
#endif
            MainWindow mainWindow = new MainWindow();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(mainWindow);

            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }

        /// <summary>
        /// Setzt CultureInfo.
        /// </summary>
        private static void SetCultureInfo()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }
}