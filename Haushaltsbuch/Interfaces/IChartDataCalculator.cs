using Haushaltsbuch.Objects;

namespace Haushaltsbuch.Interfaces
{
    /// <summary>
    /// Schnittstelle, die Funktionen zum Berechnen von Diagrammdaten anbietet.
    /// </summary>
    public interface IChartDataCalculator
    {
        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Kategorie in Währungseinheiten.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Daten für Diagramm in Währungseinheiten.</returns>
        ChartData[] CalculateChartDataByCategoryInCurrencyUnits(
            Transaction[] selectedTransactions,
            Helper.TransactionType transactionType);

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Kategorie in Prozent.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Daten für Diagramm in Prozent.</returns>
        ChartData[] CalculateChartDataByCategoryInPercent(
            Transaction[] selectedTransactions,
            Helper.TransactionType transactionType);

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Art in Währungseinheiten.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Daten für Diagramm in Währungseinheiten.</returns>
        ChartData[] CalculateChartDataByTypeInCurrencyUnits(Transaction[] selectedTransactions);

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Art in Prozent.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Daten für Diagramm in Prozent.</returns>
        ChartData[] CalculateChartDataByTypeInPercent(Transaction[] selectedTransactions);

        /// <summary>
        /// Berechnet Summe von Einträgen.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Berechnete Summe.</returns>
        decimal CalculateTotal(Transaction[] selectedTransactions, Helper.TransactionType transactionType);
    }
}