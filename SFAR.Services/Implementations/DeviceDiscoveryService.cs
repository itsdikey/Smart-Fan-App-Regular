using InTheHand.Bluetooth;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using SFAR.Models;
using SFAR.Services.Repositories;
using System.Diagnostics;

namespace SFAR.Services.Implementations
{
    internal sealed class DeviceDiscoveryService : IDeviceDiscoveryService
    {
        private readonly IBluetoothLE ble;
        private readonly IAdapter adapter;
        private readonly IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService;

        private BluetoothLEScan? _currentScan;
        public DeviceDiscoveryService(IBluetoothLE ble, IAdapter adapter, IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService)
        {
            this.ble = ble;
            this.adapter = adapter;
            this.discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;

            //this.adapter.DeviceDiscovered += Adapter_DeviceDiscovered;

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

        //private void Adapter_DeviceDiscovered(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        //{
        //    discoveredDevicesRepositoryService.AddDevice(e.Device);
        //}
        public async Task StartScanning()
        {

            var bluetoothLEScanOptions = new BluetoothLEScanOptions() { KeepRepeatedDevices = false, AcceptAllAdvertisements = false };
            var scanFilter = new BluetoothLEScanFilter();
            scanFilter.Services.Add(BluetoothUuid.FromGuid(Consts.SERVICE_UUID));
            bluetoothLEScanOptions.Filters.Add(scanFilter);
            var requestScanDeviceOptions = new RequestDeviceOptions();
            requestScanDeviceOptions.Filters.Add(scanFilter);
            _currentScan = await Bluetooth.RequestLEScanAsync(bluetoothLEScanOptions);
        }

        public async Task StopScanning()
        {
            if (_currentScan != null)
            {
                _currentScan.Stop();
                _currentScan = null;
            }
        }
    }
}
