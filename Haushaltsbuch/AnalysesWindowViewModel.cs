using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Haushaltsbuch.Properties;

namespace Haushaltsbuch
{
    /// <summary>
    /// ViewModel von AnalysesWindow.
    /// </summary>
    internal sealed class AnalysesWindowViewModel : INotifyPropertyChanged
    {
        #region Felder

        /// <summary>
        /// Dateiname der XML-Datei.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// Schnittstelle, die Funktionen zum Berechnen von Daten anbietet.
        /// </summary>
        private readonly IDataCalculator dataCalculator = new DataCalculator();

        /// <summary>
        /// Schnittstelle, die Funktionen zum Lesen aus XML-Datei anbietet.
        /// </summary>
        private readonly IXmlFileReader xmlFileReader = new XmlFileReader();

        /// <summary>
        /// Schnittstelle, die Funktionen zum Berechnen von Diagrammdaten anbietet.
        /// </summary>
        private readonly IChartDataCalculator chartDataCalculator = new ChartDataCalculator();

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
        /// Ansicht in Prozent.
        /// </summary>
        private bool viewInPercent;

        /// <summary>
        /// Ansicht in Währungseinheiten.
        /// </summary>
        private bool viewInCurrencyUnits;

        /// <summary>
        /// Überschrift der Ausgaben.
        /// </summary>
        private string outgoingHeading;

        /// <summary>
        /// Überschrift der Einnahmen.
        /// </summary>
        private string incomingHeading;

        /// <summary>
        /// Überschrift des Saldos.
        /// </summary>
        private string balanceHeading;

        /// <summary>
        /// Daten des Ausgaben-Diagramms.
        /// </summary>
        private ChartData[] outgoingChartData;

        /// <summary>
        /// Daten des Einnahmen-Diagramms.
        /// </summary>
        private ChartData[] incomingChartData;

        /// <summary>
        /// Daten des Saldo-Diagramms.
        /// </summary>
        private ChartData[] balanceChartData;

        #endregion

        #region Eigenschaften

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
        /// Holt oder setzt ausgewählten Monat.
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

