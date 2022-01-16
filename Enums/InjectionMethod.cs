
namespace YaDi.Enums
{
    public enum InjectionMethod : ushort
    {
        Undef = 0,
        LoadLibrary = 1,
        SetWindowsHook = 2,
        ThreadHijack = 3,
        QueueUserAPC = 4,
        IATHook = 5,
    }
}
