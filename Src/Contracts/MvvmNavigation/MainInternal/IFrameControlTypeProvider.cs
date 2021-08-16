using System;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Internal
{
    internal interface IFrameControlTypeProvider
    {
        Type GetFrameControlType(Type navigationManagerType);
    }
}