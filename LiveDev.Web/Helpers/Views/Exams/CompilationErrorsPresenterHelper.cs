using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LiveDev.Web.Helpers.Views.Exams
{
    public static class CompilationErrorsPresenterHelper
    {
        public static MvcHtmlString GetFormatedErrors(this HtmlHelper helper, IList<string> errors)
        {
            return
                new MvcHtmlString(errors.Aggregate(string.Empty,
                                                   (current, error) => current + string.Format("<li>{0}</li>", error)));
        }
    }
}