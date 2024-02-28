using SFAR.Models.Devices;
using System.Collections.ObjectModel;

namespace SFAR.Services.Repositories
{
    public interface IDiscoveredDevicesRepositoryService : IService
    {
        ObservableCollection<SmartFanBLEDevice> Devices { get; }

        void AddDevice(Plugin.BLE.Abstractions.Contracts.IDevice device);
        void ClearAll();
        Plugin.BLE.Abstractions.Contracts.IDevice? GetDevice(SmartFanBLEDevice smartFanBleDevice);
        SmartFanBLEDevice? GetSmartDevice(Plugin.BLE.Abstractions.Contracts.IDevice device);
    }
}
