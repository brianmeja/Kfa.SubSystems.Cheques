namespace Avalonia.ExtendedToolkit.Controls
{
    // This source file is adapted from the Windows Presentation Foundation project.
    // (https://github.com/dotnet/wpf/)

    ///<summary>
    /// The IAddChild interface is used for parsing objects that
    /// allow objects or text underneath their tags in markup that
    /// do not map directly to a property.
    ///</summary>
    public interface IAddChild
    {
        ///<summary>
        /// Called to Add the object as a Child.
        ///</summary>
        ///<param name="value">
        /// Object to add as a child
        ///</param>
        void AddChild(object value);

        ///<summary>
        /// Called when text appears under the tag in markup
        ///</summary>
        ///<param name="text">
        /// Text to Add to the Object
        ///</param>
        void AddText(string text);
    }
}
