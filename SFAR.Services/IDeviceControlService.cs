

using SFAR.Models.Devices;

namespace SFAR.Services
{
    public interface IDeviceControlService : IService
    {
        event Action<int>? FanSpeedChanged;

        Task<bool> Connect(SmartFanBLEDevice smartDevice);
        Task Disconnect(SmartFanBLEDevice smartDevice);
        Task<bool> WriteSpeed(int speed);
    }
}
