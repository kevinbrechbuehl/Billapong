namespace Billapong.Core.Client.UI
{
    using System;
    using System.Linq;
    using System.Windows;

    public class WindowManager
    {
        public void Open(ViewModelBase viewModel)
        {
            var view = this.GetWindow(viewModel.GetType());
            view.DataContext = viewModel;
            view.Closing += (sender, args) => viewModel.CloseCallback();
            view.Show();
            view.Focus();
        }

        private Window GetWindow(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "View");
            var windowType = viewModelType.Assembly.GetType(viewName);
            if (windowType == null) return null;

            return Activator.CreateInstance(windowType) as Window;
        }

        public void Close(ViewModelBase viewModel)
        {
            var view = this.FindWindow(viewModel);
            if (view != null)
            {
                view.Close();
            }
        }

        private Window FindWindow(ViewModelBase viewModel)
        {
            return Application.Current.Windows.Cast<Window>().FirstOrDefault(window => ReferenceEquals(window.DataContext, viewModel));
        }
    }
}
