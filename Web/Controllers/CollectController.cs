using IMS.Common;
using IMS.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Controllers
{
    public class CollectController : Controller
    {
        public ICollectService collectService { get; set; }
        public async Task<ActionResult> Set(long taskId, bool isCollect)
        {
            long userId = CookieHelper.GetLoginId();
            long res = await collectService.CollectAsync(userId, taskId, isCollect);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
    }
}