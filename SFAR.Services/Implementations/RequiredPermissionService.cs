using SFAR.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SFAR.Services.Implementations
{
    internal class RequiredPermissionService : IRequiredPermissionService
    {
        public Task<bool> HasAllPermissions()
        {

            TaskCompletionSource<bool> taskResult = new TaskCompletionSource<bool>();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    var result = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                    if (result != PermissionStatus.Granted)
                    {
                        taskResult.SetResult(false);
                        return;
                    }
                }

                status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

                if (status != PermissionStatus.Granted)
                {
                    var result = await Permissions.RequestAsync<Permissions.Bluetooth>();

                    if (result != PermissionStatus.Granted)
                    {
                        taskResult.SetResult(false);
                        return;
                    }
                }


                status = await Permissions.CheckStatusAsync<Permissions.NearbyWifiDevices>();

                if (status != PermissionStatus.Granted)
                {
                    var result = await Permissions.RequestAsync<Permissions.NearbyWifiDevices>();

                    if (result != PermissionStatus.Granted)
                    {
                        taskResult.SetResult(false);
                        return;
                    }
                }

                if (DeviceInfo.Version.Major >= 12)
                {
                    status = await Permissions.CheckStatusAsync<MyBluetoothPermission>();

                    if (status != PermissionStatus.Granted)
                    {
                        var result = await Permissions.RequestAsync<MyBluetoothPermission>();

                        if (result != PermissionStatus.Granted)
                        {
                            taskResult.SetResult(false);
                            return;
                        }
                    }
                }

              


                taskResult.SetResult(true);
            });
          
            return taskResult.Task; 
        }
    }
}