                if (selectedYear != null && (viewInPercent || viewInCurrencyUnits))
                {
                    CalculateCharts();
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

                if (selectedMonth != null && (viewInPercent || viewInCurrencyUnits))
                {
                    CalculateCharts();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt einen Wert, der angibt, ob Ansicht in Prozent.
        /// </summary>
        public bool ViewInPercent
        {
            get
            {
                return viewInPercent;
            }

            set
            {
                viewInPercent = value;
                OnPropertyChanged();

                // Vermeidung mehrfacher Aufrufe
                if (selectedMonth != null && selectedYear != null &&
                    (viewInPercent && !viewInCurrencyUnits || !viewInPercent && viewInCurrencyUnits))
                {
                    CalculateCharts();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt einen Wert, der angibt, ob Ansicht in Währungseinheiten.
        /// </summary>
        public bool ViewInCurrencyUnits
        {
            get
            {
                return viewInCurrencyUnits;
            }

            set
            {
                viewInCurrencyUnits = value;
                OnPropertyChanged();

                // Vermeidung mehrfacher Aufrufe
                if (selectedMonth != null && selectedYear != null &&
                    (viewInCurrencyUnits && !viewInPercent || !viewInCurrencyUnits && viewInPercent))
                {
                    CalculateCharts();
                }
            }
        }

        /// <summary>
        /// Holt oder setzt die Überschrift der Ausgaben.
        /// </summary>
        public string OutgoingHeading
        {
            get
            {
                return outgoingHeading;
            }

            set
            {
                outgoingHeading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Überschrift der Einnahmen.
        /// </summary>
        public string IncomingHeading
        {
            get
            {
                return incomingHeading;
            }

            set
            {
                incomingHeading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Daten des Ausgaben-Diagramms.
        /// </summary>
        public ChartData[] OutgoingChartData
        {
            get
            {
                return outgoingChartData;
            }

            set
            {
                outgoingChartData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Daten des Einnahmen-Diagramms.
        /// </summary>
        public ChartData[] IncomingChartData
        {
            get
            {
                return incomingChartData;
            }

            set
            {
                incomingChartData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Daten des Saldo-Diagramms.
        /// </summary>
        public ChartData[] BalanceChartData
        {
            get
            {
                return balanceChartData;
            }

            set
            {
                balanceChartData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Holt oder setzt die Überschrift des Saldos.
        /// </summary>
        public string BalanceHeading
        {
            get
            {
                return balanceHeading;
            }

            set
            {
                balanceHeading = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="AnalysesWindowViewModel" /> Klasse.
        /// </summary>
        /// <param name="month">Ausgewählter Monat.</param>
        /// <param name="year">Ausgewähltes Jahr.</param>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        public AnalysesWindowViewModel(string month, string year, string fileName)
        {
            this.fileName = fileName;

            Months = dataCalculator.GetMonths();
            SelectedMonth = month;

            Years = dataCalculator.GetYears();
            SelectedYear = year;

            ViewInPercent = true;
        }

        /// <summary>
        /// Automatisch generiertes Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Automatisch generierte Methode.
        /// </summary>
        /// <param name="propertyName">Automatisch generierter Paramter.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Berechnet Diagramme.
        /// </summary>
        private void CalculateCharts()
        {
            Filter filter = dataCalculator.CreateFilter(
                Array.IndexOf(months, selectedMonth),
                SelectedYear,
                string.Empty,
                string.Empty);
            Transaction[] transactions = xmlFileReader.GetTransactions(fileName, filter);

            CalculateChartDatas(transactions);
            CaluclateChartTexts(transactions);
        }

        /// <summary>
        /// Berechnet Daten für Diagramme.
        /// </summary>
        /// <param name="transactions">Einträge, aus denen Daten für Diagramme berechnet werden.</param>
        private void CalculateChartDatas(Transaction[] transactions)
        {
            OutgoingChartData = CalculateChartData(transactions, Helper.TransactionType.Outgoing);
            IncomingChartData = CalculateChartData(transactions, Helper.TransactionType.Incoming);
            BalanceChartData = CalculateChartData(transactions, Helper.TransactionType.Balance);
        }

        /// <summary>
        /// Berechnet Daten für Diagramm.
        /// </summary>
        /// <param name="transactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Daten für Diagramm.</returns>
        private ChartData[] CalculateChartData(Transaction[] transactions, Helper.TransactionType transactionType)
        {
            switch (transactionType)
            {
                case Helper.TransactionType.Outgoing:
                    return viewInPercent
                        ? chartDataCalculator.CalculateChartDataByCategoryInPercent(transactions, transactionType)
                        : chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(transactions, transactionType);

                case Helper.TransactionType.Incoming:
                    return viewInPercent
                        ? chartDataCalculator.CalculateChartDataByCategoryInPercent(transactions, transactionType)
                        : chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(transactions, transactionType);

                case Helper.TransactionType.Balance:
                    return viewInPercent
                        ? chartDataCalculator.CalculateChartDataByTypeInPercent(transactions)
                        : chartDataCalculator.CalculateChartDataByTypeInCurrencyUnits(transactions);

                default:
                    return new ChartData[]
                    {
                    };
            }
        }

        /// <summary>
        /// Berechnet Text für Diagramme.
        /// </summary>
        /// <param name="transactions">Ausgewählte Einträge.</param>
        private void CaluclateChartTexts(Transaction[] transactions)
        {
            OutgoingHeading = CalculateChartText(outgoingChartData, transactions, Helper.TransactionType.Outgoing);
            IncomingHeading = CalculateChartText(incomingChartData, transactions, Helper.TransactionType.Incoming);
            BalanceHeading = CalculateChartText(balanceChartData, transactions, Helper.TransactionType.Balance);
        }

        /// <summary>
        /// Berechnet Text für Diagramm.
        /// </summary>
        /// <param name="chartData">Daten des Diagramms.</param>
        /// <param name="transactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Text für Diagramm.</returns>
        private string CalculateChartText(
            IEnumerable<ChartData> chartData,
            Transaction[] transactions,
            Helper.TransactionType transactionType)
        {
            if (!chartData.Any())
            {
                return Resources.AnalysesDataError;
            }

            string total =
            (transactionType != Helper.TransactionType.Balance
                ? chartDataCalculator.CalculateTotal(transactions, transactionType)
                : dataCalculator.CalculateBalance(transactions)).ToString(CultureInfo.CurrentCulture);

            string month = string.IsNullOrEmpty(selectedMonth)
                ? Resources.Year
                : Resources.Month + " " + selectedMonth;

            switch (transactionType)
            {
                case Helper.TransactionType.Outgoing:
                    return string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.AnalysesOutgoingInfo,
                        month,
                        selectedYear,
                        total);
                case Helper.TransactionType.Incoming:
                    return string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.AnalysesIncomingInfo,
                        month,
                        selectedYear,
                        total);
                case Helper.TransactionType.Balance:
                    return string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.AnalysesBalanceInfo,
                        month,
                        selectedYear,
                        total);
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}