namespace SFAR.Services
{
    public static class LifecycleEventManager
    {
        public static event Action<string>? LifecycleEvent;

        public static void OnLifeCycleEvent(string name)
        {
            LifecycleEvent?.Invoke(name);
        }
    }
}
