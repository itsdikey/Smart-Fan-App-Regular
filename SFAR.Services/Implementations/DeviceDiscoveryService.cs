using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using SFAR.Models;
using SFAR.Services.Repositories;

namespace SFAR.Services.Implementations
{
    internal sealed class DeviceDiscoveryService : IDeviceDiscoveryService
    {
        private readonly IBluetoothLE ble;
        private readonly IAdapter adapter;
        private readonly IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService;
        public DeviceDiscoveryService(IBluetoothLE ble, IAdapter adapter, IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService)
        {
            this.ble = ble;
            this.adapter = adapter;
            this.discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;

            this.adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        }

        private void Adapter_DeviceDiscovered(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            discoveredDevicesRepositoryService.AddDevice(e.Device);
        }
        public async Task StartScanning()
        {
            var scanFilterOptions = new ScanFilterOptions();
            
            scanFilterOptions.ServiceUuids = [Consts.SERVICE_UUID];
            await adapter.StartScanningForDevicesAsync(scanFilterOptions);
        }

        public async Task StopScanning()
        {
            await adapter.StopScanningForDevicesAsync();
        }
    }
}
