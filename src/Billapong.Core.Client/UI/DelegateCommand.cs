namespace Billapong.Core.Client.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// An <see cref="ICommand"/> whose delegates do not take any parameters for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    /// <seealso cref="DelegateCommandBase"/>
    public class DelegateCommand : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">Gets thrown when the executeMethod or canExecuteMethod is null</exception>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(parameter => executeMethod(), parameter => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        private DelegateCommand(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">Gets thrown if executeMethod or canExcuteMethod is null</exception>
        private DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base(parameter => executeMethod(), parameter => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        /// <summary>
        /// Returns a delegate command with the provided async method
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <returns>The delegate command</returns>
        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod)
        {
            return new DelegateCommand(executeMethod);
        }

        /// <summary>
        /// Froms the asynchronous handler.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <returns>The delegate command</returns>
        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>The task</returns>
        public async Task Execute()
        {
            await this.Execute(null);
        }

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>The execution check result</returns>
        public bool CanExecute()
        {
            return this.CanExecute(null);
        }
    }
}
