using System;
using System.Linq;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Internal
{
    internal class NavigationManagerTypeProvider : INavigationManagerTypeProvider
    {
        private static readonly string[] AssemblyNames =
        {
            "MvvmNavigation.Avalonia",
            "MvvmNavigation.Wpf"
        };

        public Type GetNavigationManagerType()
        {
            return AssemblyNames.Select(assemblyName => $"Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.NavigationManager, {assemblyName}")
                                .Select(Type.GetType)
                                .FirstOrDefault(x => x != null);
        }
    }
}