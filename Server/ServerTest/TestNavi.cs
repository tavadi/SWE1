using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace ServerTest
{
    [TestClass]
    public class TestNavi
    {
        // ##################################################################
        [TestMethod]
        public void Navi_CheckParameterPreparing()
        {
            Navi navi = new Navi();

            string[] param = { "Preparing" };

            navi.Init(param);

            Assert.AreEqual("Preparing", navi.Param);
        }

        // ##################################################################
        [TestMethod]
        public void Navi_CheckParameterNavigation()
        {
            Navi navi = new Navi();

            string[] param = { "Navigation" };

            navi.Init(param);

            Assert.AreEqual("Navigation", navi.Param);
        }
    }
}
