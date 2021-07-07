﻿using System;

namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// WindowCommandsOverlayBehavior type
    /// </summary>
    [Flags]
    public enum WindowCommandsOverlayBehavior
    {
        /// <summary>
        /// Doesn't overlay a hidden TitleBar.
        /// </summary>
        Never = 0,

        /// <summary>
        /// Overlays a hidden TitleBar.
        /// </summary>
        HiddenTitleBar = 1 << 0
    }
}
