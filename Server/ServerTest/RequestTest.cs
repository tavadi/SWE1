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
        public void Request_CheckPluginName_True()
        {
            string PluginName = "GetTemperatur.html";
            string HTTPHeader = @"GET /GetTemperatur.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var Request = new Request(sr);

            Assert.AreEqual(PluginName, Request.Name);
        }


        [TestMethod]
        public void Request_CheckGET_True()
        {
        }

    }
}
