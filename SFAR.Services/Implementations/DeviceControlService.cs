using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using SFAR.Models;
using SFAR.Models.Devices;
using SFAR.Services.Repositories;
using System.Diagnostics;
using System.Text;

namespace SFAR.Services.Implementations
{
    internal sealed class DeviceControlService : IDeviceControlService
    {
        public event Action<int>? FanSpeedChanged;

        private readonly IDiscoveredDevicesRepositoryService _discoveredDevicesRepositoryService;
        private readonly IAdapter _adapter;

        private ICharacteristic? _currentCharacteristic;

        public DeviceControlService(IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService, IAdapter adapter)
        {
            _discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;
            _adapter = adapter;
        }

        public Task<bool> Connect(SmartFanBLEDevice smartDevice)
        {
            var device = _discoveredDevicesRepositoryService.GetDevice(smartDevice);

            TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (device == null)
                {
                    result.SetResult(false);
                    return;
                }

                try
                {
                    await _adapter.StopScanningForDevicesAsync();
                    await Task.Delay(1000);
                    await _adapter.ConnectToDeviceAsync(device);
                }
                catch (DeviceConnectionException ex)
                {
                    Debug.WriteLine($"Connection Exception {ex}");
                    result.SetResult(false);
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception {ex}");
                    result.SetResult(false);
                    return;
                }

                var service = await device.GetServiceAsync(Consts.SERVICE_UUID);

                if (service == null)
                {
                    result.SetResult(false);
                    return;
                }

                //var characteristic = await service.GetCharacteristicAsync(Consts.FAN_SPEED_CHARACTERISTICS_UUID);

                //_currentCharacteristic = characteristic;

                //characteristic.ValueUpdated += Characteristic_ValueUpdated;

                //OnFanSpeedChanged(int.Parse(characteristic.StringValue));

                result.SetResult(true);
                return;
            });

            return result.Task;
        }

        public async Task<bool> WriteSpeed(int speed)
        {
            if (_currentCharacteristic == null)
                return false;

            await _currentCharacteristic.WriteAsync(Encoding.UTF8.GetBytes(speed.ToString()));
                
            return true;
        }

        public async Task Disconnect(SmartFanBLEDevice smartDevice)
        {
            var device = _discoveredDevicesRepositoryService.GetDevice(smartDevice);

            if (device == null)
                return;

            await _adapter.DisconnectDeviceAsync(device);
        }

        private void Characteristic_ValueUpdated(object? sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            OnFanSpeedChanged(int.Parse(e.Characteristic.StringValue));
        }

        private void OnFanSpeedChanged(int speed)
        {
            FanSpeedChanged?.Invoke(speed);
        }
    }
}
