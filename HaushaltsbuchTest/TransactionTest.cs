using System;
using System.Globalization;
using System.Threading;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Eintrag testet.
    /// </summary>
    [TestClass]
    public class TransactionTest
    {
        #region Felder

        /// <summary>
        /// Eintrag zum Testen.
        /// </summary>
        private Transaction transaction;

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert Eintrag zum Testen.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            DateTime dateTime = new DateTime(2013, 11, 23);
            transaction = new Transaction(dateTime);
        }

        /// <summary>
        /// Testet Ermitteln von formatiertem Betrag des Eintrags.
        /// </summary>
        [TestMethod]
        public void TestGetAmountString()
        {
            // Arrange
            transaction.Amount = "-100";
            const string EXPECTED = "-100.00";

            // Act
            string actual = transaction.AmountString;

            // Assert
            Assert.AreEqual(EXPECTED, actual);
        }

        /// <summary>
        /// Testet Ermitteln von formatiertem Datum des Eintrags.
        /// </summary>
        [TestMethod]
        public void TestGetDateString()
        {
            // Arrange
            const string EXPECTED = "11/23/2013";

            // Act
            string actual = transaction.DateString;

            // Assert
            Assert.AreEqual(EXPECTED, actual);
        }

        #endregion
    }
}