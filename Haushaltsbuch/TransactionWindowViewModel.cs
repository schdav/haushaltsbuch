using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Haushaltsbuch.Properties;

namespace Haushaltsbuch
{
    /// <summary>
    /// ViewModel von TransactionWindow.
    /// </summary>
    internal sealed class TransactionWindowViewModel : INotifyPropertyChanged
    {
        #region Felder

        /// <summary>
        /// Instanz von MainWindowViewModel.
        /// </summary>
        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Änderung des Eintrags.
        /// </summary>
        private readonly bool isEdit;

        /// <summary>
        /// Dateiname der XML-Datei.
        /// </summary>
        private readonly string fileName;

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
        /// Selektierter Eintrag.
        /// </summary>
        private Transaction selectedTransaction;

        /// <summary>
        /// Titel des Fensters.
        /// </summary>
        private string windowTitle;

        /// <summary>
        /// Inhalte des Speichern-Buttons.
        /// </summary>
        private string saveButtonContent;

        /// <summary>
        /// Eintragsart für Ausgaben.
        /// </summary>
        private bool isOutgoingTransaction;

        /// <summary>
        /// Eintragsart für Einnahmen.
        /// </summary>
        private bool isIncomingTransaction;

        /// <summary>
        /// Beschreibungen der Einträge.
        /// </summary>
        private string[] descriptions;

        /// <summary>
        /// Kategorien der Einträge.
        /// </summary>
        private string[] categories;

        /// <summary>
        /// Datum des Eintrags.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Beschreibung des Eintrags.
        /// </summary>
        private string description;

        /// <summary>
        /// Betrag des Eintrags.
        /// </summary>
        private string amount;

        /// <summary>
        /// Kategorie des Eintrags.
        /// </summary>
        private string category;

        /// <summary>
        /// Sichtbarkeit der Fehlermeldung.
        /// </summary>
        private Visibility errorMessage;

        /// <summary>
        /// Sichtbarkeit der Erfolgsmeldung.
        /// </summary>
        private Visibility successMessage;

        /// <summary>
        /// Befehl zum Speichern des Eintrags.
        /// </summary>
        private ICommand saveCommand;

