using Prism.Events;

namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    public class SystemMessageEvent : PubSubEvent<SystemMessageValue>
    {
        public object Data { get; set; }
    }
}