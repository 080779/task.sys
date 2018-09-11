using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Web.Areas.Admin.Models.Forward
{
    public class ForwardListViewModel
    {
        public ForwardDTO[] Forwards { get; set; }
        public long PageCount { get; set; }
        public long TotalCount { get; set; }//转发数量
        public decimal TotalBonus { get; set; }//转发佣金
    }
}