namespace Billapong.Core.Client.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Windows;
    using Billapong.Core.Client.Exceptions;
    using Billapong.Core.Client.Properties;
    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// Provides basic functionality to view models
    /// </summary>
    public abstract class ViewModelBase : NotificationObject, IDataErrorInfo
    {
        /// <summary>
        /// The validation messages of all properties with validation errors
        /// </summary>
        private readonly Dictionary<string, string> validationMessages = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            this.WindowManager = new WindowManager();
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current viewmodel has validation errors
        /// </summary>
        /// <value>
        ///   <c>true</c> if the viewmodel has validation errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasValidationErrors
        {
            get
            {
                return this.validationMessages.Count > 0;
            }
        }

        /// <summary>
        /// Gets the window manager.
        /// </summary>
        /// <value>
        /// The window manager.
        /// </value>
        protected WindowManager WindowManager { get; private set; }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The validation message</returns>
        public string this[string columnName]
        {
            get
            {
                string message;
                this.validationMessages.TryGetValue(columnName, out message);
                return message ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets called when the connected view closes
        /// </summary>
        public virtual void CloseCallback()
        {
            // to nothing by default
        }

        /// <summary>
        /// Shutdowns the application on an error and show a messagebox before.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void ShutdownApplication(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = Resources.UnexpectedError;
            }

            MessageBox.Show(message, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles server communication exceptions
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="withoutShutdown">if set to <c>true</c> the application does not shut down.</param>
        protected virtual async void HandleServerException(ServerUnavailableException ex, bool withoutShutdown = false)
        {
            await Tracer.Error("Server not available", ex);
            if (!withoutShutdown)
            {
                this.ShutdownApplication(Resources.ServerUnavailable);   
            }
        }

        /// <summary>
        /// Clears all validation messages.
        /// </summary>
        protected void ClearAllValidationMessages()
        {
            this.validationMessages.Clear();
            this.DirtyAllValues();
            this.OnPropertyChanged(GetPropertyName(() => this.HasValidationErrors));
        }

        /// <summary>
        /// Clears the validation message.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">The expression.</param>
        protected void ClearValidationMessage<T>(Expression<Func<T>> expression)
        {
            var propertyName = GetPropertyName(expression);
            this.validationMessages[propertyName] = null;
            this.OnPropertyChanged(propertyName);
            this.OnPropertyChanged(GetPropertyName(() => this.HasValidationErrors));
        }

        /// <summary>
        /// Sets the validation message.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        protected void SetValidationMessage<T>(Expression<Func<T>> expression, string value)
        {
            var propertyName = GetPropertyName(expression);
            this.validationMessages[propertyName] = value;
            this.OnPropertyChanged(propertyName);
            this.OnPropertyChanged(GetPropertyName(() => this.HasValidationErrors));
        }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The validation message</returns>
        protected string GetValidationMessage<T>(Expression<Func<T>> expression)
        {
            var propertyName = GetPropertyName(expression);
            string message;
            this.validationMessages.TryGetValue(propertyName, out message);
            return message ?? string.Empty;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The property name
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Gets thrown whether the method was unable to resolve the property name</exception>
        private static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression != null)
            {
                var memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    var propertyInfo = memberExpression.Member as System.Reflection.PropertyInfo;
                    if (propertyInfo != null)
                    {
                        return propertyInfo.Name;        
                    }
                }
            }

            throw new InvalidOperationException("Unable to resolve the property name");
        }
    }
}