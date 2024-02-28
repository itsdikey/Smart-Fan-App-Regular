using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
        private ICommand _speedCommand;

        public int FanSpeed
        {
            get => _fanSpeed; 
            set => SetProperty(ref _fanSpeed, value);
        }

        public ICommand SpeedCommand
        {
            get => _speedCommand; 
            set => SetProperty(ref _speedCommand, value);
        }

        public ControlDevicePageViewModel(ISelectedDeviceRepositoryService selectedDeviceRepositoryService, IDeviceControlService deviceControlService)
        {
            _selectedDeviceRepositoryService = selectedDeviceRepositoryService;
            _deviceControlService = deviceControlService;
            _deviceControlService.FanSpeedChanged += DeviceControlService_FanSpeedChanged;
            SpeedCommand = new Command<int>(async (x) =>
            {
                var result = await _deviceControlService.WriteSpeed(x);
                if (!result)
                {
                    IToast toast = Toast.Make("Failed to update device speed", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);

                    await toast.Show();
                }
            }, (_)=>true);
            Initialize();
        }

        private void DeviceControlService_FanSpeedChanged(int speed)
        {
            FanSpeed = speed;
        }

        private async void Initialize()
        {
            if (_selectedDeviceRepositoryService.SelectedDevice == null)
            {
                return;
            }
            await _deviceControlService.Connect(_selectedDeviceRepositoryService.SelectedDevice);
        }
    }
}
