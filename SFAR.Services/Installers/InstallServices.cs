namespace SFAR.Services.Installers
{
    public static class InstallServices
    {
        public static void InstallSFARServices(this IServiceCollection services)
        {
            var assembly = typeof(InstallServices).Assembly;

            var iServiceType = typeof(IService);

            var types = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && iServiceType.IsAssignableFrom(x));

            foreach(var type in types)
            {
                var serviceInterface = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && iServiceType.IsAssignableFrom(x) && iServiceType!=x);
                
                if (serviceInterface == null)
                {
                    continue;
                }

                services.AddSingleton(serviceInterface, type);
            }
        }
    }
}
