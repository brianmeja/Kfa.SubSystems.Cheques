using Usb.Events;

namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    public record UsbMountObj(string Path, bool IsMounting);

    public class UsbDeviceMountEvent : Prism.Events.PubSubEvent<UsbMountObj> { }

    public record UsbUpdateObj(UsbDevice Device, bool IsAdding);

    public class UsbDeviceUpdateEvent : Prism.Events.PubSubEvent<UsbUpdateObj> { }
}