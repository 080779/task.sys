using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static IMS.Common.Pagination;

namespace IMS.Web.Areas.Admin.Models.TakeCash
{
    public class TakeCashListViewModel
    {
        public TakeCashModel[] TakeCashes { get; set; }
        public IdNameDTO[] States { get; set; }
        public long PageCount { get; set; }
    }
    public class TakeCashModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Mobile { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public string StateName { get; set; }
        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public string PayTypeInfo { get; set; }
        public string AdminMobile { get; set; }
        public DateTime CreateTime { get; set; }
    }
}