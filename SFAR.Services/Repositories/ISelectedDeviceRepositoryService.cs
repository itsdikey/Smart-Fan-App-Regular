using SFAR.Models.Devices;

namespace SFAR.Services.Repositories
{
    public interface ISelectedDeviceRepositoryService : IService
    {
        public SmartFanBLEDevice? SelectedDevice { get; set; }
    }
}
