using JetBrains.Annotations;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Abstractions
{
    public static class NavigationManagerExtensions
    {
        private static Stack<string> backwards = new Stack<string>();
        private static Stack<string> forwards = new Stack<string>();
        private static string lastNavigationKey;
        private static bool isMovingForward, isMovingBackward;

        public static void Navigate(this INavigationManager navigationManager, [NotNull] string navigationKey)
        {
            navigationManager.Navigate(navigationKey, null);

            if (isMovingForward)
            {
                backwards.Push(lastNavigationKey);
            }
            else if (isMovingBackward)
            {
                forwards.Push(lastNavigationKey);
            }
            else
            {
                forwards.Clear();

                if (!string.IsNullOrWhiteSpace(lastNavigationKey) && lastNavigationKey != navigationKey)
                    backwards.Push(lastNavigationKey);
            }

            lastNavigationKey = navigationKey;

            Declarations.EventAggregator
                .GetEvent<SystemMessageEvent>()
                .Publish(new SystemMessageValue { Value = navigationManager, Message = SystemMessage.Navigated });
        }

        public static void NavigateForward(this INavigationManager navigationManager)
        {
            try
            {
                isMovingForward = true;

                if (forwards.Any())
                    navigationManager.Navigate(forwards.Pop());
            }
            finally
            {
                isMovingForward = false;
            }
        }

        public static void NavigateBack(this INavigationManager navigationManager)
        {
            try
            {
                isMovingBackward = true;

                if (backwards.Any())
                    navigationManager.Navigate(backwards.Pop());
            }
            finally
            {
                isMovingBackward = false;
            }
        }

        public static bool CanNavigateBack(this INavigationManager navigationManager)
        {
            return backwards.Any();
        }

        public static bool CanNavigateForward(this INavigationManager navigationManager)
        {
            return forwards.Any();
        }
    }
}