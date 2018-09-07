using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Web.Areas.Admin.Models.Settting
{
    public class SettingListViewModel
    {
        public SettingDTO Phone { get; set; }
        public SettingDTO Code { get; set; }
        public SettingDTO AppImg { get; set; }
        public SettingDTO Logo { get; set; }
        public SettingDTO About { get; set; }
    }
}