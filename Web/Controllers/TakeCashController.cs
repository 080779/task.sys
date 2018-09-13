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
    public class TakeCashController : Controller
    {
        private int pageSize = 10;
        public ITakeCashService takeCashService { get; set; }
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> List(int pageIndex = 1)
        {
            long userId = CookieHelper.GetLoginId();
            TakeCashSearchResult result = await takeCashService.GetModelListAsync(userId,null,null,null,null,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
    }
}