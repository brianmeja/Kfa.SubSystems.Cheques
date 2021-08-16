namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    using Avalonia.Controls;
    using Avalonia.Input;

    public class KeyUpEvent : Prism.Events.PubSubEvent<(KeyEventArgs Args, IControl Page)> { }
}