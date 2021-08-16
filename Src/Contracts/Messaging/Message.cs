namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    using ReactiveUI;
    using System;

    /// <summary>
    /// Defines the <see cref="NotificationMessage" />.
    /// </summary>
    public class NotificationMessage : ViewModelBase
    {
        /// <summary>
        /// Defines the isActive.
        /// </summary>
        private bool isActive = true;

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        public bool IsActive { get => isActive; set => this.RaiseAndSetIfChanged(ref isActive, value); }

        /// <summary>
        /// Defines the logIt.
        /// </summary>
        private bool logIt = false;

        /// <summary>
        /// Gets or sets a value indicating whether LogIt.
        /// </summary>
        public bool LogIt { get => logIt; set => this.RaiseAndSetIfChanged(ref logIt, value); }

        /// <summary>
        /// Defines the messageTypes.
        /// </summary>
        private MessageTypes messageTypes;

        /// <summary>
        /// Gets or sets the MessageType.
        /// </summary>
        public MessageTypes MessageType { get => messageTypes; set => this.RaiseAndSetIfChanged(ref messageTypes, value); }

        /// <summary>
        /// Defines the timeSpan.
        /// </summary>
        private TimeSpan timeSpan;

        /// <summary>
        /// Gets or sets the TimeSpan.
        /// </summary>
        public TimeSpan TimeSpan { get => timeSpan; set => this.RaiseAndSetIfChanged(ref timeSpan, value); }

        /// <summary>
        /// Defines the sender.
        /// </summary>
        private object sender;

        /// <summary>
        /// Gets or sets the Sender.
        /// </summary>
        public object Sender { get => sender; set => this.RaiseAndSetIfChanged(ref sender, value); }

        /// <summary>
        /// Defines the title.
        /// </summary>
        private string title;

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }

        /// <summary>
        /// Defines the message.
        /// </summary>
        private string message;

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get => message; set => this.RaiseAndSetIfChanged(ref message, value); }

        /// <summary>
        /// Defines the narration.
        /// </summary>
        private string narration;

        /// <summary>
        /// Gets or sets the Narration.
        /// </summary>
        public string Narration { get => narration; set => this.RaiseAndSetIfChanged(ref narration, value); }

        /// <summary>
        /// Defines the subMessage.
        /// </summary>
        private string subMessage;

        /// <summary>
        /// Gets or sets the SubMessage.
        /// </summary>
        public string SubMessage { get => subMessage; set => this.RaiseAndSetIfChanged(ref subMessage, value); }
    }
}