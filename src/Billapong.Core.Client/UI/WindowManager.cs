namespace Billapong.Core.Client.UI
{
    using System;
    using System.Windows;

    public class WindowManager
    {
        public void Open(ViewModelBase viewModel)
        {
            var view = this.GetWindow(viewModel.GetType());
            view.DataContext = viewModel;
            view.Show();
        }

        private Window GetWindow(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "View");
            var windowType = viewModelType.Assembly.GetType(viewName);
            if (windowType == null) return null;

            return Activator.CreateInstance(windowType) as Window;
        }
    }
}
