using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace ServerTest
{
    [TestClass]
    public class TestStatic
    {
        // ##################################################################
        [TestMethod]
        public void Static_CheckFileExtensionJpg()
        {
            Static staticPlugin = new Static();

            string[] param = { "JPEG2.jpg" };

            staticPlugin.Init(param);
            staticPlugin.Split();

            Assert.AreEqual("jpg", staticPlugin.FileExtension);
        }

        // ##################################################################
        [TestMethod]
        public void Static_CheckFileExtensionPdf()
        {
            Static staticPlugin = new Static();

            string[] param = { "PDF.pdf" };

            staticPlugin.Init(param);
            staticPlugin.Split();

            Assert.AreEqual("pdf", staticPlugin.FileExtension);
        }

        // ##################################################################
        [TestMethod]
        public void Static_CheckFileExtensionWma()
        {
            Static staticPlugin = new Static();

            string[] param = { "WMA.wma" };

            staticPlugin.Init(param);
            staticPlugin.Split();

            Assert.AreEqual("wma", staticPlugin.FileExtension);
        }

    }
}
