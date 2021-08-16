using JetBrains.Annotations;
using System;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Abstractions
{
    public interface INavigationManager
    {
        bool CanNavigate(string navigationKey);

        void Navigate([NotNull] string navigationKey, object arg);

        event EventHandler<NavigationEventArgs> Navigated;
    }
}