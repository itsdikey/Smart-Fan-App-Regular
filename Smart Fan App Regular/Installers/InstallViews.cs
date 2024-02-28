using SFAR.App.Views;

namespace SFAR.Installers
{
    public static class InstallViews
    {
        public static void InstallSFARViews(this IServiceCollection services)
        {
            var assembly = typeof(InstallViews).Assembly;

            var viewBaseType = typeof(IViewBase);

            var types = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && viewBaseType.IsAssignableFrom(x));

            foreach(var type in types)
            {
                var viewInterface = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && viewBaseType==x);
                
                if (viewInterface == null)
                {
                    continue;
                }

                services.AddTransient(type);
            }
        }
    }
}
