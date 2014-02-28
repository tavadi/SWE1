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

            string[] param = { "EditSave", "0", "FeedName", "Golem", "FeedUrl", "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("Golem", feed.Name);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckUrl_True()
        {
            RssFeed feed = new RssFeed();

            string url = "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0";

            string[] param = { "EditSave", "0", "FeedName", "Golem", "FeedUrl", url };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual(url, feed.Url);
        }


        // ##########################################################################################################################################
        // Database Connection
        // ##########################################################################################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionUsername_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "0", "FeedName", "Golem", "FeedUrl", "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual(".\\SqlExpress", feed.Username);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionDatabase_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "0", "FeedName", "Golem", "FeedUrl", "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("SWE_Temperatur", feed.Database);
        }

        // ##################################################################
        [TestMethod]
        public void RssFeed_CheckDbConnectionSecurity_True()
        {
            RssFeed feed = new RssFeed();

            string[] param = { "EditSave", "0", "FeedName", "Golem", "FeedUrl", "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0" };

            feed.Init(param);
            feed.ParseParameters();

            Assert.AreEqual("true", feed.Security);
        }
    }
}
