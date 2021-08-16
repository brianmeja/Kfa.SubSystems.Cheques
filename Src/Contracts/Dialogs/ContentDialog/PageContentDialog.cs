using Aura.UI.Controls.Primitives;
using Aura.UI.UIExtensions;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Kfa.SubSystems.Cheques.Contracts;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using System;

namespace Aura.UI.Controls
{
    public partial class PageContentDialog : ContentDialogBase
    {
        public bool OkButtonIsVisible { get; set; } = true;
        private Button OkButton;
        private LongProcessNotification longProcessNotification;
        public LongProcessNotification LongProcessNotification { get; private set; }

        public PageContentDialog()
        {
            Declarations.EventAggregator
               .GetEvent<ProcessingEvent>()
               .Subscribe(tt =>
               {
                   Functions.RunOnMain(() =>
                   {
                       try
                       {
                           if (!tt.IsActive) HideProgressInformation();
                           else
                               ShowProgressInformation(tt.SubMessage, tt.SubMessageOption, tt.Message);
                       }
                       catch { }
                   });
               });
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            try
            {
                base.OnApplyTemplate(e);

                OkButton = this.GetControl<Button>(e, "PART_OkButton");
                var bs = this.GetControl<Grid>(e, "PART_GridContainer");

                if (OkButtonIsVisible)
                {
                    OkButton.IsVisible = OkButtonIsVisible;
                    OkButton.Click += (s, e) =>
                    {
                        var x = new RoutedEventArgs(OkButtonClickEvent);
                        RaiseEvent(x);
                        x.Handled = true;
                    };
                }
                else
                {
                    OkButton.IsVisible = OkButtonIsVisible;
                }
            }
            catch (System.Exception ex)
            {
                Notifier.NotifyError(ex);
            }
        }

        protected void HideProgressInformation()
        {
            try
            {
                LongProcessNotification = new LongProcessNotification
                {
                    IsActive = false,
                    Message = null,
                    SubMessage = null,
                    SubMessageOption = null,
                    Title = null
                };
            }
            catch (Exception ex)
            {
                Notifier.NotifyError(ex);
            }
        }

        protected void ShowProgressInformation(string subMessage, string subMessageOption, string message = "Please Wait")
        {
            try
            {
                LongProcessNotification = new LongProcessNotification
                {
                    IsActive = true,
                    Message = message,
                    SubMessage = subMessage,
                    SubMessageOption = subMessageOption,
                    Title = "Processing"
                };
            }
            catch (Exception ex)
            {
                Notifier.NotifyError(ex);
            }
        }
    }
}