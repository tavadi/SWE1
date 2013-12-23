using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace ServerTest
{
    [TestClass]
    public class TestRssFeed
    {
        // ##########################################################################################################################################
        // RssFeed
        // ##########################################################################################################################################
        [TestMethod]
        public void RssFeed_CheckName_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "test", "FeedUrl", "www.google.at" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("test", feed.Name);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckUrl_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "test", "FeedUrl", "www.google.at" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("www.google.at", feed.Url);
        }


        // ##########################################################################################################################################
        // Database Connection
        // ##########################################################################################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionUsername_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "test", "FeedUrl", "www.google.at" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual(".\\SqlExpress", feed.Username);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionDatabase_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "test", "FeedUrl", "www.google.at" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("SWE_Temperatur", feed.Database);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionSecurity_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "test", "FeedUrl", "www.google.at" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("true", feed.Security);
        }
    }
}