        /// <summary>
        /// Befehl, wenn Speichern-Button Fokus verliert.
        /// </summary>
        private ICommand saveButtonLostFocus;

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt oder setzt einen Wert, der angibt, ob die Eintrag Ausgabe ist.
        /// </summary>
        public bool IsOutgoingTransaction
        {
            get
            {
                return isOutgoingTransaction;
            }

            set
            {
                isOutgoingTransaction = value;
                OnPropertyChanged();

                if (isOutgoingTransaction)
                {
                    RefreshInformationForOutgoingTransactions();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt einen Wert, der angibt, ob die Eintrag Einnahme ist.
        /// </summary>
        public bool IsIncomingTransaction
        {
            get
            {
                return isIncomingTransaction;
            }

            set
            {
                isIncomingTransaction = value;
                OnPropertyChanged();

                if (isIncomingTransaction)
                {
                    RefreshInformationForIncomingTransactions();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt Beschreibungen der Einträge.
        /// </summary>
        public string[] Descriptions
        {
            get
            {
                return descriptions;
            }

            set
            {
                descriptions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt Kategorien der Einträge.
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
        /// Holt oder setzt das Datum des Eintrags.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Beschreibung des Eintrags.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt den Betrag des Eintrags.
        /// </summary>
        public string Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Kategorie des Eintrags.
        /// </summary>
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt den Titel des Fensters.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return windowTitle;
            }

            set
            {
                windowTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt den Inhalt des Speichern-Buttons.
        /// </summary>
        public string SaveButtonContent
        {
            get
            {
                return saveButtonContent;
            }

            set
            {
                saveButtonContent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Sichtbarkeit der Fehlermeldung.
        /// </summary>
        public Visibility ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Sichtbarkeit der Erfolgsmeldung.
        /// </summary>
        public Visibility SuccessMessage
        {
            get
            {
                return successMessage;
            }

            set
            {
                successMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt den Befehl zum Speichern des Eintrags.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(p => SaveEntry(), p => CanSave()));
            }
        }

        /// <summary>
        /// Holt den Befehl zum Ausblenden der Erfolgsmeldung.
        /// </summary>
        public ICommand SaveButtonLostFocus
        {
            get
            {
                return saveButtonLostFocus ?? (saveButtonLostFocus = new RelayCommand(p => HideSuccessMessage()));
            }
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="TransactionWindowViewModel" /> Klasse.
        /// </summary>
        /// <param name="mainWindowViewModel">Instanz von MainWindowViewModel.</param>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="isEdit">
        /// <c>true</c> Eintrag wird bearbeitet.
        /// <c>false</c> Eintrag wird erstellt.
        /// </param>
        /// <param name="selectedTransaction">Selektierter Eintrag.</param>
        public TransactionWindowViewModel(
            MainWindowViewModel mainWindowViewModel,
            string fileName,
            bool isEdit,
            Transaction selectedTransaction)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.fileName = fileName;
            this.isEdit = isEdit;

            if (this.isEdit)
            {
                WindowTitle = Resources.TransactionEdit;
                SaveButtonContent = Resources.ButtonEdit;
                this.selectedTransaction = selectedTransaction;
                InitializeData();
            }
            else
            {
                WindowTitle = Resources.TransactionAdd;
                SaveButtonContent = Resources.ButtonAdd;
                IsOutgoingTransaction = true;
                date = DateTime.Now;
            }

            ErrorMessage = Visibility.Hidden;
            HideSuccessMessage();
        }

        /// <summary>
        /// Automatisch generiertes Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Automatisch generierte Methode.
        /// </summary>
        /// <param name="propertyName">Automatisch generierter Parameter.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initialisiert Daten.
        /// </summary>
        private void InitializeData()
        {
            if (selectedTransaction.TransactionType == Helper.TransactionType.Outgoing)
            {
                IsOutgoingTransaction = true;
            }
            else
            {
                IsIncomingTransaction = true;
            }

            Date = selectedTransaction.Date;
            Description = selectedTransaction.Description;
            Amount = selectedTransaction.AmountString;
            Category = selectedTransaction.Category;
        }

        /// <summary>
        /// Speichert Eintrag.
        /// </summary>
        private void SaveEntry()
        {
            if (isEdit)
            {
                xmlFileEditor.DeleteTransaction(fileName, selectedTransaction);
            }

            Transaction transaction = CreateTransaction();
            xmlFileEditor.WriteTransaction(fileName, transaction);

            if (isOutgoingTransaction)
            {
                RefreshInformationForOutgoingTransactions();
            }
            else
            {
                RefreshInformationForIncomingTransactions();
            }

            mainWindowViewModel.RefreshData();
            SuccessMessage = Visibility.Visible;

            selectedTransaction = transaction;
        }

        /// <summary>
        /// Erstellt Eintrag aus Benutzereingaben.
        /// </summary>
        /// <returns>Erstellter Eintrag.</returns>
        private Transaction CreateTransaction()
        {
            Transaction transaction = new Transaction(date)
            {
                Amount = dataCalculator.ConvertAmount(amount, isOutgoingTransaction),
                Category = category,
                Description = description,
                TransactionType =
                    isOutgoingTransaction ? Helper.TransactionType.Outgoing : Helper.TransactionType.Incoming
            };

            return transaction;
        }

        /// <summary>
        /// Aktualisiert Beschreibungen und Kategorien von Ausgaben.
        /// </summary>
        private void RefreshInformationForOutgoingTransactions()
        {
            Descriptions = xmlFileReader.GetDescriptionsFromOutgoingTransactions(fileName);
            Categories = xmlFileReader.GetCategoriesFromOutgoingTransactions(fileName);
        }

        /// <summary>
        /// Aktualisiert Beschreibungen und Kategorien von Einnahmen.
        /// </summary>
        private void RefreshInformationForIncomingTransactions()
        {
            Descriptions = xmlFileReader.GetDescriptionsFromIncomingTransactions(fileName);
            Categories = xmlFileReader.GetCategoriesFromIncomingTransactions(fileName);
        }

        /// <summary>
        /// Gibt zurück, ob Eintrag gespeichert werden kann.
        /// </summary>
        /// <returns>
        /// <c>true</c> Eintrag kann gespeichert werden.
        /// <c>false</c> Eintrag kann nicht gespeichert werden.
        /// </returns>
        private bool CanSave()
        {
            if (dataCalculator.ValidateAmount(amount))
            {
                ErrorMessage = Visibility.Hidden;
                return true;
            }

            ErrorMessage = Visibility.Visible;
            return false;
        }

        /// <summary>
        /// Blendet Erfolgsmeldung aus.
        /// </summary>
        private void HideSuccessMessage()
        {
            SuccessMessage = Visibility.Hidden;
        }

        #endregion
    }
}