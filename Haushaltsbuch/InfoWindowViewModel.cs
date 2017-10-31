using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;

namespace Haushaltsbuch
{
    /// <summary>
    /// ViewModel von InfoWindow.
    /// </summary>
    internal class InfoWindowViewModel : INotifyPropertyChanged
    {
        #region Felder

        /// <summary>
        /// Befehl zum Öffnen des Icons-Hyperlinks.
        /// </summary>
        private ICommand openIconsHyperlink;

        /// <summary>
        /// Befehl zum Öffnen des Schdav-Hyperlinks.
        /// </summary>
        private ICommand openSchdavHyperlink;

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt den Befehl zum Öffnen des Schdav-Hyperlinks.
        /// </summary>
        public ICommand OpenSchdavHyperlinkCommand
        {
            get
            {
                return openSchdavHyperlink ?? (openSchdavHyperlink = new RelayCommand(p => OpenSchdavHyperlink()));
            }
        }

        /// <summary>
        /// Holt den Befehl zum Öffnen des Icons-Hyperlinks.
        /// </summary>
        public ICommand OpenIconsHyperlinkCommand
        {
            get
            {
                return openIconsHyperlink ?? (openIconsHyperlink = new RelayCommand(p => OpenIconsHyperlink()));
            }
        }

        /// <summary>
        /// Holt oder setzt den Produktnamen aus Assembly-Informationen.
        /// </summary>
        public string Product
        {
            get;
            set;
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="InfoWindowViewModel" /> Klasse.
        /// </summary>
        public InfoWindowViewModel()
        {
            GetProductName();
        }

        /// <summary>
        /// Automatisch genertiertes Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Automatisch generierte Methode.
        /// </summary>
        /// <param name="propertyName">Automatisch generierter Paramter.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Öffnet Schdav-Hyperlink.
        /// </summary>
        private static void OpenSchdavHyperlink()
        {
            Hyperlink schdavHyperlink = new Hyperlink
            {
                NavigateUri = new Uri("mailto:info@schdav-dev.com")
            };
            Process.Start(schdavHyperlink.NavigateUri.ToString());
        }

        /// <summary>
        /// Öffnet Icons-Hyperlink.
        /// </summary>
        private static void OpenIconsHyperlink()
        {
            Hyperlink iconsHyperlink = new Hyperlink
            {
                NavigateUri = new Uri("http://p.yusukekamiyamane.com/")
            };
            Process.Start(iconsHyperlink.NavigateUri.ToString());
        }

        /// <summary>
        /// Ermittelt Produktname aus Assembly-Informationen.
        /// </summary>
        private void GetProductName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            if (assembly.Location == null)
            {
                return;
            }

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Product = fileVersionInfo.ProductName;
        }

        #endregion
    }
}