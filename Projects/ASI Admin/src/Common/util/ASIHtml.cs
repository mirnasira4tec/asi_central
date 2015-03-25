using System;
using System.Web.Mvc;

namespace asi.asicentral.util
{
    public static class ASIHtml
    {
        public static MvcHtmlString AntiForgeryToken(this System.Web.Mvc.HtmlHelper helper)
        {
            return new MvcHtmlString("<input name='__RequestVerificationToken' type='hidden' value='' />");
        }
    }
}
