using InTheHand.Bluetooth;
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
        public event Action<bool>? ReadyChanged;

        private readonly IDiscoveredDevicesRepositoryService _discoveredDevicesRepositoryService;

        private GattCharacteristic? _currentCharacteristic;

        public DeviceControlService(IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService)
        {
            _discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;
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

                var devices = await Bluetooth.GetPairedDevicesAsync();

                var targetDevice = devices.FirstOrDefault(x => x.Id == device.Id);

                if(targetDevice != null)
                {
                    targetDevice.Gatt.Disconnect();

                    await Task.Delay(500);
                }
           

                RemoteGattServer? gatt;

                try
                {
                    await Task.Delay(1000);
                    if (targetDevice != null)
                    {
                        gatt = targetDevice.Gatt;
                    }
                    else
                    {
                        gatt = device.Gatt;
                    }
                    await gatt.ConnectAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception {ex}");
                    result.SetResult(false);
                    return;
                }

                if (gatt == null)
                {
                    return;
                }

                try
                {

                    if (!gatt.IsConnected)
                    {
                        OnReadyChanged(false);
                    }

                    var service = await gatt.GetPrimaryServiceAsync(BluetoothUuid.FromGuid(Consts.SERVICE_UUID));

                    if (service == null)
                    {
                        result.SetResult(false);
                        return;
                    }

                    var characteristic = await service.GetCharacteristicAsync(Consts.FAN_SPEED_CHARACTERISTICS_UUID);
                    _currentCharacteristic = characteristic;
                    _currentCharacteristic.CharacteristicValueChanged += Characteristic_ValueUpdated;
                    var value = await characteristic.ReadValueAsync();

                    OnValueChange(value);
                    OnReadyChanged(true);

                    result.SetResult(true);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    result.SetResult(false);
                    return;
                }
               
                return;
            });

            return result.Task;
        }

        private void Characteristic_ValueUpdated(object? sender, GattCharacteristicValueChangedEventArgs e)
        {
            OnValueChange(e.Value);
        }

        private void OnReadyChanged(bool value)
        {
            ReadyChanged?.Invoke(value);
        }

        public void OnValueChange(byte[] value)
        {
            if(value == null || value.Length == 0)
            {
                OnFanSpeedChanged(0);
                return;
            }

            OnFanSpeedChanged(int.Parse(Encoding.UTF8.GetString(value)));
        }

        public async Task<bool> WriteSpeed(int speed)
        {
            if (_currentCharacteristic == null)
                return false;

            await _currentCharacteristic.WriteValueWithoutResponseAsync(Encoding.UTF8.GetBytes(speed.ToString()));

            await Task.Delay(500);

            var value = await _currentCharacteristic.ReadValueAsync();
            OnValueChange(value);
            return true;
        }

        public Task Disconnect(SmartFanBLEDevice smartDevice)
        {
            var device = _discoveredDevicesRepositoryService.GetDevice(smartDevice);

            if (device == null)
                return Task.CompletedTask;


            device.Gatt.Disconnect();

            return Task.CompletedTask;
        }

        private void OnFanSpeedChanged(int speed)
        {
            FanSpeedChanged?.Invoke(speed);
        }
    }
}
