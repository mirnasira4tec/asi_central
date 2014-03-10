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
        public void SubmitWebRequest_Post_Test()
        {
            string url = "http://stage-store.asicentral.com/Store/Supplier/Package/9";
            string content = "Become an ASI Supplier Member Today";
   
            string result = HtmlHelper.SubmitWebRequest(url, null, content, true);

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SubmitWebRequest_Get_Test()
        {
            string url = "http://stage-store.asicentral.com/Store/Supplier/Package/9";
      
            string result = HtmlHelper.SubmitWebRequest(url, null, null, false);

            Assert.IsNotNull(result);
        }
    }
}
