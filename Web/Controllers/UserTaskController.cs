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
    public class UserTaskController : Controller
    {
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Incomes()
        {
            return View();
        }
    }
}