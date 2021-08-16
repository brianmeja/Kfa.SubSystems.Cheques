using Prism.Events;

namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    public class DatabaseUpdateEvent : PubSubEvent<(object, UserActivities)>
    {
    }
}