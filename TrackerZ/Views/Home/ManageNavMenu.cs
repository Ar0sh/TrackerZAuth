using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerZ.Views.Home
{
    public class ManageNavMenu
    {
        public static string Index => "Index";
        public static string Table => "Table";
        public static string New => "New";
        public static string Category => "Category";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string TableNavClass(ViewContext viewContext) => PageNavClass(viewContext, Table);
        public static string NewNavClass(ViewContext viewContext) => PageNavClass(viewContext, New);
        public static string CategoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, Category);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
