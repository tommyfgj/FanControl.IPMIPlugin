using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace FanControl.IPMIPlugin
{
    public struct FanEntiry
    {
        public string name;
        public string index;
        public string oem;
        public int min_speed;

        public FanEntiry(string n, string i, string o, int m)
        {
            name = n;
            index = i;
            oem = o;
            min_speed = m;
        }
    }

    public struct IPMIRemoteInfo
    {
        public string addr;
        public string user;
        public string passwd;

        public IPMIRemoteInfo(string a, string u, string p)
        {
            addr = a;
            user = u;
            passwd = p;
        }

        public string Print()
        {
            return $" -H {addr} -U {user} -P {passwd} ";
        }
    }

    public class IPMIPlugin : IPlugin
    {

        public string Name => "tommyfgj";
        private IPMIRemoteInfo IpmiInfo;
        private FanEntiry TargetFan;

        public void Close()
        {

        }

        public void Initialize()
        {
            IpmiInfo = new IPMIRemoteInfo(
                ConfigurationManager.AppSettings["ipmi.addr"],
                ConfigurationManager.AppSettings["ipmi.user"],
                ConfigurationManager.AppSettings["ipmi.passwd"]
            );

            TargetFan = new FanEntiry(
                ConfigurationManager.AppSettings["ipmi.fan_name"], 
                ConfigurationManager.AppSettings["ipmi.fan_index"],
                ConfigurationManager.AppSettings["ipmi.fan_oem"],
                int.Parse(ConfigurationManager.AppSettings["ipmi.fan_min_speed"])
            );
            Console.WriteLine($"read config, {IpmiInfo.Print()}");
        }

        public void Load(IPluginSensorsContainer _container)
        {
            IEnumerable<IPMIFanManagementControlSensor> fanControls = new[] {
                            new { a=IpmiInfo,b= TargetFan }
                        }.Select(i => new IPMIFanManagementControlSensor(i.a, i.b)).ToArray();

            IEnumerable<IPMIFanManagementFanSensor> fanSensors = new[] {
                            new { a=IpmiInfo,b= TargetFan }
                        }.Select(i => new IPMIFanManagementFanSensor(i.a, i.b)).ToArray();

            _container.ControlSensors.AddRange(fanControls);
            _container.FanSensors.AddRange(fanSensors);
        }
    }
}
