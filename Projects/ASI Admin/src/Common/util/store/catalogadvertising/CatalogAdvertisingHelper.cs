using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.util.store.catalogadvertising
{
    public static class CatalogAdvertisingHelper
    {
        public static MvcHtmlString LabelSpanFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> attributes)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? fieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var tag = new TagBuilder("label");
            tag.MergeAttributes(attributes);
            tag.Attributes.Add("for", fieldName);
            tag.InnerHtml = labelText;
            return new MvcHtmlString(tag.ToString());
        }
    }
}
