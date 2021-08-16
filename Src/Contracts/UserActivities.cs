using System;

namespace Kfa.SubSystems
{
    [Flags]
    public enum UserActivities : short
    {
        /// <summary>
        /// Defines the None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Defines the Updated.
        /// </summary>
        Update = 1,

        /// <summary>
        /// Defines the Inserted.
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Defines the Delete.
        /// </summary>
        Delete = 4,

        /// <summary>
        /// Defines the View.
        /// </summary>
        View = 8,

        /// <summary>
        /// Defines the Search.
        /// </summary>
        Search = 16,

        /// <summary>
        /// Defines the Print.
        /// </summary>
        Print = 32,

        /// <summary>
        /// Defines the Export.
        /// </summary>
        Export = 64,

        /// <summary>
        /// Defines the Approve.
        /// </summary>
        Approve = 128,

        /// <summary>
        /// Defines the Comment.
        /// </summary>
        Comment = 256,

        /// <summary>
        /// Defines the FullAccess.
        /// </summary>
        FullAccess = FullCrud | Export | Print,

        /// <summary>
        /// Defines the FullCrud.
        /// </summary>
        FullCrud = Update | Insert | Delete | ReadOnly,

        /// <summary>
        /// Defines the ReadOnly.
        /// </summary>
        ReadOnly = View | Search
    }
}