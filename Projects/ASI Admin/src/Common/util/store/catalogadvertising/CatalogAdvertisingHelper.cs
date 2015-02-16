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

        public static void CopyModelState(ModelStateDictionary from, ModelStateDictionary to)
        {
            if (from == null || to == null) return;
            var keys = from.Keys.ToList();
            var values = from.Values.ToList();
            for (var i = 0; i < keys.Count; i++)
            {
                var errors = values[i].Errors.ToList();
                for (var j = 0; j < errors.Count; j++)
                {
                    to.AddModelError(keys[i], errors[j].ErrorMessage);
                }
            }
        }
    }
}
