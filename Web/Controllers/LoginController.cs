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
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public ISettingService settingService { get; set; }
        public IUserService userService { get; set; }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string name, string password)
        {
            long id = await userService.CheckLoginAsync(name, password);
            if (id <= 0)
            {
                if (id == -3)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "用户账户已经被冻结" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "用户名或密码错误" });
            }
            Session["Platform_UserId"] = id;
            CookieHelper.Login(id.ToString(), "Platform_UserId", false);
            HttpCookie UserCookie = new HttpCookie("Platform_UserId");
            UserCookie["Id"] = id.ToString();
            Response.AppendCookie(UserCookie);
            if (string.IsNullOrEmpty(await userService.GetMobileByIdAsync(id)))
            {
                return Json(new AjaxResult { Status = 1, Msg = "登录成功", Data = "/user/bindinfo" });
            }
            else
            {
                return Json(new AjaxResult { Status = 1, Msg = "登录成功", Data = "/task/index" });
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        public async Task<ActionResult> GetCode()
        {
            string codeUrl = await settingService.GetParmByNameAsync("客服二维码");
            return Json(new AjaxResult { Status = 1, Data = codeUrl });
        }
        public async Task<ActionResult> GetLogo()
        {
            string logoUrl = await settingService.GetParmByNameAsync("系统LOGO");
            return Json(new AjaxResult { Status = 1, Data = logoUrl });
        }
    }
}