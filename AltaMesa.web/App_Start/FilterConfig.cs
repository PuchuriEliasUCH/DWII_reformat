using AltaMesa.web.App_Start;
using System.Web.Mvc;

namespace AltaMesa.web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            FluentValidationConfig.RegisterFilter(filters);
        }
    }
}
