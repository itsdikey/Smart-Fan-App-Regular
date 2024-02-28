using InTheHand.Bluetooth;
using SFAR.Models;
using SFAR.Services.Repositories;

namespace SFAR.Services.Implementations
{
    internal sealed class DeviceDiscoveryService : IDeviceDiscoveryService
    {
        private readonly IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService;

        private BluetoothLEScan? _currentScan;
        public DeviceDiscoveryService(IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService)
        {
            this.discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;

            Bluetooth.AdvertisementReceived += Bluetooth_AdvertisementReceived;
        }

        private void Bluetooth_AdvertisementReceived(object? sender, BluetoothAdvertisingEvent e)
        {
            if (e.Uuids.Contains(Consts.SERVICE_UUID))
            {
                if(!discoveredDevicesRepositoryService.Has(e.Device))
                {
                    discoveredDevicesRepositoryService.AddDevice(e.Device);
                }
                
            }
        }

        public async Task StartScanning()
        {

            var bluetoothLEScanOptions = new BluetoothLEScanOptions() { KeepRepeatedDevices = false, AcceptAllAdvertisements = false };
            var scanFilter = new BluetoothLEScanFilter();
            scanFilter.Services.Add(BluetoothUuid.FromGuid(Consts.SERVICE_UUID));
            bluetoothLEScanOptions.Filters.Add(scanFilter);
            _currentScan = await Bluetooth.RequestLEScanAsync();
        }

        public Task StopScanning()
        {
            if (_currentScan != null)
            {
                _currentScan.Stop();
                _currentScan = null;
            }

            return Task.CompletedTask;
        }
    }
}
