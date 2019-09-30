namespace BeSafe.Core.Interfaces
{
    public interface IControlForegroundService
    {
        void CreateService(bool? savedInstanceState);
        bool StartServices();
        bool StopServices();
    }
}