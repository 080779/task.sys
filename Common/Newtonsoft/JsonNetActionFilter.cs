﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IMS.Common.Newtonsoft
{
    public class JsonNetActionFilter : IActionFilter
    {
        /// <summary>
        /// Asp.Net MVC默认也是使用JavaScriptSerializer做json序列化，不好用（DateTime日期的格式化、循环引用、属性名开头小写等）。
        ///而 Json.Net(newtonjs)很好的解决了这些问 题，是.Net中使用频率非常高的json库。		
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //把 filterContext.Result从JsonResult换成JsonNetResult
            //filterContext.Result值得就是Action执行返回的ActionResult对象
            if (filterContext.Result is JsonResult
            && !(filterContext.Result is JsonNetResult))
            {
                JsonResult jsonResult = (JsonResult)filterContext.Result;
                JsonNetResult jsonNetResult = new JsonNetResult();
                jsonNetResult.ContentEncoding = jsonResult.ContentEncoding;
                jsonNetResult.ContentType = jsonResult.ContentType;
                jsonNetResult.Data = jsonResult.Data;
                jsonNetResult.JsonRequestBehavior = jsonResult.JsonRequestBehavior;
                jsonNetResult.MaxJsonLength = jsonResult.MaxJsonLength;
                jsonNetResult.RecursionLimit = jsonResult.RecursionLimit;

                filterContext.Result = jsonNetResult;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}
