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
        public void GetTemperatur_CheckYear_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(2013), temp.Year);
        }

        // ##################################################################
        [TestMethod]
        public void GetTemperatur_CheckMonth_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(10), temp.Month);
        }

        // ##################################################################
        [TestMethod]
        public void GetTemperatur_CheckDay_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(25), temp.Day);
        }

        // ##################################################################
        [TestMethod]
        public void GetTemperatur_CheckMax_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(30), temp.Max);
        }


        // ##########################################################################################################################################
        // Database Connection
        // ##########################################################################################################################################
        [TestMethod]
        public void GetTemperatur_CheckDbConnectionUsername_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(".\\SqlExpress", temp.Username);
        }

        // ##################################################################
        [TestMethod]
        public void GetTemperatur_CheckDbConnectionDatabase_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual("SWE_Temperatur", temp.Database);
        }

        // ##################################################################
        [TestMethod]
        public void GetTemperatur_CheckDbConnectionSecurity_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual("true", temp.Security);
        }
    }
}
