using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Server;

namespace ServerTest
{
    [TestClass]
    public class TestServer
    {
        // ##########################################################################################################################################
        // Server
        // ##########################################################################################################################################
        [TestMethod]
        public void Server_CheckUrl_True()
        {
            string PluginName = "GetTemperatur.html";
            string HTTPHeader = @"GET /GetTemperatur.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }
    }
}
