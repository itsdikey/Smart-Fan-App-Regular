using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SFAR.Services.Implementations
{
    internal class RequiredPermissionService : IRequiredPermissionService
    {
        public async Task<bool> HasAllPermissions()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if(status != PermissionStatus.Granted)
            {
                var result = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if(result != PermissionStatus.Granted)
                {
                    return false;
                }
            }

            status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

            if (status != PermissionStatus.Granted)
            {
                var result = await Permissions.RequestAsync<Permissions.Bluetooth> ();

                if (result != PermissionStatus.Granted)
                {
                    return false;
                }
            }


            status = await Permissions.CheckStatusAsync<Permissions.NearbyWifiDevices>();

            if (status != PermissionStatus.Granted)
            {
                var result = await Permissions.RequestAsync<Permissions.NearbyWifiDevices>();

                if (result != PermissionStatus.Granted)
                {
                    return false;
                }
            }


            return true;
        }
    }
}
