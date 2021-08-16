using System.Collections.Generic;

namespace Kfa.SubSystems.Cheques.Contracts.Classes
{
    /// <summary>
    ///     For the Value monitor <b>ValueChanged</b> event.
    /// </summary>
    /// <typeparam name="TValueType"> The type of the value in ValueMonitor class </typeparam>
    /// <param name="oldValue"> The old value, that has been overwritten </param>
    /// <param name="newValue"> The new value, that has been set </param>
    public delegate void ValueChangedDelegate<TValueType>(TValueType oldValue, TValueType newValue);

    public interface IValueMonitor<TValueType>
    {
        TValueType Value { get; }

        event ValueChangedDelegate<TValueType> ValueChanged;
    }

    /// <summary>
    ///     Monitors the value of any variable.
    ///     If the value changes by means of <b>set</b> function - the event is rised.
    ///     If set function is called with the same value - then we still quiet.
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public class ValueMonitor<TValueType> : IValueMonitor<TValueType>
    {
        private readonly IEqualityComparer<TValueType> comparer;
        private TValueType aValue;

        public ValueMonitor(TValueType initialValue) => aValue = initialValue;

        /// <summary>
        ///     Creates an instance of ValueMonitor with the given comparer.
        /// </summary>
        /// <param name="initialValue">  The initial value of the variable.  </param>
        /// <param name="comparator"> The comparer object </param>
        public ValueMonitor(TValueType initialValue, IEqualityComparer<TValueType> comparator)
        {
            aValue = initialValue;
            comparer = comparator;
        }

        /// <summary>
        ///     Gets or sets the value of the variable.
        /// </summary>
        public TValueType Value
        {
            get { return aValue; }
            set
            {
                var areEqual = comparer == null ? aValue.Equals(value) : comparer.Equals(aValue, value);

                if (areEqual) return;
                var oldValue = aValue; // remember previous for the event rising
                aValue = value;

                if (ValueChanged != null)
                    ValueChanged(oldValue, aValue);
            }
        }

        /// <summary>
        ///     Raised, whenever the value REALLY changed
        /// </summary>
        public event ValueChangedDelegate<TValueType> ValueChanged;

        /// <summary>
        ///     "Quietly" sets the connection status.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetQuietly(TValueType newValue) => aValue = newValue;
    }
}