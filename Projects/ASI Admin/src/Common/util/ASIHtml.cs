using System;
using System.Web.Mvc;

namespace asi.asicentral.util
{
    public static class ASIHtml
    {
        public static MvcHtmlString AntiForgeryToken(this System.Web.Mvc.HtmlHelper helper, bool isAsi)
        {
            return MvcHtmlString.Create("<input name='__RequestVerificationToken' type='hidden' value='' />");
        }
    }
}
