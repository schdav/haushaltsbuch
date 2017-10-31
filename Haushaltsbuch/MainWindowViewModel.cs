using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Haushaltsbuch.Properties;

namespace Haushaltsbuch
{
    /// <summary>
    /// ViewModel von MainWindow.
    /// </summary>
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Felder

        /// <summary>
        /// Dateiname der XML-Datei.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// Instanz des MainWindow-Fensters.
        /// </summary>
        private readonly MainWindow mainWindow;

        /// <summary>
        /// Schnittstelle, die Funktionen zum Berechnen von Daten anbietet.
        /// </summary>
        private readonly IDataCalculator dataCalculator = new DataCalculator();

        /// <summary>
        /// Schnittstelle, die Funktionen zum Bearbeiten von XML-Datei anbietet.
        /// </summary>
        private readonly IXmlFileEditor xmlFileEditor = new XmlFileEditor();

        /// <summary>
        /// Schnittstelle, die Funktionen zum Lesen aus XML-Datei anbietet.
        /// </summary>
        private readonly IXmlFileReader xmlFileReader = new XmlFileReader();

        /// <summary>
        /// Befehl zum Erstellen eines neuen Eintrags.
        /// </summary>
        private ICommand addTransactionCommand;

        /// <summary>
        /// Befehl zum Löschen eines Eintrags.
        /// </summary>
        private ICommand deleteTransactionCommand;

        /// <summary>
        /// Befehl zum Bearbeiten eines Eintrags.
        /// </summary>
        private ICommand editTransactionCommand;

        /// <summary>
        /// Befehl zum Anzeigen der Auswertungen.
        /// </summary>
        private ICommand showAnalysesCommand;

        /// <summary>
        /// Befehl zum Suchen.
        /// </summary>
        private ICommand searchCommand;

        /// <summary>
        /// Befehl zum Anzeigen des Info-Dialogs.
        /// </summary>
        private ICommand showInfoCommand;

        /// <summary>
        /// Befehl zum Löschen der Suche.
        /// </summary>
        private ICommand deleteSearchCommand;

        /// <summary>
        /// Alle Monate.
        /// </summary>
        private string[] months;

        /// <summary>
        /// Ausgewählter Monat.
        /// </summary>
        private string selectedMonth;

        /// <summary>
        /// Alle Jahre.
        /// </summary>
        private string[] years;

        /// <summary>
        /// Ausgewähltes Jahr.
        /// </summary>
        private string selectedYear;

        /// <summary>
        /// Alle Kategorien.
        /// </summary>
        private string[] categories;

        /// <summary>
        /// Ausgewählte Kategorie.
        /// </summary>
        private string selectedCategory;

        /// <summary>
        /// Alle Einträge.
        /// </summary>
        private Transaction[] transactions;

        /// <summary>
        /// Ausgewählter Eintrag.
        /// </summary>
        private Transaction selectedTransaction;

        /// <summary>
        /// Saldo aller sichtbaren Einträge.
        /// </summary>
        private string balance;

        /// <summary>
        /// Sichtbarkeit der Filter-Grafik.
        /// </summary>
        private Visibility filterImageVisibility;

        /// <summary>
        /// Sichtbarkeit der Suche-Grafik.
        /// </summary>
        private Visibility searchImageVisibility;

        /// <summary>
        /// Sichtbarkeit des Suche-Buttons.
        /// </summary>
        private Visibility searchButtonVisibility;

