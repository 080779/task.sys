﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Copy()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Pojie()
        {
            return View();
        }
    }
}