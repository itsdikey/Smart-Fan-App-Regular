using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SFAR.Services;
using SFAR.Services.Repositories;
using System.Windows.Input;

namespace SFAR.ViewModels
{
    public sealed class ControlDevicePageViewModel : ViewModelBase
    {
        private readonly ISelectedDeviceRepositoryService _selectedDeviceRepositoryService;
        private readonly IDeviceControlService _deviceControlService;
        private int _fanSpeed;
        private ICommand? _speedCommand;
        private bool _isLoading;

        public int FanSpeed
        {
            get => _fanSpeed; 
            set => SetProperty(ref _fanSpeed, value);
        }

        public ICommand SpeedCommand
        {
            get => _speedCommand!; 
            set => SetProperty(ref _speedCommand, value);
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        public ControlDevicePageViewModel(ISelectedDeviceRepositoryService selectedDeviceRepositoryService, IDeviceControlService deviceControlService)
        {
            _selectedDeviceRepositoryService = selectedDeviceRepositoryService;
            _deviceControlService = deviceControlService;
            _deviceControlService.FanSpeedChanged += DeviceControlService_FanSpeedChanged;
            LifecycleEventManager.LifecycleEvent += LifecycleEventManager_LifecycleEvent;
            SpeedCommand = new AsyncRelayCommand<string>(async (x) =>
            {
                var result = await _deviceControlService.WriteSpeed(int.Parse(x??"0"));
                if (!result)
                {
                    IToast toast = Toast.Make("Failed to update device speed", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);

                    await toast.Show();
                }
            });
            Initialize();
        }

        private async void LifecycleEventManager_LifecycleEvent(string obj)
        {
            if (obj != "OnStart")
            {
                if (_selectedDeviceRepositoryService.SelectedDevice != null)
                {
                    await _deviceControlService.Disconnect(_selectedDeviceRepositoryService.SelectedDevice);
                }
            }
        }

        private void DeviceControlService_FanSpeedChanged(int speed)
        {
            FanSpeed = speed;
        }

        private async void Initialize()
        {
            IsLoading = true;

            if (_selectedDeviceRepositoryService.SelectedDevice == null)
            {
                return;
            }
            var result = await _deviceControlService.Connect(_selectedDeviceRepositoryService.SelectedDevice);

            IsLoading = !result;

            if (!result)
            {
                IToast toast = Toast.Make("Failed to connect to the device", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);

                await toast.Show();
            }
        }
    }
}
