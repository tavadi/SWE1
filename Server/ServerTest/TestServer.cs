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
        public void Server_CheckUrlTemperatur_True()
        {
            string PluginName = "Temperatur.html";
            string HTTPHeader = @"GET /Temperatur.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }

        // ##########################################################################################################################################
        [TestMethod]
        public void Server_CheckUrlNavi_True()
        {
            string PluginName = "Navi.html";
            string HTTPHeader = @"GET /Navi.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }

        // ##########################################################################################################################################
        [TestMethod]
        public void Server_CheckUrlRssFeed_True()
        {
            string PluginName = "RssFeed.html";
            string HTTPHeader = @"GET /RssFeed.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }

        // ##########################################################################################################################################
        [TestMethod]
        public void Server_CheckUrlStatic_True()
        {
            string PluginName = "Static.html";
            string HTTPHeader = @"GET /Static.html";

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(HTTPHeader);

            MemoryStream MemoryStream = new MemoryStream(byteArray);

            StreamReader sr = new StreamReader(MemoryStream);

            var request = new Request(sr);

            Assert.AreEqual(PluginName, request.Name);
        }

    }
}
