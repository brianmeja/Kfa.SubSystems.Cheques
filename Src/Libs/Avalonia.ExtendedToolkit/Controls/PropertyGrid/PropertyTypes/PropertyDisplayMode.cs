﻿namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.PropertyTypes
{
    //
    // ported from https://github.com/DenisVuyka/WPG
    //

    /// <summary>
    /// Defines modes for property visualization.
    /// </summary>
    public enum PropertyDisplayMode
    {
        /// <summary>
        /// Show all properties.
        /// </summary>
        All,

        /// <summary>
        /// Show dependency properties only.
        /// </summary>
        Dependency,

        /// <summary>
        /// Show native CLR properties only.
        /// </summary>
        Native
    }
}
