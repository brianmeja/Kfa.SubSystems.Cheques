namespace Kfa.SubSystems.Cheques.Contracts.Messaging
{
    /// <summary>
    /// Defines the MessageTypes.
    /// </summary>
    public enum MessageTypes
    {
        /// <summary>
        /// Defines the Information.
        /// </summary>
        Information = 0,

        /// <summary>
        /// Defines the Warning.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Defines the Error.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Defines the Question.
        /// </summary>
        Question = 3,

        /// <summary>
        /// Defines the ValidationError.
        /// </summary>
        ValidationError = 4,

        /// <summary>
        /// Defines the Success.
        /// </summary>
        Success = 5
    }
}