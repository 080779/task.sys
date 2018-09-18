using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Web.Models.Home
{
    public class HomeAboutViewModel
    {
        public SettingDTO Phone { get; set; }
        public SettingDTO Address { get; set; }
        public SettingDTO Logo { get; set; }
        public SettingDTO About { get; set; }
    }
}