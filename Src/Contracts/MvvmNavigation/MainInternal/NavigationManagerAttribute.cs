using System;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Internal
{
    internal class NavigationManagerAttribute : Attribute
    {
        internal NavigationManagerAttribute(Type frameControlType)
        {
            FrameControlType = frameControlType;
        }

        internal Type FrameControlType { get; }
    }
}