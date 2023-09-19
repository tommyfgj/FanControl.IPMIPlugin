using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FanControl.IPMIPlugin;
using System.Threading;
using System.Configuration;

namespace FanControlTest
{
    [TestClass]
    public class IPMICommandExecutorTests
    {
        [TestMethod]
        public void TestSetFanSpeedByPercentage()
        {

            IPMIRemoteInfo IpmiInfo = new IPMIRemoteInfo(
                ConfigurationManager.AppSettings["ipmi.addr"],
                ConfigurationManager.AppSettings["ipmi.user"],
                ConfigurationManager.AppSettings["ipmi.passwd"]
            );

            FanEntiry TargetFan = new FanEntiry(
                ConfigurationManager.AppSettings["ipmi.fan_name"],
                ConfigurationManager.AppSettings["ipmi.fan_index"],
                ConfigurationManager.AppSettings["ipmi.fan_oem"],
                int.Parse(ConfigurationManager.AppSettings["ipmi.fan_min_speed"])
            );

            int val = 100;

            try
            {
                // Act
                IPMICommandExecutor.SetFanSpeedByPercentage(IpmiInfo, TargetFan, val);

                Thread.Sleep(5000);

                float result = IPMICommandExecutor.GetFanSpeed(IpmiInfo, TargetFan);
                // Assert
                Assert.IsTrue(result>4000);
            } catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }

            IPMICommandExecutor.SetFanSpeedByPercentage(IpmiInfo, TargetFan, 60);

        }
    }
}
