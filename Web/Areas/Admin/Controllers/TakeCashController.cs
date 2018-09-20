using IMS.Common;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using IMS.Web.Areas.Admin.Models.TakeCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class TakeCashController : Controller
    {
        public ITakeCashService takeCashService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IUserService userService { get; set; }
        private int pageSize = 10;
        //[Permission("积分管理_积分管理")]
        public ActionResult List()
        {
            return View();
        }
        //[Permission("积分管理_积分管理")]
        [AdminLog("提现管理", "查看提现管理列表")]
        [HttpPost]
        public async Task<ActionResult> List(long? stateId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            TakeCashSearchResult result = await takeCashService.GetModelListAsync(null, stateId, keyword, startTime, endTime, pageIndex, pageSize);
            TakeCashListViewModel model = new TakeCashListViewModel();
            model.TakeCashes = result.TakeCashes.Select(t=>new TakeCashModel {
                AdminMobile=t.AdminMobile,
                Amount=t.Amount,
                Code=t.Code,
                CreateTime=t.CreateTime,
                Description=t.Description,
                Id=t.Id,
                Mobile=t.Mobile,
                Name=t.Name,
                PayTypeInfo=userService.GetPayInfo(t.UserId,t.TypeName),
                StateId=t.StateId,
                StateName=t.StateName,
                TypeId=t.TypeId,
                TypeName=t.TypeName,
                UserId=t.UserId                
            }).ToArray();
            model.PageCount = result.PageCount;
            model.States = await idNameService.GetByTypeNameAsync("提现状态");
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        [HttpPost]
        [AdminLog("提现管理", "标记结款")]
        [Permission("提现管理_标记结款")]
        public async Task<ActionResult> Confirm(long id, bool isSuccess)
        {
            long res = await takeCashService.Confirm(id, Convert.ToInt64(Session["Platform_AdminUserId"]), isSuccess);
            if(res<=0)
            {
                if(res==-4)
                {
                    return Json(new AjaxResult { Status = 1, Msg = "取消结款成功" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "结款失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "结款成功" });
        }

        [HttpPost]
        [AdminLog("提现管理", "取消结款")]
        [Permission("提现管理_取消结款")]
        public async Task<ActionResult> Cancel(long id, bool isSuccess)
        {
            long res = await takeCashService.Confirm(id, Convert.ToInt64(Session["Platform_AdminUserId"]), isSuccess);
            if (res <= 0)
            {
                if (res == -4)
                {
                    return Json(new AjaxResult { Status = 1, Msg = "取消结款成功" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "取消结款失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "取消结款成功" });
        }
    }
}