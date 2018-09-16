using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Areas.Admin.Controllers
{
    public class TestController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(TestController));
        public IAdminLogService adminLogService { get; set; }
        public IPermissionTypeService permissionTypeService { get; set; }
        public ActionResult List()
        {
            permissionTypeService.DelByNameAsync("佣金记录");
            return View();
        }

        public ActionResult Layui()
        {
            return View();
        }

        public async Task<ActionResult> GetPage(int pageIndex = 1)
        {
            var res = await adminLogService.GetModelListAsync(null, null, null, null, pageIndex, 5);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        public ActionResult Index()
        {
            try
            {
                log.DebugFormat("传进来的code：{DateTime.Now}");
            }
            catch(Exception ex)
            {
                log.DebugFormat("错误：{ex.ToString()}");
            }
            return View();
        }

        public ActionResult Code()
        {
            string verifyCode = CommonHelper.GetCaptcha(4);
            TempData["verifyCode"] = verifyCode;
            VerificationCode code = new VerificationCode();
            MemoryStream ms = code.OutputImageStream(verifyCode);
            return File(ms, "image/jpeg");
        }
        public ActionResult getres()
        {
            return Json(new AjaxResult { Status = 1 ,Data=new listres() });
        }
        public class file
        {
            public string src { get; set; }
        }
        public class listres
        {
            public List<file> imgList { get; set; }
        }
    }
}