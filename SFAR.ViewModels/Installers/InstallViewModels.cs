namespace SFAR.ViewModels.Installers
{
    public static class InstallViewModels
    {
        public static void InstallSFARViewModels(this IServiceCollection services)
        {
            var assembly = typeof(InstallViewModels).Assembly;

            var viewModelBaseType = typeof(ViewModelBase);

            var types = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && viewModelBaseType.IsAssignableFrom(x));

            foreach (var type in types)
            { 
                services.AddTransient(type);
            }
        }
    }
}
