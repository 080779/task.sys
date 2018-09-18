using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Web.Areas.Admin.Models.Settting
{
    public class SettingListViewModel
    {
        public SettingDTO Address { get; set; }
        public SettingDTO Phone { get; set; }
        public SettingDTO Code { get; set; }
        public SettingDTO AppImg01 { get; set; }
        public SettingDTO AppImg02 { get; set; }
        public SettingDTO AppImg03 { get; set; }
        public SettingDTO Logo { get; set; }
        public SettingDTO About { get; set; }
    }
}