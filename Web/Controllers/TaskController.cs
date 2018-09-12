using IMS.Common;
using IMS.DTO;
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
    public class TaskController : Controller
    {
        private int pageSize = 10;
        public ITaskService taskService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Get(int? within, int pageIndex=1)
        {
            long userId = Convert.ToInt64(Session["Platform_UserId"]);
            if (within!=null)
            {
                var res = await taskService.GetModelListAsync(userId, 7, pageIndex, pageSize);
                return Json(new AjaxResult { Status = 1, Data = res });
            }
            else
            {
                var res = await taskService.GetModelListAsync(userId, null, pageIndex, pageSize);
                return Json(new AjaxResult { Status = 1, Data = res });
            }
        }

        public async Task<ActionResult> Detail(long id)
        {
            long userId = Convert.ToInt64(Session["Platform_UserId"]);
            TaskDTO dto = await taskService.GetModelAsync(id, userId);
            return View(dto);
        }
    }
}