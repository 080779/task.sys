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
    public class TaskController : Controller
    {
        public ITaskService taskService { get; set; }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Info(long id)
        {
            string res = await taskService.GetContentAsync(id);
            return View((object)res);
        }
    }
}