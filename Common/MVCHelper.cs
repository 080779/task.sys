using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IMS.Common
{
    public static class MVCHelper
    {
        //页面静态化方法
        public static string RenderViewToString(ControllerContext context, string viewPath, object model = null)
        {
            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult == null)
            {
                throw new FileNotFoundException("View" + viewPath + "cannot be found.");
            }                
            IView view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewContext ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                return sw.ToString();
            }
        }
    }
}
