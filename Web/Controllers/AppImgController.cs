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
        [AllowAnonymous]
        public async Task<ApiResult> get()
        {
            string appImg = await settingService.GetParmByNameAsync("App启动图");
            return new ApiResult { Status = 1, Data = appImg };
        }
    }
}
