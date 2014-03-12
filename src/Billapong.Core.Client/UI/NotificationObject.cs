namespace Billapong.Core.Client.UI
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Enables property notification on objects
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// The values
        /// </summary>
        private Dictionary<string, object> values = new Dictionary<string, object>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the property</returns>
        protected T GetValue<T>([CallerMemberName]string propertyName = "")
        {
            object value = null;
            if (!this.values.TryGetValue(propertyName, out value))
            {
                value = default(T);
            }
            return (T)value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetValue<T>(T value, [CallerMemberName]string propertyName = "")
        {
            var oldValue = this.GetValue<T>(propertyName);
            var changed = !object.Equals(oldValue, value);
            if (changed)
            {
                this.values[propertyName] = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Dirties all values.
        /// </summary>
        protected void DirtyAllValues()
        {
            foreach (var keyValuePair in this.values)
            {
                this.OnPropertyChanged(keyValuePair.Key);
            }
        }

        /// <summary>
        /// Called when a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
