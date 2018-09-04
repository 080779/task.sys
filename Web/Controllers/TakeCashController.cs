﻿using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Web.App_Start.Filter;
using IMS.Web.Models.TakeCash;
using IMS.Web.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace IMS.Web.Controllers
{    
    public class TakeCashController : ApiController
    {
        public ITakeCashService takeCashService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IUserService userService { get; set; }
        public async Task<ApiResult> List(TakeCashListModel model)
        {
            User user = JwtHelper.JwtDecrypt<User>(ControllerContext);
            TakeCashSearchResult res = await takeCashService.GetModelListAsync(user.Id, null, null, null, null, model.PageIndex, model.PageSize);
            TakeCashListApiModel result = new TakeCashListApiModel();
            result.takeCashes = res.TakeCashes.Select(t=>new TakeCash { createTime=t.CreateTime,amount=t.Amount,description=t.Description,payTypeName=t.PayTypeName,stateName=t.StateName});
            result.pageCount = res.PageCount;
            return new ApiResult { status = 1, data = result };
        }
        public async Task<ApiResult> Apply(TakeCashApplyModel model)
        {
            if(model.Amount<=0)
            {
                return new ApiResult { status = 0, msg = "提现金额必须大于零" };
            }            
            if(model.Amount%100!=0)
            {
                return new ApiResult { status = 0, msg = "提现金额必是100的倍数" };
            }
            if (model.PayTypeId <= 0)
            {
                return new ApiResult { status = 0, msg = "提现收款类型id必须大于零" };
            }
            User user = JwtHelper.JwtDecrypt<User>(ControllerContext);
            var userres= await userService.GetModelAsync(user.Id);
            if(userres.LevelName=="普通会员")
            {
                return new ApiResult { status = 0, msg = "普通会员不能提现" };
            }
            if(user==null)
            {
                return new ApiResult { status = 0, msg = "用户不存在"};
            }
            
            long id = await takeCashService.AddAsync(user.Id, model.PayTypeId, model.Amount, "佣金提现");
            if(id<=0)
            {
                if(id==-1)
                {
                    return new ApiResult { status = 0, msg = "用户不存在" };
                }
                if (id == -2)
                {
                    return new ApiResult { status = 0, msg = "用户账户余额不足" };
                }
                if(id==-4)
                {
                    return new ApiResult { status = 0, msg = "-4" };
                }
                return new ApiResult { status = 0, msg="申请提现失败" };
            }
            return new ApiResult { status = 1, msg = "申请提现成功" };
        }
        public async Task<ApiResult> PayTypes()
        {
            var result = await idNameService.GetByTypeNameAsync("收款方式");
            var res = result.Select(i => new { id = i.Id, name = i.Name });
            return new ApiResult { status = 1, data = res };
        }
    }    
}