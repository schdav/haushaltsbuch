using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Haushaltsbuch
{
    /// <summary>
    /// ViewModel von SearchWindow.
    /// </summary>
    internal class SearchWindowViewModel : INotifyPropertyChanged
    {
        #region Felder

        /// <summary>
        /// Instanz von MainWindowViewModel.
        /// </summary>
        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Befehl zum Suchen des Suchbegriffs.
        /// </summary>
        private ICommand searchCommand;

        /// <summary>
        /// Suchbegriff, nach dem gesucht werden soll.
        /// </summary>
        private string searchTerm;

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt oder setzt die Methode zum Schließen des Fensters.
        /// </summary>
        public Action CloseWindow
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt den Suchbegriff, nach dem gesucht werden soll.
        /// </summary>
        public string SearchTerm
        {
            get
            {
                return searchTerm;
            }

            set
            {
                searchTerm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt den Befehl zum Suchen des Suchbegriffs.
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                return searchCommand ?? (searchCommand = new RelayCommand(p => StartSearch(), p => CanSearch()));
            }
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="SearchWindowViewModel" /> Klasse.
        /// </summary>
        /// <param name="mainWindowViewModel">Instanz von MainWindowViewModel.</param>
        public SearchWindowViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        /// <summary>
        /// Automatisch generiertes Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Automatisch generierte Methode.
        /// </summary>
        /// <param name="propertyName">Automatisch generierter Parameter.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gibt zurück, ob gesucht werden kann.
        /// </summary>
        /// <returns>
        /// <c>true</c> Es kann gesucht werden.
        /// <c>false</c> Es kann nicht gesucht werden.
        /// </returns>
        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(searchTerm);
        }

        /// <summary>
        /// Startet die Suche.
        /// </summary>
        private void StartSearch()
        {
            CommitSearchTerm();
            CloseWindow();
        }

        /// <summary>
        /// Übergibt den Suchbegriff an das Hauptfenster.
        /// </summary>
        private void CommitSearchTerm()
        {
            mainWindowViewModel.SearchTerm = searchTerm;
        }

        #endregion
    }
}