﻿using IMS.Common;
using IMS.IService;
using IMS.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Controllers
{
    public class UserController : Controller
    {
        private int pageSize = 10;
        private long userId = CookieHelper.GetLoginId();
        public ISettingService settingService { get; set; }
        public IUserService userService { get; set; }
        public ITaskService taskService { get; set; }
        public ActionResult Center()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Get()
        {
            var res = await userService.GetModelAsync(userId);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        public ActionResult MyTask()
        {
            return View();
        }

        public async Task<ActionResult> GetCollects(int pageIndex = 1)
        {
            var res = await taskService.GetModelListCollectAsync(userId, pageIndex, 30);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        public async Task<ActionResult> GetForwards(int pageIndex = 1)
        {
            var res = await taskService.GetModelListForwardAsync(userId, null, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        //联系客服
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> BindInfo(string type= "bar")
        {
            ViewBag.Type = type;
            var res = await userService.GetModelAsync(userId);
            return View(res);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Send(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号不能为空" });
            }
            if (!Regex.IsMatch(mobile, @"^1\d{10}$"))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号格式不正确" });
            }
            string state;
            string msgState;
            string code = CommonHelper.GetNumberCaptcha(4);
            string content = string.Format(System.Configuration.ConfigurationManager.AppSettings["SMS_Template1"], code);
            string stateCode = CommonHelper.SendMessage2(mobile, content, out state, out msgState);
            CacheHelper.SetCache("App_User_SendMsg" + mobile, code, DateTime.UtcNow.AddMinutes(5), TimeSpan.Zero);
            if (stateCode != "0")
            {
                return Json(new AjaxResult { Status = 0, Msg = "发送短信返回消息：" + msgState });
            }
            return Json(new AjaxResult { Status = 1, Msg = "发送短信成功" + msgState });
        }

        [HttpPost]
        public async Task<ActionResult> BindInfo(string mobile, string trueName, string wechat, string alipay, string code)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号不能为空" });
            }
            if (!Regex.IsMatch(mobile, @"^1\d{10}$"))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户手机号格式不正确" });
            }
            //if (string.IsNullOrEmpty(trueName))
            //{
            //    return Json(new AjaxResult { Status = 0, Msg = "用户真实姓名不能为空" });
            //}
            if (string.IsNullOrEmpty(wechat))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户微信号不能为空" });
            }
            if (string.IsNullOrEmpty(alipay))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户支付宝账号不能为空" });
            }
            if (string.IsNullOrEmpty(code))
            {
                return Json(new AjaxResult { Status = 0, Msg = "手机验证码不能为空" });
            }
            object obj = CacheHelper.GetCache("App_User_SendMsg" + mobile);
            if (obj == null)
            {
                return Json(new AjaxResult { Status = 0, Msg = "未发送短信验证或验证码过期" });
            }
            if (obj.ToString() != code)
            {
                return Json(new AjaxResult { Status = 0, Msg = "手机验证码错误" });
            }
            long res = await userService.BindInfoAsync(userId, mobile, trueName, wechat, alipay);
            if (res <= 0)
            {
                if (res == -2)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "该手机号码已被绑定" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "绑定失败" });
            }
            //if (Request.Cookies["Platform_UserId"] != null)
            //{
            //    HttpCookie UserCookie = Request.Cookies["Platform_UserId"];
            //    UserCookie["Mobile"] = mobile;
            //    Response.Cookies.Set(UserCookie);
            //}
            return Json(new AjaxResult { Status = 1, Msg = "绑定成功" });
        }

        [HttpPost]
        public async Task<ActionResult> CheckBind()
        {
            string mobile = await userService.GetMobileByIdAsync(userId);
            if(string.IsNullOrEmpty(mobile))
            {
                return Json(new AjaxResult { Status = 0, Msg = "未绑定手机号、微信、支付宝信息",Data="/user/bindinfo" });
            }
            return Json(new AjaxResult { Status = 1,Data="/takecash/take" });
        }


        [HttpGet]
        public ActionResult Edit(UserEditModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string file, string nickName)
        {
            if (string.IsNullOrEmpty(file))
            {
                return Json(new AjaxResult { Status = 0, Msg = "请选择头像图片" });
            }
            bool res = false;
            if (file.Contains(";base64,"))
            {
                string path;
                bool flag = ImageHelper.SaveBase64(file, out path);
                if (!flag)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "图片保存失败" });
                }
                res = await userService.UpdateInfoAsync(userId, nickName, path);
            }
            else
            {
                res = await userService.UpdateInfoAsync(userId, nickName, file);
            }
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "信息修改失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "信息修改成功", Data = "/user/center" });
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["Platform_UserId"] != null)
            {
                HttpCookie user = Request.Cookies["Platform_UserId"];
                user.Expires = DateTime.Now.AddDays(-2);
                Response.Cookies.Set(user);
            }
            return Redirect("/login/login");
        }
    }
}