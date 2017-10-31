using System;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Klasse zum Berechnen von Diagrammdaten testet.
    /// </summary>
    [TestClass]
    public class ChartDataCalculatorTest
    {
        #region Felder

        /// <summary>
        /// Klasse zum Berechnen von Diagrammdaten.
        /// </summary>
        private IChartDataCalculator chartDataCalculator;

        /// <summary>
        /// Ausgaben und Einnahmen zum Berechnen der Diagrammdaten.
        /// </summary>
        private Transaction[] testTransactions;

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert Klasse zum Berechnen von Diagrammdaten.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            chartDataCalculator = new ChartDataCalculator();
            DateTime dateTime = DateTime.Today;

            testTransactions = new[]
            {
                new Transaction(dateTime)
                {
                    Amount = "-22,00",
                    Category = "OutgoingCategory2",
                    TransactionType = Helper.TransactionType.Outgoing
                },
                new Transaction(dateTime)
                {
                    Amount = "-00,85",
                    Category = "OutgoingCategory2",
                    TransactionType = Helper.TransactionType.Outgoing
                },
                new Transaction(dateTime)
                {
                    Amount = "-50,51",
                    Category = "OutgoingCategory1",
                    TransactionType = Helper.TransactionType.Outgoing
                },
                new Transaction(dateTime)
                {
                    Amount = "50,00",
                    Category = "IncomingCategory2",
                    TransactionType = Helper.TransactionType.Incoming
                },
                new Transaction(dateTime)
                {
                    Amount = "15,16",
                    Category = "IncomingCategory1",
                    TransactionType = Helper.TransactionType.Incoming
                }
            };
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Ausgaben in Prozent.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromOutgoingTransactionsInPercent()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "OutgoingCategory1",
                    Quantity = 69
                },
                new ChartData
                {
                    Group = "OutgoingCategory2",
                    Quantity = 31
                }
            };

            // Act
            ChartData[] calculatedChartData = chartDataCalculator.CalculateChartDataByCategoryInPercent(
                testTransactions, Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Ausgaben in Währungseinheiten.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromOutgoingTransactionsInCurrencyUnits()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "OutgoingCategory1",
                    Quantity = 50.51m
                },
                new ChartData
                {
                    Group = "OutgoingCategory2",
                    Quantity = 22.85m
                }
            };

            // Act
            ChartData[] calculatedChartData =
                chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(
                    testTransactions, Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Einnahmen in Prozent.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromIncomingTransactionsInPercent()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "IncomingCategory1",
                    Quantity = 23
                },
                new ChartData
                {
                    Group = "IncomingCategory2",
                    Quantity = 77
                }
            };

            // Act
            ChartData[] calculatedChartData = chartDataCalculator.CalculateChartDataByCategoryInPercent(
                testTransactions, Helper.TransactionType.Incoming);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Einnahmen in Währungseinheiten.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromIncomingTransactionsInCurrencyUnits()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "IncomingCategory1",
                    Quantity = 15.16m
                },
                new ChartData
                {
                    Group = "IncomingCategory2",
                    Quantity = 50.00m
                }
            };

            // Act
            ChartData[] calculatedChartData =
                chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(
                    testTransactions, Helper.TransactionType.Incoming);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Ausgaben ohne Einträge.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromOutgoingTransactionsWithEmptyTransactions()
        {
            // Arrange
            testTransactions = new Transaction[0];
            ChartData[] expectedChartData = new ChartData[0];

            // Act
            ChartData[] calculatedChartDataInPercent =
                chartDataCalculator.CalculateChartDataByCategoryInPercent(testTransactions,
                    Helper.TransactionType.Outgoing);

            ChartData[] calculatedChartDataInCurrencyUnits =
                chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(
                    testTransactions, Helper.TransactionType.Outgoing);

            // Assert
            CollectionAssert.AreEqual(expectedChartData, calculatedChartDataInPercent);
            CollectionAssert.AreEqual(expectedChartData, calculatedChartDataInCurrencyUnits);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Ausgaben ohne Kategorie in Prozent.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromOutgoingTransactionsWithEmptyCategoryInPercent()
        {
            // Arrange
            DateTime dateTime = DateTime.Today;

            testTransactions = new[]
            {
                new Transaction(dateTime)
                {
                    Amount = "-10",
                    TransactionType = Helper.TransactionType.Outgoing
                }
            };

            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "Unbekannt",
                    Quantity = 100
                }
            };

            // Act
            ChartData[] calculatedChartData = chartDataCalculator.CalculateChartDataByCategoryInPercent(
                testTransactions, Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Kategorie von Ausgaben ohne Kategorie in Währungseinheiten.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByCategoryFromOutgoingTransactionsWithEmptyCategoryInCurrencyUnits()
        {
            // Arrange
            DateTime dateTime = DateTime.Today;

            testTransactions = new[]
            {
                new Transaction(dateTime)
                {
                    Amount = "-10",
                    TransactionType = Helper.TransactionType.Outgoing
                }
            };

            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "Unbekannt",
                    Quantity = 10.00m
                }
            };

            // Act
            ChartData[] calculatedChartData =
                chartDataCalculator.CalculateChartDataByCategoryInCurrencyUnits(
                    testTransactions, Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Art in Prozent.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByTypeInPercent()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "Ausgaben",
                    Quantity = 53
                },
                new ChartData
                {
                    Group = "Einnahmen",
                    Quantity = 47
                }
            };

            // Act
            ChartData[] calculatedChartData = chartDataCalculator.CalculateChartDataByTypeInPercent(testTransactions);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Art in Währungseinheiten.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByTypeInCurrencyUnits()
        {
            // Arrange
            ChartData[] expectedChartData =
            {
                new ChartData
                {
                    Group = "Ausgaben",
                    Quantity = 73.36m
                },
                new ChartData
                {
                    Group = "Einnahmen",
                    Quantity = 65.16m
                }
            };

            // Act
            ChartData[] calculatedChartData =
                chartDataCalculator.CalculateChartDataByTypeInCurrencyUnits(testTransactions);

            // Assert
            Assert.AreEqual(expectedChartData[0].Group, calculatedChartData[0].Group);
            Assert.AreEqual(expectedChartData[0].Quantity, calculatedChartData[0].Quantity);

            Assert.AreEqual(expectedChartData[1].Group, calculatedChartData[1].Group);
            Assert.AreEqual(expectedChartData[1].Quantity, calculatedChartData[1].Quantity);
        }

        /// <summary>
        /// Testet Berechnen von Diagrammdaten gruppiert nach Art ohne Einträge.
        /// </summary>
        [TestMethod]
        public void TestCalculateChartDataByTypeWithEmptyTransactions()
        {
            // Arrange
            testTransactions = new Transaction[0];
            ChartData[] expectedChartData = new ChartData[0];

            // Act
            ChartData[] calculatedChartDataInPercent =
                chartDataCalculator.CalculateChartDataByTypeInPercent(testTransactions);
            ChartData[] calculatedChartDataInCurrencyUnits =
                chartDataCalculator.CalculateChartDataByTypeInCurrencyUnits(testTransactions);

            // Assert
            CollectionAssert.AreEqual(expectedChartData, calculatedChartDataInPercent);
            CollectionAssert.AreEqual(expectedChartData, calculatedChartDataInCurrencyUnits);
        }

        /// <summary>
        /// Testet Berechnen von Summe von Ausgaben.
        /// </summary>
        [TestMethod]
        public void TestCalculateTotalOutgoing()
        {
            // Arrange
            const decimal EXPECTEDTOTAL = -73.36m;

            // Act
            decimal calculatedTotal = chartDataCalculator.CalculateTotal(testTransactions,
                Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(EXPECTEDTOTAL, calculatedTotal);
        }

        /// <summary>
        /// Testet Berechnen von Summe von Einnahmen.
        /// </summary>
        [TestMethod]
        public void TestCalculateTotalIncoming()
        {
            // Arrange
            const decimal EXPECTEDTOTAL = 65.16m;

            // Act
            decimal calculatedTotal = chartDataCalculator.CalculateTotal(testTransactions,
                Helper.TransactionType.Incoming);

            // Assert
            Assert.AreEqual(EXPECTEDTOTAL, calculatedTotal);
        }

        /// <summary>
        /// Testet Berechnen von Summe von Ausgaben ohne Einträge.
        /// </summary>
        [TestMethod]
        public void TestCalculateTotalOutgoingWithEmptyTransactions()
        {
            // Arrange
            testTransactions = new Transaction[0];
            const decimal EXPECTEDTOTAL = 0;

            // Act
            decimal calculatedTotal = chartDataCalculator.CalculateTotal(testTransactions,
                Helper.TransactionType.Outgoing);

            // Assert
            Assert.AreEqual(EXPECTEDTOTAL, calculatedTotal);
        }

        #endregion
    }
}