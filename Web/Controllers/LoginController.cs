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
    public class LoginController : Controller
    {
        public ISettingService settingService { get; set; }
        public IUserService userService { get; set; }
        public ActionResult Login()
        {
            return View();
        }

        public async Task<ActionResult> GetCode()
        {
            string codeUrl = await settingService.GetParmByNameAsync("客服二维码");
            return Json(new AjaxResult { Status = 1, Data = codeUrl });
        }

        public async Task<ActionResult> BindInfo(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号不能为空" });
            }
            if (!Regex.IsMatch(mobile, @"^1\d{10}$"))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号格式不正确" });
            }
            if(string.IsNullOrEmpty(trueName))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户真实姓名不能为空" });
            }
            if (string.IsNullOrEmpty(wechatPayCode))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户微信号不能为空" });
            }
            if (string.IsNullOrEmpty(aliPayCode))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户支付宝账号不能为空" });
            }
            bool flag = await userService.BindInfoAsync(id,mobile,trueName,wechatPayCode,aliPayCode);
            if(!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg="绑定失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "绑定成功" });
        }
    }
}