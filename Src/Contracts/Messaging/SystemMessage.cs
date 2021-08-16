namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    public enum SystemMessage
    {
        None = 0,
        ShutdownWithoutNotice = 1,
        ShutdownWithNotice = 2,
        SystemLock = 8,
        Resize = 9,
        Minimize = 3,
        Maximize = 4,
        Logout = 6,
        LoggedIn = 7,
        ShutdownDownStarted = 5,
        PasswordChanged = 6,
        AddedMenuItem = 10,
        Navigated = 11
    }

    public struct SystemMessageValue
    {
        public SystemMessage Message { get; set; }
        public object Value { get; set; }
    }
}