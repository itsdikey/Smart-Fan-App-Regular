namespace SFAR.Services
{
    public interface IRequiredPermissionService : IService
    {
        Task<bool> HasAllPermissions();
    }
}
