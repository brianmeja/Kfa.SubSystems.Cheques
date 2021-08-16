namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    using System;

    /// <summary>
    /// Defines the <see cref="Notifier" />.
    /// </summary>
    public static class Notifier
    {
        /// <summary>
        /// The LongProcess.
        /// </summary>
        /// <param name="subMessage">The subMessage<see cref="string"/>.</param>
        /// <param name="subMessageOption">The subMessageOption<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void LongProcess(string subMessage, string subMessageOption, string message = "Please Wait")
        {
            try
            {
                Declarations.EventAggregator
                    .GetEvent<ProcessingEvent>()
                    .Publish(new LongProcessNotification
                    {
                        Message = message ?? "Please Wait...",
                        SubMessageOption = subMessageOption,
                        SubMessage = subMessage,
                        IsActive = true
                    });
            }
            catch { }
        }

        /// <summary>
        /// The NotifyMessage.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="subMessage">The subMessage<see cref="string"/>.</param>
        public static void NotifyMessage(string message, string title, string subMessage = null)
        {
            try
            {
                Declarations.EventAggregator
                    .GetEvent<MessageEvent>()
                    .Publish(
                     new NotificationMessage
                     {
                         Message = message,
                         MessageType = MessageTypes.Information,
                         Title = title,
                         SubMessage = subMessage
                     });
            }
            catch { }
        }

        /// <summary>
        /// Defines the previousMessage.
        /// </summary>
        internal static string previousMessage;

        /// <summary>
        /// The NotifyError.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void NotifyError(string message, string title, Exception ex = null)
        {
            try
            {
                ex = ex.InnerError();
                if (ex == null || previousMessage == ex.ToString()) return;

                previousMessage = ex.ToString();
                //var bb = new MySqlSink

                if (ex == null)
                    ex = new Exception(message);
                Declarations.DbLogger?.Error(ex, message);

                var subMsg = message;

                if (ex != null && !ex.Message.Contains(ex.Message))
                    subMsg = $"{message}\r\n{ex.Message}";

                Declarations.EventAggregator
                    .GetEvent<MessageEvent>()
                    .Publish(
                     new NotificationMessage
                     {
                         Message = message,
                         MessageType = MessageTypes.Error,
                         Title = title,
                         SubMessage = $"Error: {subMsg}",
                         Narration = ex.Message,
                         Sender = null
                     });
            }
            catch { }
        }

        /// <summary>
        /// The NotifyError.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void NotifyError(Exception ex)
        {
            try
            {
                ex = ex.InnerError();
                if (ex == null || previousMessage == ex.ToString()) return;

                Declarations.DbLogger?.Error(ex, ex.Message);
                previousMessage = ex.ToString();

                var subMsg = ex.Message;

                Declarations.EventAggregator
                    .GetEvent<MessageEvent>()
                    .Publish(
                     new NotificationMessage
                     {
                         Message = subMsg,
                         MessageType = MessageTypes.Error,
                         Title = "Error",
                         SubMessage = $"Error: {subMsg}",
                         Narration = ex.Message,
                         Sender = null
                     });
            }
            catch { }
        }

        /// <summary>
        /// The LongProcessDeNotify.
        /// </summary>
        public static void LongProcessDeNotify()
        {
            try
            {
                Declarations.EventAggregator
                    .GetEvent<ProcessingEvent>()
                    .Publish(new LongProcessNotification
                    {
                        IsActive = false
                    });
            }
            catch { }
        }

        /// <summary>
        /// The LongProcessNotify.
        /// </summary>
        /// <param name="longProcessNotification">The longProcessNotification<see cref="LongProcessNotification"/>.</param>
        public static void LongProcessNotify(LongProcessNotification longProcessNotification)
        {
            try
            {
                Declarations.EventAggregator
                    .GetEvent<ProcessingEvent>()
                    .Publish(longProcessNotification);
            }
            catch { }
        }
    }
}