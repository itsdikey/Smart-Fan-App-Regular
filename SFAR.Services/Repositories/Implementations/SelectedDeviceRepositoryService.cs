using SFAR.Models.Devices;

namespace SFAR.Services.Repositories.Implementations
{
    internal sealed class SelectedDeviceRepositoryService : ISelectedDeviceRepositoryService
    {
        public SmartFanBLEDevice? SelectedDevice { get; set; }
    }
}
