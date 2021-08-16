namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    public struct LongProcessNotification
    {
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string SubMessage { get; set; }
        public string SubMessageOption { get; set; }
        public bool StartNextProcess { get; set; }
    }
}