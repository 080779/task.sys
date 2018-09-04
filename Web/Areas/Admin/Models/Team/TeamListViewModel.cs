﻿using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static IMS.Common.Pagination;

namespace IMS.Web.Areas.Admin.Models.Team
{
    public class TeamListViewModel
    {
        public UserDTO TeamLeader { get; set; }
        public UserDTO[] Members { get; set; }
        public long PageCount { get; set; }
        public long TotalCount { get; set; }
        public SettingDTO[] TeamLevels { get; set; }
    }
}