namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// FlyoutTheme types
    /// </summary>
    public enum FlyoutTheme
    {
        /// <summary>
        /// Adapts the Flyout's theme to the theme of its host window.
        /// </summary>
        Adapt,

        /// <summary>
        /// Adapts the Flyout's theme to the theme of its host window, but inverted.
        /// This theme can only be applied if the host window's theme abides the "Dark" and "Light" affix convention.
        /// (see <see cref="ThemeManager.GetInverseTheme"/> for more infos.
        /// </summary>
        Inverse,

        /// <summary>
        /// The dark theme. This is the default theme.
        /// </summary>
        Dark,

        /// <summary>
        /// The light theme.
        /// </summary>
        Light,

        /// <summary>
        /// The flyouts theme will match the host window's accent color.
        /// </summary>
        Accent
    }
}
