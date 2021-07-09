using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// itemscontrol with <see cref="IndexListHeaderItem"/>
    /// </summary>
    public class IndexList : ItemsControl
    {
        /// <summary>
        /// Gets or sets ShowEmptyItems.
        /// </summary>
        public bool ShowEmptyItems
        {
            get { return (bool)GetValue(ShowEmptyItemsProperty); }
            set { SetValue(ShowEmptyItemsProperty, value); }
        }

        /// <summary>
        /// Defines the ShowEmptyItems property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowEmptyItemsProperty =
        AvaloniaProperty.Register<IndexList, bool>(nameof(ShowEmptyItems), defaultValue: true);

        /// <summary>
        /// Occurs when the control's selection changes.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged
        {
            add => AddHandler(SelectingItemsControl.SelectionChangedEvent, value);
            remove => RemoveHandler(SelectingItemsControl.SelectionChangedEvent, value);
        }

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly DirectProperty<IndexList, object> SelectedItemProperty =
            SelectingItemsControl.SelectedItemProperty.AddOwner<IndexList>(
                o => o.SelectedItem,
                (o, v) => o.SelectedItem = v);

        private object _selectedItem;

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
            }
        }

        /// <summary>
        /// Defines the <see cref="AutoScrollToSelectedItem"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> AutoScrollToSelectedItemProperty =
            SelectingItemsControl.AutoScrollToSelectedItemProperty.AddOwner<IndexList>();

        /// <summary>
        /// Gets or sets a value indicating whether to automatically scroll to newly selected items.
        /// </summary>
        public bool AutoScrollToSelectedItem
        {
            get => GetValue(AutoScrollToSelectedItemProperty);
            set => SetValue(AutoScrollToSelectedItemProperty, value);
        }

        /// <summary>
        /// generator for the <see cref="IndexListHeaderItem"/> subitems
        /// </summary>
        /// <returns></returns>
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var result = new IndexListItemGenerator(
                                    this,
                                    IndexListHeaderItem.HeaderProperty,
                                    IndexListHeaderItem.ItemTemplateProperty,
                                    IndexListHeaderItem.ItemsProperty
                                    );
            result.Materialized += ContainerMaterialized;

            return result;
        }

        /// <summary>
        /// if item is found and <see cref="AutoScrollToSelectedItem"/> is
        /// enabled, the container is BringIntoView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerMaterialized(object sender, ItemContainerEventArgs e)
        {
            var selectedItem = SelectedItem;

            if (selectedItem == null)
            {
                return;
            }

            foreach (var container in e.Containers)
            {
                if (container.Item == selectedItem)
                {
                    ((IndexListItem)container.ContainerControl).IsSelected = true;

                    if (AutoScrollToSelectedItem)
                    {
                        Dispatcher.UIThread.Post(container.ContainerControl.BringIntoView);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Scrolls the specified item into view.
        /// </summary>
        /// <param name="item">The item.</param>
        public void ScrollIntoView(object item)
        {
            Presenter?.ScrollIntoView(Array.IndexOf(Items.OfType<object>().ToArray(), item));
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            var prop = e.GetCurrentPoint(this).Properties;

            if (prop.IsLeftButtonPressed || prop.IsRightButtonPressed)
            {
                e.Handled = UpdateSelectionFromEventSource(
                    e.Source,
                    true);
            }
        }

        /// <summary>
        /// gets the container from the event source
        /// and calls <see cref="UpdateSelectionFromContainer"/>
        /// </summary>
        /// <param name="eventSource"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        protected bool UpdateSelectionFromEventSource(
            IInteractive eventSource,
            bool select = true
            )
        {
            var container = GetContainerFromEventSource(eventSource);

            if (container != null)
            {
                UpdateSelectionFromContainer(container, select);
                return true;
            }

            return false;
        }

        /// <summary>
        /// tries to find the <see cref="IndexListItem"/>
        /// </summary>
        /// <param name="eventSource"></param>
        /// <returns></returns>
        protected IndexListItem GetContainerFromEventSource(IInteractive eventSource)
        {
            var item = ((IVisual)eventSource).GetSelfAndVisualAncestors()
                .OfType<IndexListItem>()
                .FirstOrDefault();
            return item;
        }

        /// <summary>
        /// deselects the old item and
        /// selects the new item and raises
        /// <see cref="SelectionChangedEventArgs"/>
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isSelect"></param>
        protected void UpdateSelectionFromContainer(
            IControl container,
            bool isSelect = true
            )
        {
            if (container is IndexListItem item)
            {
                List<object> oldItems = new List<object>();
                if (SelectedItem is IndexListItem lastItem)
                {
                    lastItem.IsSelected = false;
                    oldItems.Add(lastItem);
                }

                if (oldItems.Contains(item) == false)
                {
                    item.IsSelected = isSelect;
                    SelectedItem = item;
                }
                else
                {
                    //remove selection if the item was already selected
                    SelectedItem = null;
                }

                var changed = new SelectionChangedEventArgs(
                    SelectingItemsControl.SelectionChangedEvent,
                    new List<object> { SelectedItem },
                    oldItems);
                RaiseEvent(changed);
            }
        }
    }
}
