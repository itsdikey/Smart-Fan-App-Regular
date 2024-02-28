
namespace SFAR.Services
{
    public interface IDeviceDiscoveryService : IService
    {
        Task StartScanning();
        Task StopScanning();
    }
}
