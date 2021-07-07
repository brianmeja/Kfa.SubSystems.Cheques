﻿using Avalonia.ExtendedToolkit.Controls;
using Avalonia.ExtendedToolkit.TriggerExtensions;
using Avalonia.VisualTree;

namespace Avalonia.ExtendedToolkit.Actions
{
    //ported from https://github.com/MahApps/MahApps.Metro

    /// <summary>
    /// command action which closes
    /// the <see cref="Flyout"/>
    /// </summary>
    public class CloseFlyoutAction : CommandTriggerAction
    {
        private Flyout associatedFlyout;

        private Flyout AssociatedFlyout => this.associatedFlyout ?? (this.associatedFlyout = this.AssociatedObject.GetVisualParent<Flyout>());

        /// <summary>
        /// invokes the close fyout command if associated object is a flyout
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null || (this.AssociatedObject != null && !this.AssociatedObject.IsEnabled))
            {
                return;
            }

            var command = this.Command;
            if (command != null)
            {
                var commandParameter = this.GetCommandParameter();
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
            else
            {
                this.AssociatedFlyout?.SetValue(Flyout.IsOpenProperty, false);
            }
        }

        /// <summary>
        /// if command parameter is null use the associated object as parameter
        /// </summary>
        /// <returns></returns>
        protected override object GetCommandParameter()
        {
            return this.CommandParameter ?? this.AssociatedFlyout;
        }
    }
}
