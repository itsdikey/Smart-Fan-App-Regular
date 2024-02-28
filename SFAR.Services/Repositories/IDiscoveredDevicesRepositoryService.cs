using InTheHand.Bluetooth;
using SFAR.Models.Devices;
using System.Collections.ObjectModel;

namespace SFAR.Services.Repositories
{
    public interface IDiscoveredDevicesRepositoryService : IService
    {
        ObservableCollection<SmartFanBLEDevice> Devices { get; }

        void AddDevice(BluetoothDevice device);
        void ClearAll();
        BluetoothDevice? GetDevice(SmartFanBLEDevice smartFanBleDevice);
        SmartFanBLEDevice? GetSmartDevice(BluetoothDevice device);
        bool Has(BluetoothDevice device);
    }
}
