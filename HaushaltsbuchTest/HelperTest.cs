using System;
using System.IO;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Hilfsklasse testet.
    /// </summary>
    [TestClass]
    public class HelperTest
    {
        #region Methoden

        /// <summary>
        /// Testet Ermitteln von Dateinamen der XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetXmlFileName()
        {
            // Arrance
            string expected = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Haushaltsbuch\\data.xml");

            // Act
            string actual = Helper.XmlFileName;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}