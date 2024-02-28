using SFAR.Models.Devices;
using SFAR.Services;
using SFAR.Services.Repositories;
using System.Collections.ObjectModel;
using System.Threading;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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
            Devices = discoveredDevicesRepositoryService.Devices;

            Initialize();
        }

        private async void Initialize()
        {
            if (_selectedDeviceRepositoryService.SelectedDevice != null)
            {
                await _deviceControlService.Disconnect(_selectedDeviceRepositoryService.SelectedDevice);
            }

            _selectedDeviceRepositoryService.SelectedDevice = null;

            _discoveredDevicesRepositoryService.ClearAll();

            var hasPermissions = await _requiredPermissionService.HasAllPermissions();

            if (!hasPermissions)
            {
                string text = "Missing Required Permissions";
                ToastDuration duration = ToastDuration.Short;
                double fontSize = 14;

                var toast = Toast.Make(text, duration, fontSize);

                await toast.Show(default);

                return;
            }


            await _deviceDiscoveryService.StartScanning();
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
