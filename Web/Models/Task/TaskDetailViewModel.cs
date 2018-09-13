using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Web.Models.Task
{
    public class TaskDetailViewModel
    {
        public TaskDTO Task { get; set; }
        public string ForwardStateName { get; set; }
    }
}