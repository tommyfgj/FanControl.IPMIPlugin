using FanControl.Plugins;

namespace FanControl.IPMIPlugin
{
    public class IPMIFanManagementControlSensor: IPluginControlSensor
    {
        private readonly FanEntiry _fanIndex;
        private readonly IPMIRemoteInfo _ipmiInfo;
        private float? _val;

        public IPMIFanManagementControlSensor(IPMIRemoteInfo IpmiInfo, FanEntiry fanIndex)
        {
            _fanIndex = fanIndex;
            _ipmiInfo = IpmiInfo;
        }

        public float? Value { get; private set; }

        public string Name => $"{_fanIndex.oem} {_fanIndex.name}";

        public string Id => _fanIndex.name;

        public void Reset()
        {
            IPMICommandExecutor.SetFanSpeedByPercentage(_ipmiInfo, _fanIndex, _fanIndex.min_speed);
        }

        public void Set(float val)
        {
            _val = val;
            int valInt = (int)val;
            if (valInt < _fanIndex.min_speed)
            {
                valInt = _fanIndex.min_speed;
            }
            IPMICommandExecutor.SetFanSpeedByPercentage(_ipmiInfo, _fanIndex, valInt);
        }

        public void Update() => Value = _val;
    }
}
