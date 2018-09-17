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
        public async Task<ActionResult> Info(long id)
        {
            var res = await taskService.GetModelAsync(id, 0);
            return View(res);
        }        
    }
}