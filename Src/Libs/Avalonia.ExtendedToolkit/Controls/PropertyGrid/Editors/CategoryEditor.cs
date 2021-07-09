﻿using System;
using System.Diagnostics;

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Editors
{
    //
    // ported from https://github.com/DenisVuyka/WPG
    //

    /// <summary>
    /// Specifies the default category editor.
    /// </summary>
    [DebuggerDisplay("[CategoryEditor] CategoryName: {CategoryName} DeclaringType: {DeclaringType}")]
    public class CategoryEditor : Editor
    {
        /// <summary>
        /// style key for this control
        /// </summary>
        public new Type StyleKey => typeof(CategoryEditor);

        /// <summary>
        /// get/sets DeclaringType
        /// </summary>
        public Type DeclaringType
        {
            get { return (Type)GetValue(DeclaringTypeProperty); }
            set { SetValue(DeclaringTypeProperty, value); }
        }

        /// <summary>
        /// <see cref="DeclaringType"/>
        /// </summary>
        public static readonly StyledProperty<Type> DeclaringTypeProperty =
            AvaloniaProperty.Register<CategoryEditor, Type>(nameof(DeclaringType));

        /// <summary>
        /// get/sets CategoryName
        /// </summary>
        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        /// <summary>
        /// <see cref="CategoryName"/>
        /// </summary>
        public static readonly StyledProperty<string> CategoryNameProperty =
            AvaloniaProperty.Register<CategoryEditor, string>(nameof(CategoryName));

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryEditor"/> class.
        /// </summary>
        public CategoryEditor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryEditor"/> class.
        /// </summary>
        /// <param name="declaringType">Declaring type.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="inlineTemplate">The inline template.</param>
        public CategoryEditor(Type declaringType, string categoryName, object inlineTemplate)
        {
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (string.IsNullOrEmpty(categoryName))
                throw new ArgumentNullException(nameof(categoryName));

            DeclaringType = declaringType;
            CategoryName = categoryName;

            InlineTemplate= GetEditorTemplate(inlineTemplate);
        }
    }
}
