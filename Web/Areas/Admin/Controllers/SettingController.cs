using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using IMS.Web.Areas.Admin.Models.Settting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        public ISettingService settingService { get; set; }
        //[Permission("日志管理_查看日志")]
        public ActionResult List()
        {            
            return View();
        }
        [HttpPost]
        //[Permission("日志管理_查看日志")]
        [AdminLog("系统设置", "查看系统设置")]
        public async Task<ActionResult> List(bool flag=true)
        {
            SettingListViewModel model = new SettingListViewModel();
            model.Phone = await settingService.GetModelByNameAsync("客服电话");
            model.Code = await settingService.GetModelByNameAsync("客服二维码");
            model.AppImg = await settingService.GetModelByNameAsync("App启动图");
            model.Logo = await settingService.GetModelByNameAsync("系统LOGO");
            model.About = await settingService.GetModelByNameAsync("关于我们");
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        [HttpPost]
        [ValidateInput(false)]
        [AdminLog("系统设置", "编辑系统设置")]
        [Permission("系统设置_系统设置")]
        public async Task<ActionResult> Edit(List<SettingDTO> settings)
        {
            if(settings.Count()<=0)
            {
                return Json(new AjaxResult { Status = 0,Msg="无参数"});
            }
            string path = "";
            bool res = false;
            foreach (var item in settings)
            {
                if(item.Parm.Contains(";base64,"))
                {
                    bool flag =ImageHelper.SaveBase64(item.Parm, out path);
                    if(!flag)
                    {
                        return Json(new AjaxResult { Status = 0, Msg = "图片保存失败" });
                    }
                    res = await settingService.UpdateAsync(item.Id, path);
                }
                else
                {
                    res = await settingService.UpdateAsync(item.Id, item.Parm);
                }
            }
            if(!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改失败" });
            }
            return Json(new AjaxResult { Status = 1,Msg="修改成功" });
        }
    }
}