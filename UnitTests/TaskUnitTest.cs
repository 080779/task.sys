using System;
using System.Threading.Tasks;
using IMS.Service.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TaskUnitTest
    {
        private TaskService taskService = new TaskService();
        [TestMethod]
        public async Task TestMethod1()
        {
            //await taskService.AddAsync("2", 2, "转发到朋友圈", "dj", "djdjdjdj", DateTime.Now, DateTime.Now, "admin");
        }
    }
}
