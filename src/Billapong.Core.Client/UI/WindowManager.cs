namespace Billapong.Core.Client.UI
{
    using System;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// WPF window manager to manage opening and closing of windows.
    /// </summary>
    public class WindowManager
    {
        /// <summary>
        /// Opens the specified view model with the corresponding view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Open(ViewModelBase viewModel)
        {
            var view = this.GetWindow(viewModel.GetType());
            view.DataContext = viewModel;
            view.Closing += (sender, args) => viewModel.CloseCallback();
            view.Show();
            view.Focus();
        }

        /// <summary>
        /// Closes the window with the corresponding viewmodel.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Close(ViewModelBase viewModel)
        {
            var view = this.FindWindow(viewModel);
            if (view != null)
            {
                view.Close();
            }
        }

        /// <summary>
        /// Gets an instance of the view to the corresponding viewmodel.
        /// It search for the view with the same name -> for "MyViewModel" it returns "MyView".
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns>Window instance of the view</returns>
        private Window GetWindow(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "View");
            var windowType = viewModelType.Assembly.GetType(viewName);
            if (windowType == null) return null;

            return Activator.CreateInstance(windowType) as Window;
        }

        /// <summary>
        /// Finds an open window based on a viewmodel.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The window instance or null</returns>
        private Window FindWindow(ViewModelBase viewModel)
        {
            return Application.Current.Windows.Cast<Window>().FirstOrDefault(window => ReferenceEquals(window.DataContext, viewModel));
        }
    }
}
