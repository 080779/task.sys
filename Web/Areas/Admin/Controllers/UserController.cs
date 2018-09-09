using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private int pageSize = 10;
        public IUserService userService { get; set; }
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        [AdminLog("会员管理", "查看用户管理列表")]
        public async Task<ActionResult> List(long? levelId,string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            var result = await userService.GetModelListAsync(keyword, startTime, endTime, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        [Permission("会员管理_新增会员")]
        [AdminLog("会员管理", "添加用户")]
        public async Task<ActionResult> Add(string name, string password)
        {
            if(string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户名不能为空" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "登录密码不能为空" });
            }
            long id= await userService.AddAsync(name,password,null,null);
            if(id<=0)
            {
                if (id == -1)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "用户名已经存在" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "会员添加失败" });
            }            
            return Json(new AjaxResult { Status = 1, Msg = "会员添加成功" });
        }

        [Permission("会员管理_新增会员")]
        public async Task<ActionResult> Check(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户名不能为空" });
            }
            long id = await userService.CheckUserNameAsync(name);
            return Json(new AjaxResult { Status = 1, Msg = "检测成功",Data=id });
        }

        [AdminLog("会员管理", "重置用户密码")]
        [Permission("会员管理_重置密码")]
        public async Task<ActionResult> ResetPwd(long id, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "登录密码不能为空" });
            }
            long res = await userService.ResetPasswordAsync(id,password);
            if (res <= 0)
            {
                if (id == -1)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "用户不存在" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "重置密码失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "重置密码成功" });
        }
        [AdminLog("会员管理", "冻结用户")]
        [Permission("会员管理_冻结用户")]
        public async Task<ActionResult> Frozen(long id)
        {
            bool res= await userService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻用户失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻用户成功" });
        }
        [AdminLog("会员管理", "删除用户")]
        [Permission("会员管理_删除用户")]
        public async Task<ActionResult> Delete(long id)
        {
            long res = await userService.DeleteAsync(id);
            if (res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除用户失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除用户成功" });
        }
    }
}