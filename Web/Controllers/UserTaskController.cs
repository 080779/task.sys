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
    public class UserTaskController : Controller
    {
        private int pageSize = 10;
        private long userId = CookieHelper.GetLoginId();
        public IJournalService journalService { get; set; }
        public IForwardStateService forwardStateService { get; set; }
        public ITaskService taskService { get; set; }
        public IIdNameService idNameService { get; set; }
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> List(int pageIndex = 1)
        {
            long stateId = await forwardStateService.GetIdByNameAsync("任务完成");
            var result = await taskService.GetModelListForwardAsync(userId, stateId, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        [HttpGet]
        public ActionResult Incomes()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Incomes(int pageIndex = 1)
        {
            long userId = CookieHelper.GetLoginId();
            long journalTypeId = await idNameService.GetIdByNameAsync("任务转发");
            JournalSearchResult result = await journalService.GetModelListAsync(userId, journalTypeId, null, null, null, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
    }
}