using System;
using System.Threading.Tasks;
using IMS.Service.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ForwardUnitTest
    {
        private ForwardService forwardService = new ForwardService();
        [TestMethod]
        public async Task TestMethod()
        {
            //await forwardService.ForwardAsync(2,2,"dfsdfewewr");
            //await forwardService.ForwardAsync(2, 1, "dfsdfewewr");
            //await forwardService.Confirm(1,false);
            await forwardService.ConfirmAsync(2, true);
        }
    }
}
