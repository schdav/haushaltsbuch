using System;
using System.Globalization;
using System.Linq;
using DataCreator.Properties;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;

namespace DataCreator
{
    /// <summary>
    /// Hauptklasse der Anwendung.
    /// </summary>
    internal class Program
    {
        #region Felder

        /// <summary>
        /// Kategorien der Einträge.
        /// </summary>
        private static readonly string[] Categories = new string[Settings.Default.NumberOfCategories];

        /// <summary>
        /// Anzahl der Einträge.
        /// </summary>
        private static readonly int NumberOfTransactions = Settings.Default.NumberOfTransactions;

        /// <summary>
        /// Generator für Zufallszahlen.
        /// </summary>
        private static Random random;

        #endregion

        #region Methoden

        /// <summary>
        /// Startmethode der Anwendung.
        /// </summary>
        private static void Main()
        {
            random = new Random();

            try
            {
                CreateCategories();
                WriteTransactions();
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        /// <summary>
        /// Erstellt Kategorien.
        /// </summary>
        private static void CreateCategories()
        {
            for (int i = 0; i < Categories.Length; i++)
            {
                Categories[i] = GetRandomWord();
            }
        }

        /// <summary>
        /// Gibt zufälliges Wort zurück.
        /// </summary>
        /// <returns>Zufälliges Wort.</returns>
        private static string GetRandomWord()
        {
            int minWordLength = Settings.Default.MinWordLength;
            int maxWordLength = Settings.Default.MaxWordLength;

            int randomWordSize = random.Next(minWordLength, maxWordLength + 1);
            string allowedCharacters = Settings.Default.AllowedCharacters;

            return
                new string(
                    Enumerable.Repeat(allowedCharacters, randomWordSize)
                        .Select(s => s[random.Next(s.Length)])
                        .ToArray());
        }

        /// <summary>
        /// Schreibt Einträge in XML-Datei.
        /// </summary>
        private static void WriteTransactions()
        {
            string fileName = Helper.XmlFileName;

            IXmlFileEditor xmlFileEditor = new XmlFileEditor();
            xmlFileEditor.CreateXmlDocument(fileName);

            for (int i = 0; i < NumberOfTransactions; i++)
            {
                xmlFileEditor.WriteTransaction(fileName, GetTransaction());
                WriteStatus(i);
            }
        }

        /// <summary>
        /// Schreibt Status in die Konsole.
        /// </summary>
        /// <param name="currentTransaction">Nummer des aktuellen Eintrags.</param>
        private static void WriteStatus(int currentTransaction)
        {
            string progressInPercent =
                Math.Round((decimal)currentTransaction / NumberOfTransactions * 100)
                    .ToString(CultureInfo.InvariantCulture);

            Console.WriteLine(@"Writing Transaction " + currentTransaction + @" of " + NumberOfTransactions + @" (" +
                              progressInPercent + @" %).");
        }

        /// <summary>
        /// Gibt erstellten Eintrag zurück.
        /// </summary>
        /// <returns>Erstellter Eintrag.</returns>
        private static Transaction GetTransaction()
        {
            string amount = GetRandomAmount();
            Helper.TransactionType transactionType = amount.StartsWith("-", StringComparison.Ordinal)
                ? Helper.TransactionType.Outgoing
                : Helper.TransactionType.Incoming;

            return new Transaction(GetRandomDate())
            {
                Amount = amount,
                Category = GetRandomCategory(),
                Description = GetRandomWord(),
                TransactionType = transactionType
            };
        }

        /// <summary>
        /// Gibt zufälliges Datum zurück.
        /// </summary>
        /// <returns>Zufälliges Datum.</returns>
        private static DateTime GetRandomDate()
        {
            DateTime earliestDate = new DateTime(DateTime.Today.Year - 2, 1, 1);
            int range = (DateTime.Today - earliestDate).Days;

            return earliestDate.AddDays(random.Next(range + 1));
        }

        /// <summary>
        /// Gibt zufälligen Betrag zurück.
        /// </summary>
        /// <returns>Zufälliger Betrag.</returns>
        private static string GetRandomAmount()
        {
            double minAmountValue = Settings.Default.MinAmountValue;
            double maxAmountValue = Settings.Default.MaxAmountValue;

            double randomDoubleNumber = random.NextDouble();

            return (minAmountValue + randomDoubleNumber * (maxAmountValue - minAmountValue)).ToString(
                "F2", CultureInfo.GetCultureInfo("de-DE"));
        }

        /// <summary>
        /// Gibt zufällige Kategorie zurück.
        /// </summary>
        /// <returns>Zufällige Kategorie.</returns>
        private static string GetRandomCategory()
        {
            int categoryIndex = random.Next(Categories.Length);

            return Categories[categoryIndex];
        }

        /// <summary>
        /// Schreibt Meldung eines aufgetretenen Fehlers in Konsole.
        /// </summary>
        /// <param name="exception">Aufgetretener Fehler.</param>
        private static void WriteError(Exception exception)
        {
            Console.WriteLine(@"Error: " + exception.Message);
            Console.WriteLine(@"Press RETURN to exit.");
            Console.ReadLine();
        }

        #endregion
    }
}