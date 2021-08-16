using JetBrains.Annotations;
using System;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation
{
    public interface IViewInteractionStrategy
    {
        object GetContent(object control);

        void SetContent([NotNull] object control, object content);

        object GetDataContext([NotNull] object control);

        void SetDataContext([NotNull] object control, object dataContext);

        void InvokeInUIThread([NotNull] object control, [NotNull] Action action);
    }
}