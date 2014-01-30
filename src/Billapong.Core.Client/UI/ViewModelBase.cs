namespace Billapong.Core.Client.UI
{
    /// <summary>
    /// Provides basic functionality to view models
    /// </summary>
    public abstract class ViewModelBase : NotificationObject
    {
        protected WindowManager WindowManager { get; private set; }

        protected ViewModelBase()
        {
            this.WindowManager = new WindowManager();
        }
    }
}