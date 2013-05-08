using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.util;
using System.Collections.Generic;
using asi.asicentral.model;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class FileSystemTest
    {
        [TestMethod]
        public void List()
        {
            IList<FileModel> files = FileSystemHelper.GetFiles("c:/", null);
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 1);
            Assert.AreNotEqual("..", files.ElementAt(0).Name);
            files = FileSystemHelper.GetFiles(files.ElementAt(0).FullPath, "c:/");
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 1);
            Assert.AreEqual("..", files.ElementAt(0).Name);
        }
    }
}
