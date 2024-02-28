using SFAR.Models.Devices;
using SFAR.Services;
using SFAR.Services.Repositories;
using System.Collections.ObjectModel;
using System.Threading;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using InTheHand.Bluetooth;
using System.Diagnostics;
using SFAR.Models;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace SFAR.ViewModels
{
    public class DiscoverPageViewModel : ViewModelBase
    {
        private ObservableCollection<SmartFanBLEDevice>? devices;

        public ObservableCollection<SmartFanBLEDevice> Devices 
        { 
            get => devices??new ObservableCollection<SmartFanBLEDevice>(); 
            set
            {
                SetProperty(ref devices, value);
            }
        }

        public IRelayCommand Command { get; }

        private readonly IDiscoveredDevicesRepositoryService _discoveredDevicesRepositoryService;
        private readonly IDeviceDiscoveryService _deviceDiscoveryService;
        private readonly IRequiredPermissionService _requiredPermissionService;
        private readonly ISelectedDeviceRepositoryService _selectedDeviceRepositoryService;
        private readonly IDeviceControlService _deviceControlService;

        public DiscoverPageViewModel(
            IDiscoveredDevicesRepositoryService discoveredDevicesRepositoryService,
            IDeviceDiscoveryService deviceDiscoveryService,
            IRequiredPermissionService requiredPermissionService,
            ISelectedDeviceRepositoryService selectedDeviceRepositoryService,
            IDeviceControlService deviceControlService)
        {
            _discoveredDevicesRepositoryService = discoveredDevicesRepositoryService;
            _deviceDiscoveryService = deviceDiscoveryService;
            _requiredPermissionService = requiredPermissionService;
            _selectedDeviceRepositoryService = selectedDeviceRepositoryService;
            _deviceControlService = deviceControlService;
            //Devices = discoveredDevicesRepositoryService.Devices;

            Command = new RelayCommand(() =>
            {
                Initialize();
            });

           
        }

        private async void Initialize()
        {
            var device = await Bluetooth.RequestDeviceAsync(new RequestDeviceOptions { AcceptAllDevices = true });

            _discoveredDevicesRepositoryService.AddDevice(device);

            _selectedDeviceRepositoryService.SelectedDevice = _discoveredDevicesRepositoryService.GetSmartDevice(device);

            await Shell.Current.GoToAsync("//DeviceControl");

            //var gatt = device.Gatt;
            //Debug.WriteLine("Connecting to GATT Server...");
            //await gatt.ConnectAsync();

            //Debug.WriteLine("Getting Battery Service");
            //var service = await gatt.GetPrimaryServiceAsync(Consts.SERVICE_UUID);

            //Debug.WriteLine("Getting Battery Level Characteristic...");
            //var characteristic = await service.GetCharacteristicAsync(Consts.FAN_SPEED_CHARACTERISTICS_UUID);


            //Debug.WriteLine("Reading Battery Level...");
            //var value = await characteristic.ReadValueAsync();

            //Debug.WriteLine($"Battery Level is {Encoding.UTF8.GetString(value)} %");
            //if (_selectedDeviceRepositoryService.SelectedDevice != null)
            //{
            //    await _deviceControlService.Disconnect(_selectedDeviceRepositoryService.SelectedDevice);
            //}

            //_selectedDeviceRepositoryService.SelectedDevice = null;

            //_discoveredDevicesRepositoryService.ClearAll();

            //var hasPermissions = await _requiredPermissionService.HasAllPermissions();

            //if (!hasPermissions)
            //{
            //    string text = "Missing Required Permissions";
            //    ToastDuration duration = ToastDuration.Short;
            //    double fontSize = 14;

            //    var toast = Toast.Make(text, duration, fontSize);

            //    await toast.Show(default);

            //    return;
            //}


            //await _deviceDiscoveryService.StartScanning();
        }

        public void SelectionChanged(SmartFanBLEDevice? device)
        {
            if (device == null)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {

                await _deviceDiscoveryService.StopScanning();

                _selectedDeviceRepositoryService.SelectedDevice = device;

                await Shell.Current.GoToAsync("//DeviceControl");
            });

        }
    }
}
