namespace Billapong.Core.Client.UI
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a generic approach of invoking commands
    /// </summary>
    public class DelegateCommand<T> : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, parameter => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">executeMethod</exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(parameter => executeMethod((T)parameter), parameter => canExecuteMethod((T)parameter))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        /// <summary>
        /// Froms the asynchronous handler.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <returns></returns>
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new DelegateCommand<T>(executeMethod);
        }

        /// <summary>
        /// Froms the asynchronous handler.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <returns></returns>
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new DelegateCommand<T>(executeMethod, canExecuteMethod);
        }

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The execution check result</returns>
        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public async Task Execute(T parameter)
        {
            await base.Execute(parameter);
        }


        /// <summary>
        /// Prevents a default instance of the <see cref="DelegateCommand{T}"/> class from being created.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        private DelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, parameter => true)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DelegateCommand{T}"/> class from being created.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">executeMethod</exception>
        private DelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base(parameter => executeMethod((T)parameter), parameter => canExecuteMethod((T)parameter))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }
    }
}
