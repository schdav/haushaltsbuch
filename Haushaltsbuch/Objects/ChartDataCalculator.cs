using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Properties;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die Schnittstelle zum Berechnen von Diagrammdaten implementiert.
    /// </summary>
    public class ChartDataCalculator : IChartDataCalculator
    {
        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Kategorie in Währungseinheiten.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Daten für Diagramm in Währungseinheiten.</returns>
        public ChartData[] CalculateChartDataByCategoryInCurrencyUnits(
            Transaction[] selectedTransactions, Helper.TransactionType transactionType)
        {
            return
                selectedTransactions.Where(t => t.TransactionType == transactionType)
                    .GroupBy(t => t.Category)
                    .Select(gt => new ChartData
                    {
                        Group = string.IsNullOrEmpty(gt.Key) ? Resources.UnknownCategory : gt.Key,
                        Quantity = gt.Sum(t => Math.Abs(Convert.ToDecimal(t.Amount, new CultureInfo("de-DE"))))
                    }).OrderBy(cd => cd.Group).ToArray();
        }

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Kategorie in Prozent.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Daten für Diagramm in Prozent.</returns>
        public ChartData[] CalculateChartDataByCategoryInPercent(
            Transaction[] selectedTransactions, Helper.TransactionType transactionType)
        {
            decimal total = Math.Abs(GetTotal(selectedTransactions, transactionType));

            return
                selectedTransactions.Where(t => t.TransactionType == transactionType)
                    .GroupBy(t => t.Category)
                    .Select(gt => new ChartData
                    {
                        Group = string.IsNullOrEmpty(gt.Key) ? Resources.UnknownCategory : gt.Key,
                        Quantity =
                            Math.Round(
                                gt.Sum(t => Math.Abs(Convert.ToDecimal(t.Amount, new CultureInfo("de-DE")))) / total *
                                100,
                                MidpointRounding.ToEven)
                    }).OrderBy(cd => cd.Group).ToArray();
        }

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Art in Währungseinheiten.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Daten für Diagramm in Währungseinheiten.</returns>
        public ChartData[] CalculateChartDataByTypeInCurrencyUnits(Transaction[] selectedTransactions)
        {
            return selectedTransactions.GroupBy(t => t.TransactionType).Select(gt => new ChartData
            {
                Group = gt.Key == Helper.TransactionType.Outgoing ? Resources.Outgoings : Resources.Incomings,
                Quantity = Math.Abs(GetTotal(selectedTransactions, gt.Key))
            }).OrderBy(cd => cd.Group).ToArray();
        }

        /// <summary>
        /// Berechnet Diagrammdaten gruppiert nach Art in Prozent.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Daten für Diagramm in Prozent.</returns>
        public ChartData[] CalculateChartDataByTypeInPercent(Transaction[] selectedTransactions)
        {
            decimal total = Math.Abs(GetTotal(selectedTransactions, Helper.TransactionType.Outgoing)) +
                            Math.Abs(GetTotal(selectedTransactions, Helper.TransactionType.Incoming));

            return selectedTransactions.GroupBy(t => t.TransactionType).Select(gt => new ChartData
            {
                Group = gt.Key == Helper.TransactionType.Outgoing ? Resources.Outgoings : Resources.Incomings,
                Quantity =
                    Math.Round(Math.Abs(GetTotal(selectedTransactions, gt.Key)) / total * 100, MidpointRounding.ToEven)
            }).OrderBy(cd => cd.Group).ToArray();
        }

        /// <summary>
        /// Berechnet Summe von Einträgen.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Berechnete Summe.</returns>
        public decimal CalculateTotal(
            Transaction[] selectedTransactions, Helper.TransactionType transactionType)
        {
            return GetTotal(selectedTransactions, transactionType);
        }

        /// <summary>
        /// Berechnet Summe von Einträgen.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <param name="transactionType">Art der Einträge.</param>
        /// <returns>Berechnete Summe.</returns>
        private static decimal GetTotal(
            IEnumerable<Transaction> selectedTransactions, Helper.TransactionType transactionType)
        {
            return
                selectedTransactions.Where(t => t.TransactionType == transactionType)
                    .Sum(t => Convert.ToDecimal(t.Amount, new CultureInfo("de-DE")));
        }
    }
}