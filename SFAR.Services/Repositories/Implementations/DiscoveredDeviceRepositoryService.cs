using Plugin.BLE.Abstractions.Contracts;
using SFAR.Models.Devices;
using System.Collections.ObjectModel;

namespace SFAR.Services.Repositories.Implementations
{
    internal sealed class DiscoveredDeviceRepositoryService
        : IDiscoveredDevicesRepositoryService
    {

        private ObservableCollection<SmartFanBLEDevice>? _devices;
        public ObservableCollection<SmartFanBLEDevice> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<SmartFanBLEDevice>();
                }

                return _devices;
            }
        }

        private readonly Dictionary<SmartFanBLEDevice, IDevice> _smartToDevice;
        private readonly Dictionary<IDevice, SmartFanBLEDevice> _deviceToSmart;

        public DiscoveredDeviceRepositoryService()
        {
            _smartToDevice = new Dictionary<SmartFanBLEDevice, IDevice>();
            _deviceToSmart = new Dictionary<IDevice, SmartFanBLEDevice>();
        }

        public void AddDevice(IDevice device)
        {
            var smartDevice = new SmartFanBLEDevice();
            smartDevice.Name = device.Name;
            smartDevice.Address = device.Id.ToString();
            _smartToDevice.Add(smartDevice, device);
            _deviceToSmart.Add(device, smartDevice);
            Devices.Add(smartDevice);
        }

        public IDevice? GetDevice(SmartFanBLEDevice smartFanBleDevice)
        {
            if (_smartToDevice.ContainsKey(smartFanBleDevice))
                return _smartToDevice[smartFanBleDevice];

            return null;
        }

        public SmartFanBLEDevice? GetSmartDevice(IDevice device)
        {
            if (_deviceToSmart.ContainsKey(device))
                return _deviceToSmart[device];

            return null;
        }

        public void ClearAll()
        {
            Devices.Clear();
            _smartToDevice.Clear();
            _deviceToSmart.Clear();
        }
    }
}
