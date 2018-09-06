using IMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.Web.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public ApiResult get()
        {
            test t = new test();
            t.Id = 1;
            t.Name = "jenney";
            t.Age = 16;
            return new ApiResult { Status=1,Msg="ig",Data=t};
        }
        public class test
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
