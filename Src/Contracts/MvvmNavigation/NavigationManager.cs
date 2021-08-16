using Avalonia.Controls;
using JetBrains.Annotations;
using Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Internal;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation
{
    [NavigationManager(typeof(ContentControl))]
    public class NavigationManager : NavigationManagerBase
    {
        public NavigationManager([NotNull] ContentControl frameControl)
            : base(frameControl, new ViewInteractionStrategy())
        {
        }

        public NavigationManager([NotNull] ContentControl frameControl, [NotNull] IDataStorage dataStorage)
            : base(frameControl, new ViewInteractionStrategy(), dataStorage)
        {
        }
    }
}