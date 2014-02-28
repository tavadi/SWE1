using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace ServerTest
{
    [TestClass]
    public class TestTemperatur
    {
        // ##########################################################################################################################################
        // Plugin Temperatur
        // ##########################################################################################################################################
        [TestMethod]
        public void Temperatur_CheckYear_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(2013), temp.Year);
        }

        // ##################################################################
        [TestMethod]
        public void Temperatur_CheckMonth_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(10), temp.Month);
        }

        // ##################################################################
        [TestMethod]
        public void Temperatur_CheckDay_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(25), temp.Day);
        }

        // ##################################################################
        [TestMethod]
        public void Temperatur_CheckMax_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(30), temp.Max);
        }


        // ##########################################################################################################################################
        // Database Connection
        // ##########################################################################################################################################
        [TestMethod]
        public void Temperatur_CheckDbConnectionUsername_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(".\\SqlExpress", temp.Username);
        }

        // ##################################################################
        [TestMethod]
        public void Temperatur_CheckDbConnectionDatabase_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual("SWE_Temperatur", temp.Database);
        }

        // ##################################################################
        [TestMethod]
        public void Temperatur_CheckDbConnectionSecurity_True()
        {
            Temperatur temp = new Temperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual("true", temp.Security);
        }
    }
}
