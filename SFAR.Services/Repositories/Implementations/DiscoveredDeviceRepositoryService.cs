using InTheHand.Bluetooth;
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

        private readonly Dictionary<SmartFanBLEDevice, BluetoothDevice> _smartToDevice;
        private readonly Dictionary<BluetoothDevice, SmartFanBLEDevice> _deviceToSmart;
        private readonly HashSet<string> _alreadyFoundDevices;

        public DiscoveredDeviceRepositoryService()
        {
            _smartToDevice = new Dictionary<SmartFanBLEDevice, BluetoothDevice>();
            _deviceToSmart = new Dictionary<BluetoothDevice, SmartFanBLEDevice>();
            _alreadyFoundDevices = new HashSet<string>();
        }

        public void AddDevice(BluetoothDevice device)
        {
            _alreadyFoundDevices.Add(device.Id);
            var smartDevice = new SmartFanBLEDevice();
            smartDevice.Name = device.Name;
            smartDevice.Address = device.Id.ToString();
            _smartToDevice.Add(smartDevice, device);
            _deviceToSmart.Add(device, smartDevice);
            Devices.Add(smartDevice);
        }

        public bool Has(BluetoothDevice device)
        {
            return _alreadyFoundDevices.Contains(device.Id);
        }

        public BluetoothDevice? GetDevice(SmartFanBLEDevice smartFanBleDevice)
        {
            if (_smartToDevice.ContainsKey(smartFanBleDevice))
                return _smartToDevice[smartFanBleDevice];

            return null;
        }

        public SmartFanBLEDevice? GetSmartDevice(BluetoothDevice device)
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
            _alreadyFoundDevices.Clear();
        }
    }
}
