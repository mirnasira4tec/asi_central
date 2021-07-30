using System.Collections.Generic;
using NUnit.Framework;

namespace asi.asicentral.util.Tests
{

    [TestFixture]
    public class HtmlHelperTests
    {
        private string url = "https://stage-store.asicentral.com/Store/Supplier/Package/9";

        [Test]
        public void SubmitWebRequest_Get_Test()
        {
            string result = HtmlHelper.SubmitWebRequest(url, null, null, false);

            Assert.IsNotNull(result);
        }

        [Test]
        public void SubmitWebRequest_Post_Test()
        {
            var data = new Dictionary<string, string>
            {
                {"orderId", "2"},
                {"pageNumber", "2"}
            };

            string result = HtmlHelper.SubmitForm(url, data, true, true);

            Assert.IsNotNull(result);
        }
    }
}
