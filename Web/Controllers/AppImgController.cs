using IMS.Common;
using IMS.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IMS.Web.Controllers
{
    /// <summary>
    /// webapi接口
    /// </summary>
    public class AppImgController : ApiController
    {
        public ISettingService settingService { get; set; }
        private string domain = System.Configuration.ConfigurationManager.AppSettings["DoMain"];
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult> get()
        {
            var res = await settingService.GetModelListByDescAsync("引导");
            return new ApiResult { Status = 1, Data = res.Select(s => new Image { Img = domain + s.Parm }) };
        }
        public class Image
        {
            public string Img { get; set; }
        }
    }
}
