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
        public ActionResult List()
        {
            return View();
        }

        public ActionResult MyTask()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Take()
        {
            return View();
        }
    }
}