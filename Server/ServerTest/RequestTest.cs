using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Server;


namespace ServerTest
{
    [TestClass]
    public class RequestTest
    {
        [TestMethod]
        public void GetTemperatur_CheckUrl_True()
        {
            string PluginName = "GetTemperatur.html";
            string HTTPHeader = @"GET /GetTemperatur.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }




        [TestMethod]
        public void GetTemperatur_CheckYear_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = {"year", "2013", "month", "10", "day", "25", "max", "30"};

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(2013), temp.Year);
        }


        [TestMethod]
        public void GetTemperatur_CheckMonth_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(10), temp.Month);
        }


        [TestMethod]
        public void GetTemperatur_CheckDay_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(25), temp.Day);
        }


        [TestMethod]
        public void GetTemperatur_CheckMax_True()
        {
            GetTemperatur temp = new GetTemperatur();

            string[] param = { "year", "2013", "month", "10", "day", "25", "max", "30" };

            temp.Init(param);
            temp.ParseParameters();

            Assert.AreEqual(Convert.ToUInt32(30), temp.Max);
        }



    }
}
