using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using IMS.Web.Areas.Admin.Models.Forward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class ForwardController : Controller
    {
        private int pageSize = 10;
        public IForwardService forwardService { get; set; }
        public IAdminService adminService { get; set; }
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        [AdminLog("转发管理", "查看转发管理列表")]
        public async Task<ActionResult> List(string keyword, long? stateId, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            ForwardListViewModel model = new ForwardListViewModel();
            var result = await forwardService.GetModelListAsync(keyword, stateId, startTime, endTime, pageIndex, pageSize);
            var res = await forwardService.GetMonthAsync(DateTime.Now);
            model.Forwards = result.Forwards;
            model.PageCount = result.PageCount;
            model.TotalBonus = res.TotalBonus;
            model.TotalCount = res.TotalCount;
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        [HttpPost]
        //[AdminLog("公告栏管理", "查看公告管理列表")]
        public async Task<ActionResult> GetDay(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return Json(new AjaxResult { Status = 0, Msg = "请选择时间" });
            }
            var res = await forwardService.GetDayAsync(dateTime);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        [HttpPost]
        //[AdminLog("公告栏管理", "查看公告管理列表")]
        public async Task<ActionResult> GetMonth(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return Json(new AjaxResult { Status = 0, Msg = "请选择时间" });
            }
            var res = await forwardService.GetMonthAsync(dateTime);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        [HttpPost]
        [AdminLog("转发管理", "转发审核")]
        [Permission("转发管理_转发审核")]
        public async Task<ActionResult> Confirm(long id, bool auditState)
        {
            long res = await forwardService.ConfirmAsync(id, auditState);
            if (res <= 0)
            {
                if (res == -3)
                {
                    return Json(new AjaxResult { Status = 1, Msg = "取消任务成功" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "审核失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "审核成功" });
        }

        [HttpPost]
        [AdminLog("转发管理", "转发审核")]
        [Permission("转发管理_转发审核")]
        public async Task<ActionResult> AllConfirm(long[] ids, bool auditState)
        {
            if (ids.Count() <= 0)
            {
                return Json(new AjaxResult { Status = 1, Msg = "未选择任何转发" });
            }
            foreach (long id in ids)
            {
                long res = await forwardService.ConfirmAsync(id, auditState);
                if (res <= 0)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "批量审核出错" });
                }
            }
            return Json(new AjaxResult { Status = 1, Msg = "批量审核成功" });
        }

        [HttpPost]
        [AdminLog("转发管理", "取消审核")]
        [Permission("转发管理_取消审核")]
        public async Task<ActionResult> Cancel(long id, bool auditState)
        {
            long res = await forwardService.ConfirmAsync(id, auditState);
            if (res <= 0)
            {
                if (res == -3)
                {
                    return Json(new AjaxResult { Status = 1, Msg = "取消任务成功" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "取消任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "取消任务成功" });
        }

        [HttpPost]
        [AdminLog("转发管理", "取消审核")]
        [Permission("转发管理_取消审核")]
        public async Task<ActionResult> AllCancel(long[] ids, bool auditState)
        {
            if (ids.Count() <= 0)
            {
                return Json(new AjaxResult { Status = 1, Msg = "未选择任何转发" });
            }
            foreach (long id in ids)
            {
                long res = await forwardService.ConfirmAsync(id, auditState);
                if (res <= 0)
                {
                    if (res == -3)
                    {
                        continue;
                    }
                    return Json(new AjaxResult { Status = 0, Msg = "批量取消任务出错" });
                }
            }
            return Json(new AjaxResult { Status = 1, Msg = "批量取消任务成功" });
        }
    }
}