        /// <summary>
        /// Begriff, nach dem gesucht werden soll.
        /// </summary>
        private string searchTerm;

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt Befehl zum Erstellen eines neuen Eintrags.
        /// </summary>
        public ICommand AddTransactionCommand
        {
            get
            {
                return addTransactionCommand ?? (addTransactionCommand = new RelayCommand(p => AddTransaction()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Löschen eines neuen Eintrags.
        /// </summary>
        public ICommand DeleteTransactionCommand
        {
            get
            {
                return deleteTransactionCommand ??
                       (deleteTransactionCommand = new RelayCommand(p => DeleteTransaction(), p => CanRunCommand()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Bearbeiten eines neuen Eintrags.
        /// </summary>
        public ICommand EditTransactionCommand
        {
            get
            {
                return editTransactionCommand ??
                       (editTransactionCommand = new RelayCommand(p => EditTransaction(), p => CanRunCommand()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Anzeigen der Auswertungen.
        /// </summary>
        public ICommand ShowAnalysesCommand
        {
            get
            {
                return showAnalysesCommand ?? (showAnalysesCommand = new RelayCommand(p => ShowAnalyses()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Suchen.
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                return searchCommand ?? (searchCommand = new RelayCommand(p => Search(), p => CanSearch()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Anzeigen des Info-Dialogs.
        /// </summary>
        public ICommand ShowInfoCommand
        {
            get
            {
                return showInfoCommand ?? (showInfoCommand = new RelayCommand(p => ShowInfo()));
            }
        }

        /// <summary>
        /// Holt Befehl zum Löschen der Suche.
        /// </summary>
        public ICommand DeleteSearchCommand
        {
            get
            {
                return deleteSearchCommand ?? (deleteSearchCommand = new RelayCommand(p => DeleteSearch()));
            }
        }

        /// <summary>
        /// Holt oder setzt alle Monate.
        /// </summary>
        public string[] Months
        {
            get
            {
                return months;
            }

            set
            {
                months = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt den ausgewählten Monat.
        /// </summary>
        public string SelectedMonth
        {
            get
            {
                return selectedMonth;
            }

            set
            {
                selectedMonth = value;
                OnPropertyChanged();

                // Vermeidung mehrfacher Aufrufe
                if (selectedYear != null && selectedCategory != null && searchTerm != null)
                {
                    RefreshTransactions();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt alle Jahre.
        /// </summary>
        public string[] Years
        {
            get
            {
                return years;
            }

            set
            {
                years = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt ausgewähltes Jahr.
        /// </summary>
        public string SelectedYear
        {
            get
            {
                return selectedYear;
            }

            set
            {
                selectedYear = value;
                OnPropertyChanged();

                // Vermeidung mehrfacher Aufrufe
                if (selectedMonth != null && selectedCategory != null && searchTerm != null)
                {
                    RefreshTransactions();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt alle Kategorien.
        /// </summary>
        public string[] Categories
        {
            get
            {
                return categories;
            }

            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt ausgewählte Kategorie.
        /// </summary>
        public string SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                selectedCategory = value;
                OnPropertyChanged();

                SetFilterImageVisibility();

                // Vermeidung mehrfacher Aufrufe
                if (selectedMonth != null && selectedYear != null && searchTerm != null)
                {
                    RefreshTransactions();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt alle Einträge.
        /// </summary>
        public Transaction[] Transactions
        {
            get
            {
                return transactions;
            }

            set
            {
                transactions = value;
                OnPropertyChanged();

                RefreshBalance();
            }
        }

        /// <summary>
        /// Holt oder setzt ausgewählten Eintrag.
        /// </summary>
        public Transaction SelectedTransaction
        {
            get
            {
                return selectedTransaction;
            }

            set
            {
                selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Saldo aller sichtbaren Einträge.
        /// </summary>
        public string Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Sichtbarkeit der Filter-Grafik.
        /// </summary>
        public Visibility FilterImageVisibility
        {
            get
            {
                return filterImageVisibility;
            }

            set
            {
                filterImageVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Sichtbarkeit der Suche-Grafik.
        /// </summary>
        public Visibility SearchImageVisibility
        {
            get
            {
                return searchImageVisibility;
            }

            set
            {
                searchImageVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Sichtbarkeit des Suche-Buttons.
        /// </summary>
        public Visibility SearchButtonVisibility
        {
            get
            {
                return searchButtonVisibility;
            }

            set
            {
                searchButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Begriff, nach dem gesucht werden soll.
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

                // Vermeidung mehrfacher Aufrufe
                if (selectedMonth != null && selectedYear != null && selectedCategory != null)
                {
                    RefreshTransactions();
                }

                SetSearchImageVisibility();
                SetSearchButtonVisibility();
            }
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="MainWindowViewModel" /> Klasse.
        /// </summary>
        /// <param name="mainWindow">Instanz der Klasse MainWindow.</param>
        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            fileName = Helper.XmlFileName;

            if (!xmlFileEditor.CheckXmlDocument(fileName))
            {
                xmlFileEditor.CreateXmlDocument(fileName);
            }

            InitializeControls();

            SearchTerm = string.Empty;
        }

        /// <summary>
        /// Automatisch generiertes Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Aktualisiert Daten.
        /// </summary>
        public void RefreshData()
        {
            RefreshTransactions();
            RefreshFilter();
        }

        /// <summary>
        /// Automatisch generierte Methode.
        /// </summary>
        /// <param name="propertyName">Automatisch generierter Parameter.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initialisiert Steuerelemente.
        /// </summary>
        private void InitializeControls()
        {
            Months = dataCalculator.GetMonths();
            SelectedMonth = Months[DateTime.Today.Month];

            Years = dataCalculator.GetYears();
            SelectedYear = Years[Years.Length - 1];

            RefreshFilter();
        }

        /// <summary>
        /// Erstellt Filter.
        /// </summary>
        /// <returns>Erstellter Filter.</returns>
        private Filter CreateFilter()
        {
            return dataCalculator.CreateFilter(
                Array.IndexOf(months, selectedMonth),
                selectedYear,
                selectedCategory,
                searchTerm);
        }

        /// <summary>
        /// Öffnet Fenster zum Erstellen eines neuen Eintrags.
        /// </summary>
        private void AddTransaction()
        {
            TransactionWindowViewModel transactionWindowViewModel = new TransactionWindowViewModel(
                this, fileName, false, null);

            TransactionWindow transactionWindow = new TransactionWindow
            {
                DataContext = transactionWindowViewModel,
                Owner = mainWindow
            };

            transactionWindow.ShowDialog();
        }

        /// <summary>
        /// Öffnet Fenster zum Löschen eines neuen Eintrags.
        /// </summary>
        private void DeleteTransaction()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                Resources.DeleteTransactionText,
                Resources.TransactionDelete,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (messageBoxResult != MessageBoxResult.Yes)
            {
                return;
            }

            xmlFileEditor.DeleteTransaction(fileName, selectedTransaction);

            RefreshData();
        }

        /// <summary>
        /// Öffnet Fenster zum Bearbeiten eines neuen Eintrags.
        /// </summary>
        private void EditTransaction()
        {
            TransactionWindowViewModel transactionWindowViewModel = new TransactionWindowViewModel(
                this, fileName, true, selectedTransaction);

            TransactionWindow transactionWindow = new TransactionWindow
            {
                DataContext = transactionWindowViewModel,
                Owner = mainWindow
            };

            transactionWindow.ShowDialog();
        }

        /// <summary>
        /// Öffnet Fenster zum Anzeigen der Auswertungen.
        /// </summary>
        private void ShowAnalyses()
        {
            AnalysesWindowViewModel analysesWindowsViewModel = new AnalysesWindowViewModel(
                selectedMonth, selectedYear, fileName);

            AnalysesWindow analysesWindow = new AnalysesWindow
            {
                DataContext = analysesWindowsViewModel,
                Owner = mainWindow
            };

            analysesWindow.ShowDialog();
        }

        /// <summary>
        /// Öffnet Fenster zum Suchen.
        /// </summary>
        private void Search()
        {
            SearchWindowViewModel searchWindowViewModel = new SearchWindowViewModel(this);

            SearchWindow searchWindow = new SearchWindow
            {
                DataContext = searchWindowViewModel,
                Owner = mainWindow
            };

            searchWindow.SearchTermTextBox.Focus();
            searchWindowViewModel.CloseWindow = searchWindow.Close;
            searchWindow.ShowDialog();
        }

        /// <summary>
        /// Öffnet Fenster zum Anzeigen des Info-Dialogs.
        /// </summary>
        private void ShowInfo()
        {
            InfoWindowViewModel infoWindowViewModel = new InfoWindowViewModel();

            InfoWindow infoWindow = new InfoWindow
            {
                DataContext = infoWindowViewModel,
                Owner = mainWindow
            };

            infoWindow.ShowDialog();
        }

        /// <summary>
        /// Löscht Suche.
        /// </summary>
        private void DeleteSearch()
        {
            SearchTerm = string.Empty;
        }

        /// <summary>
        /// Gibt zurück, ob Kommando ausgeführt werden kann.
        /// </summary>
        /// <returns>
        /// <c>true</c> Kommando kann ausgeführt werden.
        /// <c>false</c> Kommando kann nicht ausgeführt werden.
        /// </returns>
        private bool CanRunCommand()
        {
            return selectedTransaction != null;
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
            return transactions.Length != 0;
        }

        /// <summary>
        /// Aktualisiert Einträge.
        /// </summary>
        private void RefreshTransactions()
        {
            Transactions = xmlFileReader.GetTransactions(fileName, CreateFilter());
        }

        /// <summary>
        /// Aktualisiert Filter.
        /// </summary>
        private void RefreshFilter()
        {
            Categories = xmlFileReader.GetCategoriesFromAllTransactions(fileName);
            SelectedCategory = selectedCategory ?? string.Empty;
        }

        /// <summary>
        /// Aktualisiert Saldo.
        /// </summary>
        private void RefreshBalance()
        {
            Balance = dataCalculator.CalculateBalance(transactions).ToString("F", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Bestimmt Sichtbarkeit der Filter-Grafik.
        /// </summary>
        private void SetFilterImageVisibility()
        {
            FilterImageVisibility = string.IsNullOrEmpty(selectedCategory) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Bestimmt Sichtbarkeit der Suche-Grafik.
        /// </summary>
        private void SetSearchImageVisibility()
        {
            SearchImageVisibility = string.IsNullOrEmpty(searchTerm) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Bestimmt Sichtbarkeit des Suche-Buttons.
        /// </summary>
        private void SetSearchButtonVisibility()
        {
            SearchButtonVisibility = string.IsNullOrEmpty(searchTerm) ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion
    }
}