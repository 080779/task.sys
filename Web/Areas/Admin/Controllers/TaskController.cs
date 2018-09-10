using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class TaskController : Controller
    {
        private int pageSize = 10;
        public ITaskService taskService { get; set; }
        public IAdminService adminService { get; set; }
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        //[AdminLog("公告栏管理", "查看公告管理列表")]
        public async Task<ActionResult> List(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {            
            var result = await taskService.GetModelListAsync(null, keyword, startTime, endTime, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        [ValidateInput(false)]
        //[AdminLog("公告栏管理", "添加公告管理")]
        //[Permission("公告栏管理_新增公告")]
        public async Task<ActionResult> Add(string title, decimal bonus, string condition, string explain, string content, DateTime startTime, DateTime endTime)
        {
            long adminId = Convert.ToInt64(Session["Platform_AdminUserId"]);
            string adminMobile = await adminService.GetMobileByIdAsync(adminId);
            long id = await taskService.AddAsync(title, bonus, condition, explain, content, startTime, endTime,adminMobile);
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加任务成功" });
        }

        [ValidateInput(false)]
        //[AdminLog("公告栏管理", "添加公告管理")]
        //[Permission("公告栏管理_修改公告")]
        public async Task<ActionResult> Edit(long id, string title, decimal bonus, string condition, string explain, string content, DateTime startTime, DateTime endTime)
        {
            bool flag = await taskService.EditAsync(id, title, bonus, condition, explain, content, startTime, endTime);

            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改任务成功" });
        }
        //[AdminLog("公告栏管理", "删除公告管理")]
        //[Permission("公告栏管理_删除公告")]
        public async Task<ActionResult> Del(long id)
        {
            bool flag = await taskService.DelAsync(id);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除任务成功" });
        }
    }
}