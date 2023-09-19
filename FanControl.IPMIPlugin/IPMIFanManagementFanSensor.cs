using FanControl.Plugins;

namespace FanControl.IPMIPlugin
{
    public class IPMIFanManagementFanSensor : IPluginSensor
    {
        private readonly FanEntiry _fanIndex;
        private readonly IPMIRemoteInfo _ipmiInfo;

        public IPMIFanManagementFanSensor(IPMIRemoteInfo IpmiInfo, FanEntiry fanIndex)
        {
            _fanIndex = fanIndex;
            _ipmiInfo = IpmiInfo;
        }

        public float? Value { get; private set; }

        public string Name => $"{_fanIndex.oem} {_fanIndex.name}";

        public string Id => _fanIndex.name;

        public void Update() => Value = IPMICommandExecutor.GetFanSpeed(_ipmiInfo, _fanIndex);
    }
}
