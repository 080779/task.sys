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
        private long userId = CookieHelper.GetLoginId();
        public ITakeCashService takeCashService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IUserService userService { get; set; }
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> List(int pageIndex = 1)
        {
            long stateId = await idNameService.GetIdByNameAsync("已结款");
            TakeCashSearchResult result = await takeCashService.GetModelListAsync(userId, stateId, null,null,null,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        [HttpGet]
        public async Task<ActionResult> Take()
        {            
            decimal amount = await userService.GetAmountByIdAsync(userId);
            return View(amount);
        }
        [HttpPost]
        public async Task<ActionResult> Take(long payTypeId,decimal amount)
        {
            long res = await takeCashService.AddAsync(userId,payTypeId,amount, "");
            if(res<=0)
            {
                if (res == -2)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "账户余额不足" });
                }
                if (res == -4)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "未绑定信息，请绑定信息后再申请", Data = "/user/bindinfo" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "提现申请失败" });
            }
            return Json(new AjaxResult { Status = 1,Msg="提现申请成功" });
        }
    }
}