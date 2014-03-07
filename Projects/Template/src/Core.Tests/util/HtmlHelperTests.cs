using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace asi.asicentral.util.Tests
{

    [TestClass()]
    public class HtmlHelperTests
    {

        [TestMethod()]
        public void SubmitWebRequestTest()
        {
            string url = "http://stage-store.asicentral.com/";
            string content = "test";
   
            string result = HtmlHelper.SubmitWebRequest(url, null, content, true);

            Assert.IsNotNull(result);
        }
    }
}
